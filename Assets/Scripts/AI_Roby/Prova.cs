//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/AI_Roby/Prova.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Prova : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Prova()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Prova"",
    ""maps"": [
        {
            ""name"": ""New action map"",
            ""id"": ""b055cdcd-00d0-4a32-86a6-8958e9ee749c"",
            ""actions"": [
                {
                    ""name"": ""ProvaI"",
                    ""type"": ""Button"",
                    ""id"": ""bbd695d1-cbe3-49d9-af19-7ee2775117c7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dc21d382-0b9c-45d8-88e1-5f291da58aed"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ProvaI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // New action map
        m_Newactionmap = asset.FindActionMap("New action map", throwIfNotFound: true);
        m_Newactionmap_ProvaI = m_Newactionmap.FindAction("ProvaI", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // New action map
    private readonly InputActionMap m_Newactionmap;
    private INewactionmapActions m_NewactionmapActionsCallbackInterface;
    private readonly InputAction m_Newactionmap_ProvaI;
    public struct NewactionmapActions
    {
        private @Prova m_Wrapper;
        public NewactionmapActions(@Prova wrapper) { m_Wrapper = wrapper; }
        public InputAction @ProvaI => m_Wrapper.m_Newactionmap_ProvaI;
        public InputActionMap Get() { return m_Wrapper.m_Newactionmap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NewactionmapActions set) { return set.Get(); }
        public void SetCallbacks(INewactionmapActions instance)
        {
            if (m_Wrapper.m_NewactionmapActionsCallbackInterface != null)
            {
                @ProvaI.started -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnProvaI;
                @ProvaI.performed -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnProvaI;
                @ProvaI.canceled -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnProvaI;
            }
            m_Wrapper.m_NewactionmapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ProvaI.started += instance.OnProvaI;
                @ProvaI.performed += instance.OnProvaI;
                @ProvaI.canceled += instance.OnProvaI;
            }
        }
    }
    public NewactionmapActions @Newactionmap => new NewactionmapActions(this);
    public interface INewactionmapActions
    {
        void OnProvaI(InputAction.CallbackContext context);
    }
}
