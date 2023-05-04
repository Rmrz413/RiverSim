using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private InputMaster controls;

    [SerializeField] private GameObject menu;    

    private void OnButtonPressed()
    {
        menu.SetActive(!menu.activeSelf);
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Debug.Log("InputManager not found, creating new Input.");
        controls = new InputMaster();
        controls.Enable();
        controls.Basic.ESC.performed += _ => OnButtonPressed();
    }

    private void OnDisable()
    {
        controls.Basic.ESC.performed -= _ => OnButtonPressed();
        controls.Disable();
        //if (GameObject.FindObjectOfType<InputManager>()?.Controls != controls) controls.Disable();
    }
    
}
