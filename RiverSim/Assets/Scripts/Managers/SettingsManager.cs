using System;
using System.Collections.Generic;
using System.IO;
using SharpConfig;
using UnityEngine;

public class SettingsManager : SingletonPersistent<SettingsManager>
{
    public SimulationSettings Settings { get => settings; }
    public Texture2D Map { get => map; }

    public event EventHandler<EventArgs> OnSettingsChanged;

    [SerializeField]private SimulationSettings settings;
    private Texture2D map;

    [SerializeField] private List<SimulationUIElements> UI = new List<SimulationUIElements>(4);
    private Configuration config = new Configuration();
    private string path = "./config.cfg";

    protected override void Awake()
    {
        base.Awake();
        LoadConfig();
    }

    public void Reset()
    {
        RestoreDefault();
        foreach(var ui in UI)
        {
            UpdateOptionsMenu(ui);
        }        
    }

    public void RestoreDefault()
    {
        settings = new SimulationSettings();
        OnSettingsChanged?.Invoke(this, EventArgs.Empty);
        SaveConfig();
    }

    public void LoadConfig()
    {
        if (!File.Exists(path))
        {
            Debug.Log("Default configs restored.");
            RestoreDefault();
        }
        else
        {
            Debug.Log("Config file loaded.");
            config.Clear();
            config = Configuration.LoadFromFile(path);
            settings = config["Settings"].ToObject<SimulationSettings>();
            OnSettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SaveConfig()
    {
        if (settings == null)
        {
            Debug.Log("Default configs restored.");
            RestoreDefault();
        }
        config.Clear();
        config.Add(Section.FromObject("Settings", settings));
        config.SaveToFile(path);
        Debug.Log("Configs saved.");
    }

    public void SelectMap(Texture2D slcmap)
    {
        Debug.Log($"Selected map change: {slcmap.name}");        
        map = slcmap;        
    }

    public void DeselectMap()
    {
        Debug.Log("Map deselected.");
        map = null;
    }

    public void AddUIReference(SimulationUIElements ui)
    {
        UI.Add(ui);
        UpdateOptionsMenu(ui);
        SubscribeOptionsMenu(ui);
    }

    private void SubscribeOptionsMenu(SimulationUIElements ui)
    {
        if (settings != null)
        {
            if (ui != null)
            {
                #region Simulation
                ui.TimeScaleSlider.onValueChanged.AddListener((v) => SetTimeScale(v));
                ui.RainRateSlider.onValueChanged.AddListener((v) => SetRainRate(v));
                ui.EvaporationSlider.onValueChanged.AddListener((v) => SetEvaporation(v));
                ui.GravitySlider.onValueChanged.AddListener((v) => SetGravity(v));
                ui.Bordered.onValueChanged.AddListener((v) => SetBorders(v));
                #endregion

                #region HydroEro
                ui.SedimentCapacitySlider.onValueChanged.AddListener((v) => SetSedimentCapacity(v));
                ui.SoilSuspensionRateSlider.onValueChanged.AddListener((v) => SetSoilSuspensionRate(v));
                ui.SedimentDepositionRateSlider.onValueChanged.AddListener((v) => SetSedimentDepositionRate(v));
                ui.SedimentSofteningRateSlider.onValueChanged.AddListener((v) => SetSedimentSofteningRate(v));
                ui.MaximalErosionDepthSlider.onValueChanged.AddListener((v) => SetMaximalErosionDepth(v));
                #endregion

                #region ThermalEro
                ui.ThermalErosionTimeScaleSlider.onValueChanged.AddListener((v) => SetThermalErosionTimeScale(v));
                ui.ThermalErosionRateSlider.onValueChanged.AddListener((v) => SetThermalErosionRate(v));
                ui.TalusAngleTangentCoeffSlider.onValueChanged.AddListener((v) => SetTalusAngleTangentCoeff(v));
                ui.TalusAngleTangentBiasSlider.onValueChanged.AddListener((v) => SetTalusAngleTangentBias(v));
                #endregion

                #region Tool
                if(ui.ToolStrengthSlider != null) ui.ToolStrengthSlider.onValueChanged.AddListener((v) => SetToolStrenght(v));
                if(ui.ToolRadiousSlider != null) ui.ToolRadiousSlider.onValueChanged.AddListener((v) => SetToolRadious(v));
                #endregion
            }            
        }
    }

    // private void UnsubscribeOptionsMenu(SimulationUIElements ui)
    // {
    //     if (settings != null)
    //     {
    //         if (ui != null)
    //         {
    //             #region Simulation
    //             ui.TimeScaleSlider.onValueChanged.RemoveListener((v) => SetTimeScale(v));
    //             ui.RainRateSlider.onValueChanged.RemoveListener((v) => SetRainRate(v));
    //             ui.EvaporationSlider.onValueChanged.RemoveListener((v) => SetEvaporation(v));
    //             ui.GravitySlider.onValueChanged.RemoveListener((v) => SetGravity(v));
    //             ui.Bordered.onValueChanged.RemoveListener((v) => SetBorders(v));
    //             #endregion

    //             #region HydroEro
    //             ui.SedimentCapacitySlider.onValueChanged.RemoveListener((v) => SetSedimentCapacity(v));
    //             ui.SoilSuspensionRateSlider.onValueChanged.RemoveListener((v) => SetSoilSuspensionRate(v));
    //             ui.SedimentDepositionRateSlider.onValueChanged.RemoveListener((v) => SetSedimentDepositionRate(v));
    //             ui.SedimentSofteningRateSlider.onValueChanged.RemoveListener((v) => SetSedimentSofteningRate(v));
    //             ui.MaximalErosionDepthSlider.onValueChanged.RemoveListener((v) => SetMaximalErosionDepth(v));
    //             #endregion

    //             #region ThermalEro
    //             ui.ThermalErosionTimeScaleSlider.onValueChanged.RemoveListener((v) => SetThermalErosionTimeScale(v));
    //             ui.ThermalErosionRateSlider.onValueChanged.RemoveListener((v) => SetThermalErosionRate(v));
    //             ui.TalusAngleTangentCoeffSlider.onValueChanged.RemoveListener((v) => SetTalusAngleTangentCoeff(v));
    //             ui.TalusAngleTangentBiasSlider.onValueChanged.RemoveListener((v) => SetTalusAngleTangentBias(v));
    //             #endregion
    //         }            
    //     }
    // }

    public void UpdateOptionsMenu(SimulationUIElements ui)
    {
        if (settings != null)
        {
            if (ui != null)
            {
                #region Simulation
                ui.TimeScaleSlider.value = settings.TimeScale;                
                ui.RainRateSlider.value = settings.RainRate;
                ui.EvaporationSlider.value = settings.Evaporation;
                ui.GravitySlider.value = settings.Gravity;
                ui.Bordered.isOn = settings.Bordered;
                #endregion

                #region HydroEro
                ui.SedimentCapacitySlider.value = settings.SedimentCapacity;
                ui.SoilSuspensionRateSlider.value = settings.SoilSuspensionRate;
                ui.SedimentDepositionRateSlider.value = settings.SedimentDepositionRate;
                ui.SedimentSofteningRateSlider.value = settings.SedimentSofteningRate;
                ui.MaximalErosionDepthSlider.value = settings.MaximalErosionDepth;
                #endregion

                #region ThermalEro
                ui.ThermalErosionTimeScaleSlider.value = settings.ThermalErosionTimeScale;
                ui.ThermalErosionRateSlider.value = settings.ThermalErosionRate;
                ui.TalusAngleTangentCoeffSlider.value = settings.TalusAngleTangentCoeff;
                ui.TalusAngleTangentBiasSlider.value = settings.TalusAngleTangentBias;
                #endregion

                Debug.Log("UI Updated.");
            }            
        }
    }    

    #region Setters
    #region Simulation
    public void SetTimeScale(float timeScale)
    {
        Debug.Log("Time scale changed");
        settings.TimeScale = timeScale;        
    }

    public void SetRainRate(float rainRate)
    {
        Debug.Log("Rain rate changed");
        settings.RainRate = rainRate;
    }

    public void SetEvaporation(float evaporation)
    {
        Debug.Log("Evaporation changed");
        settings.Evaporation = evaporation;  
    }

    public void SetGravity(float gravity)
    {
        Debug.Log("Gravity changed");
        settings.Gravity = gravity;  
    }

    public void SetBorders(bool bordered)
    {
        Debug.Log("Borders changed");
        settings.Bordered = bordered;
    }
    #endregion
    #region HydraulicErosion
    public void SetSedimentCapacity(float sedimentcap)
    {
        Debug.Log("Sediment capacity changed");
        settings.SedimentCapacity = sedimentcap;        
    }

    public void SetSoilSuspensionRate(float soilSusRate)
    {
        Debug.Log("Soil suspension rate changed");
        settings.SoilSuspensionRate = soilSusRate;        
    }

    public void SetSedimentDepositionRate(float sedimentDepoRate)
    {
        Debug.Log("Sediment deposition rate changed");
        settings.SedimentDepositionRate = sedimentDepoRate;        
    }

    public void SetSedimentSofteningRate(float sediSoftRate)
    {
        Debug.Log("Sediment softening rate changed");
        settings.SedimentSofteningRate = sediSoftRate;        
    }

    public void SetMaximalErosionDepth(float maxEroDepth)
    {
        Debug.Log("Maximal erosion depth changed");
        settings.MaximalErosionDepth = maxEroDepth;        
    }
    #endregion
    #region ThermalErosion
    public void SetThermalErosionTimeScale(float thermalTimeScale)
    {
        Debug.Log("Thermal erosion time scale changed");
        settings.ThermalErosionTimeScale = thermalTimeScale;        
    }

    public void SetThermalErosionRate(float thermalEroRate)
    {
        Debug.Log("Thermal erosion rate changed");
        settings.ThermalErosionRate = thermalEroRate;        
    }

    public void SetTalusAngleTangentCoeff(float talusTangCoeff)
    {
        Debug.Log("Talus angle tangent coeff changed");
        settings.TalusAngleTangentCoeff = talusTangCoeff;        
    }

    public void SetTalusAngleTangentBias(float talusTangBias)
    {
        Debug.Log("Talus angle tangent bias changed");
        settings.TalusAngleTangentBias = talusTangBias;        
    }
    #endregion

    public void SetToolStrenght(float strength)
    {
        Debug.Log("Tool radious changed");
        settings.ToolStrength = strength;        
    }

    public void SetToolRadious(float radious)
    {
        Debug.Log("Tool radious changed");
        settings.ToolRadious = radious;        
    }
    #endregion
}
