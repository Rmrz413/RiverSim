using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class Simulation : MonoBehaviour
{
    public Material[] Materials;

    public Texture2D InitialState;

    public Material InitHeightMap;

    public ComputeShader ErosionComputeShader;

    [Serializable]    
    public class SimulationSettings
    {
        [Header("Simulation settings")]

        [Range(0f, 10f)]
        public float TimeScale = 1f;

        public float PipeLength = 1f / 256;
        public Vector2 CellSize = new Vector2(1f / 256, 1f / 256);

        [Range(0, 1f)]
        public float RainRate = 0.015f;

        [Range(0, 1f)]
        public float Evaporation = 0.015f;

        [Range(0.001f, 1000)]
        public float PipeArea = 20;

        [Range(0.1f, 20f)]
        public float Gravity = 9.81f;

        [Header("Hydraulic erosion")]
        [Range(0.1f, 3f)]
        public float SedimentCapacity = 1f;

        [Range(0.1f, 2f)]
        public float SoilSuspensionRate = 0.5f;

        [Range(0.1f, 3f)]
        public float SedimentDepositionRate = 1f;

        [Range(0f, 10f)]
        public float SedimentSofteningRate = 5f;

        [Range(0f, 40f)]
        public float MaximalErosionDepth = 10f;

        [Header("Thermal erosion")]
        [Range(0, 1000f)]
        public float ThermalErosionTimeScale = 1f;

        [Range(0, 10f)]
        public float ThermalErosionRate = 0.15f;

        [Range(0f, 10f)]
        public float TalusAngleTangentCoeff = 0.8f;

        [Range(0f, 10f)]
        public float TalusAngleTangentBias = 0.1f;            
    }

    [Header("Simulation")]   
    public SimulationSettings Settings;

    [Header("Editor settings")]
    [Range(0.0f,10.0f)]
    public float Sensitivity = 1.5f;

    [Range(0.0f,5.0f)]
    public float Speed = 0.5f;

    [Range(0.0f,1.0f)]
    public float BucketAmount = 0.5f;

    [Range(0.0f,0.1f)]
    public float BuckerRadious = 0.05f;
    
    [Range(0.0f,0.03f)]
    public float Frequency = 0.01f;

    [Range(1.0f,100.0f)]
    public float Strength = 50.0f;

    public float Offset = 0.0f;

    private int width, height;
    private RenderTexture stateTexture;
    private RenderTexture waterFluxTexture;
    private RenderTexture terrainFluxTexture;
    private RenderTexture velocityTexture;    

    private readonly string[] kernelNames = 
    {
        "RainAndControl",
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
    private bool rmbIsPressed;
    private bool lmbIsPressed;
    private Vector4 inputControls = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    private Plane floor = new Plane(Vector3.up, Vector3.zero);


    private bool paused = false;

    void Awake()    
    {        
        controls = new InputMaster();        
    }

    void Start()
    {
        width = (int)Metrics.WorldSize.x;
        height = (int)Metrics.WorldSize.z;
        controls.Basic.RMB.performed += _ => rmbIsPressed = true;
        controls.Basic.RMB.canceled += _ => rmbIsPressed = false;
        controls.Basic.LMB.performed += _ => lmbIsPressed = true;
        controls.Basic.LMB.canceled += _ => lmbIsPressed = false;
        Initialize();
    }

    void Update()
    {
        Rotate();
        Move();
        Draw();
    }

    void FixedUpdate()
    {
        if (!paused)
        {
            if (ErosionComputeShader != null)
            {
                if (Settings != null)
                {
                    ErosionComputeShader.SetFloat("timeDelta", Time.fixedDeltaTime * Settings.TimeScale);
                    ErosionComputeShader.SetFloat("pipeArea", Settings.PipeArea);
                    ErosionComputeShader.SetFloat("gravity", Settings.Gravity);
                    ErosionComputeShader.SetFloat("pipeLength", Settings.PipeLength);
                    ErosionComputeShader.SetVector("cellSize", Settings.CellSize);
                    ErosionComputeShader.SetFloat("evaporation", Settings.Evaporation);
                    ErosionComputeShader.SetFloat("rainRate", Settings.RainRate);

                    ErosionComputeShader.SetFloat("sedimentCapacity", Settings.SedimentCapacity);
                    ErosionComputeShader.SetFloat("maxErosionDepth", Settings.MaximalErosionDepth);
                    ErosionComputeShader.SetFloat("suspensionRate", Settings.SoilSuspensionRate);
                    ErosionComputeShader.SetFloat("depositionRate", Settings.SedimentDepositionRate);
                    ErosionComputeShader.SetFloat("sedimentSofteningRate", Settings.SedimentSofteningRate);

                    ErosionComputeShader.SetFloat("thermalErosionRate", Settings.ThermalErosionRate);
                    ErosionComputeShader.SetFloat("talusAngleTangentCoeff", Settings.TalusAngleTangentCoeff);
                    ErosionComputeShader.SetFloat("talusAngleTangentBias", Settings.TalusAngleTangentBias);
                    ErosionComputeShader.SetFloat("thermalErosionTimeScale", Settings.ThermalErosionTimeScale); 

                    ErosionComputeShader.SetVector("inputControls", inputControls);               
                }

                foreach (var kernel in kernels)
                {
                    ErosionComputeShader.Dispatch(kernel,
                        stateTexture.width / (int)threadsPerGroupX,
                        stateTexture.height / (int)threadsPerGroupY, 1);
                }       
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

        Graphics.Blit(InitialState, stateTexture, InitHeightMap);

        kernels = new int[kernelNames.Length];
        var i = 0;
        foreach (var kernelName in kernelNames)
        {
            var kernel = ErosionComputeShader.FindKernel(kernelName);;
            kernels[i++] = kernel;
                
            ErosionComputeShader.SetTexture(kernel, "HeightMap", stateTexture);
            ErosionComputeShader.SetTexture(kernel, "VelocityMap", velocityTexture);
            ErosionComputeShader.SetTexture(kernel, "FluxMap", waterFluxTexture);
            ErosionComputeShader.SetTexture(kernel, "TerrainFluxMap", terrainFluxTexture);
        }

        ComputeBuffer gradients = new ComputeBuffer(256, sizeof(float) * 2);
		gradients.SetData(Enumerable.Range(0, 256).Select((i) => GetRandomDirection()).ToArray());
        ErosionComputeShader.SetTexture(kernels.Length, "HeightMap", stateTexture);
        ErosionComputeShader.SetFloat("frequency", Frequency); 
        ErosionComputeShader.SetFloat("strength", Strength);
        ErosionComputeShader.SetFloat("offset", Offset);
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
        ErosionComputeShader.SetFloat("frequency", Frequency); 
        ErosionComputeShader.SetFloat("strength", Strength);
        ErosionComputeShader.SetFloat("offset", Offset);

        ErosionComputeShader.Dispatch(kernels.Length,
        stateTexture.width / (int)threadsPerGroupX,
        stateTexture.height / (int)threadsPerGroupY, 1);
    }

    private void Rotate()
    {
        if (rmbIsPressed)
        {
            Vector2 delta = controls.Basic.MouseDelta.ReadValue<Vector2>();
            Camera.main.transform.localEulerAngles = Camera.main.transform.localEulerAngles + new Vector3(-delta.y, delta.x, 0.0f) * Time.deltaTime * Sensitivity;
        }        
    }

    private void Move()
    {
        Vector2 mouse = controls.Basic.MousePos.ReadValue<Vector2>();
        Vector2 move = controls.Basic.Movement.ReadValue<Vector2>();
        Camera.main.transform.Translate(new Vector3(mouse.x * move.x, 0.0f, mouse.y * move.y) * Time.deltaTime * Speed);
    }

    private void Draw()
    {
        var amount = 0f;
        var brushX = 0f;
        var brushZ = 0f;

        if (lmbIsPressed)
        {
            Vector2 mousePos = controls.Basic.MousePos.ReadValue<Vector2>();
            Ray inputRay = Camera.main.ScreenPointToRay(mousePos);            

            if (floor.Raycast(inputRay, out var hit))
            {
                var location = inputRay.GetPoint(hit);
                amount = BucketAmount;
                brushX = location.x / width;
                brushZ = location.z / height;                
            }
            else
            {
                amount = 0f;
            }            
        }

        inputControls = new Vector4(brushX, brushZ, BuckerRadious, amount);
        Shader.SetGlobalVector("inputControls", inputControls);
    } 

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 100,0,100,50), "ApplyPerlin"))
        {
            ApplyPerlin();
        }
        if (GUI.Button(new Rect(0,0,30,30), "||"))
        {
            paused = !paused;
        }
    }    

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private static Vector2 GetRandomDirection()
	{
		return new Vector2(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)).normalized;
	}
}
