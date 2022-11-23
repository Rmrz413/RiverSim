using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ToolType
{
    Fluid = 0,
    Terrain = 1,
    Hardness = 2,
    Source = 3
}

public class Simulation : MonoBehaviour
{
    public Material[] Materials;

    public Texture2D InitialState;

    public Material InitHeightMap;

    public ComputeShader ErosionComputeShader;

    [Header("Simulation")]
    public SimulationSettings Settings;
    private SettingsManager settingsHolder;

    private int width, height;
    private RenderTexture stateTexture;
    private RenderTexture waterFluxTexture;
    private RenderTexture terrainFluxTexture;
    private RenderTexture velocityTexture;    

    private readonly string[] kernelNames = 
    {
        "Rain",
        "FluxComputation",
        "FluxApply",
        "HydraulicErosion",
        "SedimentAdvection",
        "ThermalErosion",
        "ApplyThermalErosion"        
    };

    private int[] kernels;
    private uint threadsPerGroupX;
    private uint threadsPerGroupY;
    private uint threadsPerGroupZ;
    private const string StateTextureKey = "_StateTex";

    private InputMaster controls;
    
    public ToolType InputMode = 0;
    private bool rmbIsPressed;
    private bool lmbIsPressed;
    private Vector4 inputControls = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    private Plane floor = new Plane(Vector3.up, Vector3.zero);
    private List<Vector4> SrcNDRain;
    [SerializeField]private LineRenderer line;

    void Awake()    
    {        
        settingsHolder = GameObject.FindObjectOfType<SettingsManager>();
        if (settingsHolder != null && settingsHolder.Settings != null)
        {
            Debug.Log("Simulation Settings found, settings loaded.");            
            Settings = settingsHolder.Settings;
            if (settingsHolder.Map != null)
            {
                InitialState = settingsHolder.Map;
            }
        }
        else
        {
            Debug.Log("Settings not found, loading default.");
            Settings = new SimulationSettings();
        }
         
    }

    void Start()
    {
        width = (int)Metrics.MapResolution.x;
        height = (int)Metrics.MapResolution.z;        
        Initialize();
    }

    void Update()
    {
        Rotate();
        Move();
        Editor();
    }

    void FixedUpdate()
    {
        if (ErosionComputeShader != null)
            {
                if (Settings != null)
                {
                    ErosionComputeShader.SetFloat("timeDelta", Time.fixedDeltaTime * Settings.TimeScale);
                    ErosionComputeShader.SetFloat("pipeArea", Settings.PipeArea);
                    ErosionComputeShader.SetFloat("gravity", Settings.Gravity);
                    ErosionComputeShader.SetFloat("pipeLength", Settings.PipeLength);                    
                    ErosionComputeShader.SetFloat("cellSizeX", Settings.CellSizeX);
                    ErosionComputeShader.SetFloat("cellSizeY", Settings.CellSizeY);
                    ErosionComputeShader.SetFloat("evaporation", Settings.Evaporation);
                    ErosionComputeShader.SetFloat("rainRate", Settings.RainRate);
                    ErosionComputeShader.SetBool("bordered", Settings.Bordered);

                    ErosionComputeShader.SetFloat("sedimentCapacity", Settings.SedimentCapacity);
                    ErosionComputeShader.SetFloat("maxErosionDepth", Settings.MaximalErosionDepth);
                    ErosionComputeShader.SetFloat("suspensionRate", Settings.SoilSuspensionRate);
                    ErosionComputeShader.SetFloat("depositionRate", Settings.SedimentDepositionRate);
                    ErosionComputeShader.SetFloat("sedimentSofteningRate", Settings.SedimentSofteningRate);

                    ErosionComputeShader.SetFloat("thermalErosionRate", Settings.ThermalErosionRate);
                    ErosionComputeShader.SetFloat("talusAngleTangentCoeff", Settings.TalusAngleTangentCoeff);
                    ErosionComputeShader.SetFloat("talusAngleTangentBias", Settings.TalusAngleTangentBias);
                    ErosionComputeShader.SetFloat("thermalErosionTimeScale", Settings.ThermalErosionTimeScale);

                    ErosionComputeShader.SetInt("inputMode", (int)InputMode);
                    ErosionComputeShader.SetVector("inputControls", inputControls);
                }

                foreach (var kernel in kernels)
                {                    
                    ErosionComputeShader.Dispatch(kernel,
                        stateTexture.width / (int)threadsPerGroupX,
                        stateTexture.height / (int)threadsPerGroupY, 1);
                }

                foreach(var loc in SrcNDRain)
                {
                    ErosionComputeShader.SetInt("inputMode", 0);
                    ErosionComputeShader.SetVector("inputControls", loc);
                    ErosionComputeShader.Dispatch(7,
                        stateTexture.width / (int)threadsPerGroupX,
                        stateTexture.height / (int)threadsPerGroupY, 1);
                }       
            }        
    }

    void Initialize()
    {
        stateTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBFloat)
        {
            enableRandomWrite = true,
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };        

        waterFluxTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBFloat)
        {
            enableRandomWrite = true,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        terrainFluxTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBFloat)
        {
            enableRandomWrite = true,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Repeat
        };

        velocityTexture = new RenderTexture(width, height, 0, RenderTextureFormat.RGFloat)
        {
            enableRandomWrite = true,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        InitHeightMap.SetTexture("_MainTex", InitialState);
        Graphics.Blit(InitialState, stateTexture, InitHeightMap);

        SrcNDRain = new List<Vector4>(8);

        kernels = new int[kernelNames.Length];
        var i = 0;
        foreach (var kernelName in kernelNames)
        {
            var kernel = ErosionComputeShader.FindKernel(kernelName);
            kernels[i++] = kernel;
        }

        for (int j = 0; j < 9; j++)
        {
            ErosionComputeShader.SetTexture(j, "HeightMap", stateTexture);
            ErosionComputeShader.SetTexture(j, "VelocityMap", velocityTexture);
            ErosionComputeShader.SetTexture(j, "FluxMap", waterFluxTexture);
            ErosionComputeShader.SetTexture(j, "TerrainFluxMap", terrainFluxTexture);
        }

        ComputeBuffer gradients = new ComputeBuffer(256, sizeof(float) * 2);
		gradients.SetData(Enumerable.Range(0, 256).Select((i) => GetRandomDirection()).ToArray());
        ErosionComputeShader.SetTexture(kernels.Length, "HeightMap", stateTexture);
        ErosionComputeShader.SetFloat("frequency", Settings.Frequency); 
        ErosionComputeShader.SetFloat("strength", Settings.Scale);
        ErosionComputeShader.SetFloat("offset", Settings.Offset);
        ErosionComputeShader.SetBuffer(kernels.Length, "gradients", gradients);
        gradients.Release();
            
        ErosionComputeShader.SetInt("width", width);
        ErosionComputeShader.SetInt("height", height);
        ErosionComputeShader.GetKernelThreadGroupSizes(kernels[0], out threadsPerGroupX, out threadsPerGroupY, out threadsPerGroupZ); 

        foreach (var material in Materials)
        {
            material.SetTexture(StateTextureKey, stateTexture);
        }       
    }

    private void ApplyPerlin()
    {
        ErosionComputeShader.SetFloat("frequency", Settings.Frequency); 
        ErosionComputeShader.SetFloat("strength", Settings.Scale);
        ErosionComputeShader.SetFloat("offset", Settings.Offset);

        ErosionComputeShader.Dispatch(8,
        stateTexture.width / (int)threadsPerGroupX,
        stateTexture.height / (int)threadsPerGroupY, 1);
    }

    private void Rotate()
    {
        if (rmbIsPressed)
        {
            Vector2 delta = controls.Basic.MouseDelta.ReadValue<Vector2>();
            Camera.main.transform.localEulerAngles = Camera.main.transform.localEulerAngles + new Vector3(-delta.y, delta.x, 0.0f) * Time.deltaTime * Settings.MouseSensitivity;
        }        
    }

    private void Move()
    {
        Vector2 mouse = controls.Basic.MousePos.ReadValue<Vector2>();
        Vector2 move = controls.Basic.Movement.ReadValue<Vector2>();
        Camera.main.transform.Translate(new Vector3(mouse.x * move.x, 0.0f, mouse.y * move.y) * Time.deltaTime * Settings.CameraSpeed);
    }

    private void Editor()
    {
        Vector2 mousePos = controls.Basic.MousePos.ReadValue<Vector2>();
        Ray inputRay = Camera.main.ScreenPointToRay(mousePos);
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if (floor.Raycast(inputRay, out var hit) && !isOverUI)
        {
            Vector3 location = inputRay.GetPoint(hit);

            line.SetPosition(0, location);
            line.SetPosition(1, location + Vector3.up * 100);            

            if (lmbIsPressed && InputMode != ToolType.Source)
            {
                
                float amount = Settings.ToolStrength;
                float brushX = location.x / width;
                float brushZ = location.z / height;

                inputControls = new Vector4(brushX, brushZ, Settings.ToolRadious, amount);
                ErosionComputeShader.SetVector("inputControls", inputControls);
                ErosionComputeShader.SetInt("inputMode", (int)InputMode);
                ErosionComputeShader.Dispatch(7,
                stateTexture.width / (int)threadsPerGroupX,
                stateTexture.height / (int)threadsPerGroupY, 1);
            }
        }        
    }

    private void SourceAndDrainAdd()
    {
        if (InputMode == ToolType.Source)
        {
            Vector2 mousePos = controls.Basic.MousePos.ReadValue<Vector2>();
            Ray inputRay = Camera.main.ScreenPointToRay(mousePos);
            bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

            if (floor.Raycast(inputRay, out var hit) && !isOverUI)
            {
                var location = inputRay.GetPoint(hit);
                float amount = Settings.ToolStrength;
                float brushX = location.x / width;
                float brushZ = location.z / height;

                SrcNDRain.Add(new Vector4(brushX, brushZ, Settings.ToolRadious, amount));
            }
        }
    }

    private void OnEnable()
    {        
        controls = GameObject.FindObjectOfType<InputManager>()?.Controls;
        if (controls == null)
        {
            Debug.Log("InputManager not found, creating new Input.");
            controls = new InputMaster();
            controls.Enable();
        }
        controls.Basic.RMB.performed += _ => rmbIsPressed = true;
        controls.Basic.RMB.canceled += _ => rmbIsPressed = false;
        controls.Basic.LMB.performed += _ => lmbIsPressed = true;
        controls.Basic.LMB.canceled += _ => lmbIsPressed = false;
        controls.Basic.LMB.performed += _ => SourceAndDrainAdd();
        controls.Basic.R.performed += _ => ApplyPerlin();

        if (settingsHolder != null)
            settingsHolder.OnSettingsChanged += (a,b) =>Settings = settingsHolder.Settings;   
    }

    private void OnDisable()
    {        
        controls.Basic.RMB.performed -= _ => rmbIsPressed = true;
        controls.Basic.RMB.canceled -= _ => rmbIsPressed = false;
        controls.Basic.LMB.performed -= _ => lmbIsPressed = true;
        controls.Basic.LMB.canceled -= _ => lmbIsPressed = false;
        controls.Basic.R.performed -= _ => ApplyPerlin();

        if (GameObject.FindObjectOfType<InputManager>()?.Controls != controls) controls.Disable();        

        if (settingsHolder != null)
            settingsHolder.OnSettingsChanged -= (a,b) =>Settings = settingsHolder.Settings;        
    }

    private static Vector2 GetRandomDirection()
	{
		return new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)).normalized;
	}
}
