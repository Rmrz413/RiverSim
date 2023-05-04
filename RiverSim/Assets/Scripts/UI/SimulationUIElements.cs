using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class SimulationUIElements
{
    [Header("Simulation settings")]
    public Slider TimeScaleSlider = null;
    public TMP_InputField TimeScaleInput = null;

    public Slider RainRateSlider = null;
    public TMP_InputField RainRateInput = null;

    public Slider EvaporationSlider = null;
    public TMP_InputField EvaporationInput = null;

    public Slider GravitySlider = null;
    public TMP_InputField GravityInput = null;

    public Toggle Bordered = null;


    [Header("Hydraulic erosion settings")]
    public Slider SedimentCapacitySlider = null;
    public TMP_InputField SedimentCapacityInput = null;

    public Slider SoilSuspensionRateSlider = null;
    public TMP_InputField SoilSuspensionRateInput = null;

    public Slider SedimentDepositionRateSlider = null;
    public TMP_InputField SedimentDepositionRateInput = null;

    public Slider SedimentSofteningRateSlider = null;
    public TMP_InputField SedimentSofteningRateInput = null;

    public Slider MaximalErosionDepthSlider = null;
    public TMP_InputField MaximalErosionDepthInput = null;


    [Header("Thermal erosion settings")]
    public Slider ThermalErosionTimeScaleSlider = null;
    public TMP_InputField ThermalErosionTimeScaleInput = null;

    public Slider ThermalErosionRateSlider = null;
    public TMP_InputField ThermalErosionRateInput = null;

    public Slider TalusAngleTangentCoeffSlider = null;
    public TMP_InputField TalusAngleTangentCoeffInput = null;

    public Slider TalusAngleTangentBiasSlider = null;
    public TMP_InputField TalusAngleTangentBiasInput = null;

    [Header("Tool Settings")]
    public Slider ToolStrengthSlider = null;
    public TMP_InputField ToolStrengthInput = null;

    public Slider ToolRadiousSlider = null;
    public TMP_InputField ToolRadiousInput = null;
}
