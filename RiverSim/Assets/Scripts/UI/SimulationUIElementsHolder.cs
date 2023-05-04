using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationUIElementsHolder : MonoBehaviour
{
    [SerializeField] private SimulationUIElements[] UIs;    
    
    private void Start()
    {
        SettingsManager SM = FindObjectOfType<SettingsManager>();
        if (SM != null)
        for (int i = 0; i < UIs.Length; i++)
        {
            SM.AddUIReference(UIs[i]);            
        }
        Debug.Log("UI elements connected.");
    }
}
