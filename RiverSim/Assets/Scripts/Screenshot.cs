using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Screenshot : SingletonPersistent<Screenshot>
{
    private InputMaster controls;
    private string path = "./Screenshots";

    protected override void Awake()
    {
        base.Awake();        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    
    private void Start()
    {
        Debug.Log("Screenshot started");
        controls = GameObject.FindObjectOfType<InputManager>()?.Controls;
        if (controls == null)
        {
            Debug.Log("InputManager not found, creating new Input.");
            controls = new InputMaster();
            controls.Enable();
        }
        controls.Basic.C.performed += _ => OnButtonPressed();
    }

    public void OnButtonPressed()
    {
        int i = 0;
        while (File.Exists($"{path}/{i}.png"))
        {
            i++;
        }
        ScreenCapture.CaptureScreenshot($"{path}/{i}.png");
        Debug.Log("Screenshot taken.");
    }

    private void OnEnable()
    {
        if (controls != null) controls.Basic.C.performed += _ => OnButtonPressed();
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Basic.C.performed -= _ => OnButtonPressed();
            if (GameObject.FindObjectOfType<InputManager>()?.Controls != controls) controls.Disable();
        }        
    }
}
