using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonPersistent<InputManager>
{
    public InputMaster Controls { get => controls; }
    private static InputMaster controls;
    
    protected override void Awake()
    {
        base.Awake();
        if (controls == null)
        {
            controls = new InputMaster();            
        }      
        controls.Enable();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    
    private void OnDisable()
    {
        if (controls != null) controls.Disable();
    }
}
