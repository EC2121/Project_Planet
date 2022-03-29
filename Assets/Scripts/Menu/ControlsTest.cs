using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.Layouts;

public class ControlsTest : MonoBehaviour
{
    public GameObject SettingsPanel, MainButtonsPanel;
    private OnScreenButton backButton;
    private OnScreenControl button;

    
    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    private void Start()
    {
        backButton = GetComponent<OnScreenButton>();
    }

    private void Update()
    {
        
        if (backButton.control.IsPressed())
        {
            //GetComponent<EventTrigger>().OnPointerClick(BaseEventData);
            Debug.Log("Pippo");
        }    
    }
}
