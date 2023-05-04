using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SimulationSettings
{
    [Header("Simulation settings")]

    [Range(0f, 3f)]
    [SerializeField] public float TimeScale = 1f;

    [SerializeField] public float PipeLength = 1f;

    //[SerializeField] public Vector2 CellSize = new Vector2(1f / 256, 1f / 256);

    [SerializeField] public float CellSizeX = 1f;

    [SerializeField] public float CellSizeY = 1f;

    [Range(0, 1f)]
    [SerializeField] public float RainRate = 0.0f;

    [Range(0, 1f)]
    [SerializeField] public float Evaporation = 0.0f;

    [Range(0.1f, 100)]
    [SerializeField] public float PipeArea = 5.0f;

    [Range(0.1f, 20f)]
    [SerializeField] public float Gravity = 9.81f;
    [SerializeField] public bool Bordered = true;

    [Header("Hydraulic erosion settings")]
    [Range(0.01f, 1f)]
    [SerializeField] public float SedimentCapacity = 0.1f;

    [Range(0.01f, 1f)]
    [SerializeField] public float SoilSuspensionRate = 0.2f;

    [Range(0.01f, 1f)]
    [SerializeField] public float SedimentDepositionRate = 0.4f;

    [Range(0.01f, 1f)]
    [SerializeField] public float SedimentSofteningRate = 0.2f;

    [Range(0.1f, 10f)]
    [SerializeField] public float MaximalErosionDepth = 1f;

    [Header("Thermal erosion settings")]
    [Range(0, 3f)]
    [SerializeField] public float ThermalErosionTimeScale = 1.0f;

    [Range(0.01f, 1f)]
    [SerializeField] public float ThermalErosionRate = 0.8f;

    [Range(0.01f, 1f)]
    [SerializeField] public float TalusAngleTangentCoeff = 0.6f;

    [Range(0.01f, 1f)]
    [SerializeField] public float TalusAngleTangentBias = 0.2f;

    [Header("Camera settings")]
    [Range(0.0f,10.0f)]
    [SerializeField] public float MouseSensitivity = 1.5f;

    [Range(0.0f,5.0f)]
    [SerializeField] public float CameraSpeed = 0.5f;

    [Header("Editor settings")]
    [Range(-1.0f,1.0f)]
    [SerializeField] public float ToolStrength = 0.5f;

    [Range(0.001f,0.05f)]
    [SerializeField] public float ToolRadious = 0.05f;

    [Header("Map settings")]
    [Range(1.0f,1000.0f)]
    [SerializeField] public float Scale = 50.0f;

    [Range(0.0f,1000.0f)]
    [SerializeField] public float SeaLevel = 10.0f;

    [Range(0.0f,100.0f)]
    [SerializeField] public float Bias = 0.0f;
    
    [Range(0.0f,0.01f)]
    [SerializeField] public float Frequency = 0.005f;

    [SerializeField] public float Offset = 0.0f;
}