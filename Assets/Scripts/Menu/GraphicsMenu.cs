using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

public class GraphicsMenu : MonoBehaviour
{
    public Toggle VSync, HDR;
    public TMP_Dropdown resDropdown, DisplayMode, Shadows, Displays;
    private Resolution[] reversedRes;
    void Start()
    {
        foreach (var display in Display.displays)
        {
            Debug.Log(display.ToString());       
        }
        
        //Preparo la Dropdown per le risoluzioni
        #region Risoluzioni

        reversedRes = Screen.resolutions.Reverse().ToArray();
        //Rigiro la lista delle risoluzioni altrimenti me le propone dalla più bassa
        foreach (var resolution in reversedRes)
        {
            resDropdown.options.Add((new TMP_Dropdown.OptionData() {text=resolution.ToString()}));
        }

        #endregion

        //Preparo la Dropdown per le ombre
        #region Ombre
            foreach (var resolution in Enum.GetValues(typeof(ShadowResolution)))
            {
                Shadows.options.Add((new TMP_Dropdown.OptionData() {text=resolution.ToString()}));
            }
            Debug.Log((int)QualitySettings.shadowResolution);
            Shadows.value = (int)QualitySettings.shadowResolution;
            Shadows.RefreshShownValue();
        #endregion

        //Controllo se l'HDR è disponibile
        #region HDR

        if (!HDROutputSettings.displays[Display.activeEditorGameViewTarget].available)
        {
            HDR.isOn = false;
            HDR.interactable = false;
        }
        else
        {
            HDR.isOn = false;
            HDR.interactable = true;    
        }

        #endregion

    }

    public void SetResolution()
    {
        Screen.SetResolution(reversedRes[resDropdown.value].width, reversedRes[resDropdown.value].height, Screen.fullScreen);
    }

    public void OnVSyncChange()
    {
        if (VSync.isOn)
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1; //Default Framerate
            VSync.transform.GetChild(1).GetComponent<TMP_Text>().text = "On";
        }
        else
        {
            Application.targetFrameRate = 0; //Don't Sync
            VSync.transform.GetChild(1).GetComponent<TMP_Text>().text = "Off";
        }
    }
    
    public void OnDisplayModeChange()
    {
        if (DisplayMode.value == 0)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if (DisplayMode.value == 1)
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void OnShadowsChange()
    {
        QualitySettings.shadowResolution = (ShadowResolution)Shadows.value;
    }

    public void OnHDRChange()
    {
        if (HDR.isOn)
        {
            HDR.transform.GetChild(1).GetComponent<TMP_Text>().text = "On";
            HDROutputSettings.displays[Display.activeEditorGameViewTarget].RequestHDRModeChange(true);
            
            //Camera.main.allowHDR = true;   
        }
        else
        {
            HDR.transform.GetChild(1).GetComponent<TMP_Text>().text = "Off";
            HDROutputSettings.displays[Display.activeEditorGameViewTarget].RequestHDRModeChange(false);
            
            //Camera.main.allowHDR = true;     
        }
    }
}
