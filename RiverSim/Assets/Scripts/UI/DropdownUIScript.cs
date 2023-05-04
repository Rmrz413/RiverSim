using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownUIScript : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Simulation sim;

    public void DropdownIndexChanged(int idx)
    {
        sim.InputMode = (ToolType)idx;
    }

    // Start is called before the first frame update
    void Start()
    {
        PopulateList();
    }

    private void PopulateList()
    {
        string[] enumNames = Enum.GetNames(typeof(ToolType));
        dropdown.ClearOptions();
        dropdown.AddOptions(enumNames.ToList());
    }
}
