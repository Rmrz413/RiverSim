// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Basic"",
            ""id"": ""ea8f10bf-6ad8-437a-98af-b48942a18063"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1d5a1711-6ac7-482d-91eb-a30d3ef96c11"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePos"",
                    ""type"": ""PassThrough"",
                    ""id"": ""07dc66bc-e522-41a0-bd0f-f668e0c14a87"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""MouseDelta"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4051f25a-73a5-47b0-b334-f0c64870ac76"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""RMB"",
                    ""type"": ""PassThrough"",
                    ""id"": ""323fb1e0-8cc2-4df3-afac-3fec5c1a2504"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""LMB"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a228e2fe-bab3-446e-97a4-bc7ef5651908"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""db20fd86-76a0-42c5-9665-bc0ed4f08c9d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b543197d-22ca-4282-aa56-3a927f8682c9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e657fa8c-551d-467b-b35d-6aade9ba496a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5aea9863-cfbe-47d4-859d-281c45969b75"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""84dc0875-3feb-43bc-92e4-6aabd571e470"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""2cba52c6-0baa-4e6c-9acb-172283bdfd95"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""89d243c9-3492-4e88-94ac-71bcad7c485d"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""99a1ad92-cb69-4b01-b057-566a44f42ad2"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3d4a8537-acf4-47cf-a6f5-de8a16582ade"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""74d201e3-9495-46a7-81bc-af8ba1f02df8"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9029ef0c-824f-49a9-aeab-7678b8ce665a"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9fe0ae8a-7311-42bf-a6a0-4b24d48ff0f9"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a83a1d6-dfa3-4b68-8f9d-b1930be7965e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f6fcf6d-037a-4c6d-97cf-49bf1a4f907c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Basic
        m_Basic = asset.FindActionMap("Basic", throwIfNotFound: true);
        m_Basic_Movement = m_Basic.FindAction("Movement", throwIfNotFound: true);
        m_Basic_MousePos = m_Basic.FindAction("MousePos", throwIfNotFound: true);
        m_Basic_MouseDelta = m_Basic.FindAction("MouseDelta", throwIfNotFound: true);
        m_Basic_RMB = m_Basic.FindAction("RMB", throwIfNotFound: true);
        m_Basic_LMB = m_Basic.FindAction("LMB", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Basic
    private readonly InputActionMap m_Basic;
    private IBasicActions m_BasicActionsCallbackInterface;
    private readonly InputAction m_Basic_Movement;
    private readonly InputAction m_Basic_MousePos;
    private readonly InputAction m_Basic_MouseDelta;
    private readonly InputAction m_Basic_RMB;
    private readonly InputAction m_Basic_LMB;
    public struct BasicActions
    {
        private @InputMaster m_Wrapper;
        public BasicActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Basic_Movement;
        public InputAction @MousePos => m_Wrapper.m_Basic_MousePos;
        public InputAction @MouseDelta => m_Wrapper.m_Basic_MouseDelta;
        public InputAction @RMB => m_Wrapper.m_Basic_RMB;
        public InputAction @LMB => m_Wrapper.m_Basic_LMB;
        public InputActionMap Get() { return m_Wrapper.m_Basic; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BasicActions set) { return set.Get(); }
        public void SetCallbacks(IBasicActions instance)
        {
            if (m_Wrapper.m_BasicActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnMovement;
                @MousePos.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnMousePos;
                @MousePos.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnMousePos;
                @MousePos.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnMousePos;
                @MouseDelta.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnMouseDelta;
                @MouseDelta.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnMouseDelta;
                @MouseDelta.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnMouseDelta;
                @RMB.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnRMB;
                @RMB.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnRMB;
                @RMB.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnRMB;
                @LMB.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnLMB;
                @LMB.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnLMB;
                @LMB.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnLMB;
            }
            m_Wrapper.m_BasicActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @MousePos.started += instance.OnMousePos;
                @MousePos.performed += instance.OnMousePos;
                @MousePos.canceled += instance.OnMousePos;
                @MouseDelta.started += instance.OnMouseDelta;
                @MouseDelta.performed += instance.OnMouseDelta;
                @MouseDelta.canceled += instance.OnMouseDelta;
                @RMB.started += instance.OnRMB;
                @RMB.performed += instance.OnRMB;
                @RMB.canceled += instance.OnRMB;
                @LMB.started += instance.OnLMB;
                @LMB.performed += instance.OnLMB;
                @LMB.canceled += instance.OnLMB;
            }
        }
    }
    public BasicActions @Basic => new BasicActions(this);
    public interface IBasicActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnMousePos(InputAction.CallbackContext context);
        void OnMouseDelta(InputAction.CallbackContext context);
        void OnRMB(InputAction.CallbackContext context);
        void OnLMB(InputAction.CallbackContext context);
    }
}
