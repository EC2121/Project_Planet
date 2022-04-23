using System;
using TMPro;
using UnityEngine;
using System.Linq;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class GraphicsMenu : MonoBehaviour
{
    public Slider Framerate;
    public Toggle VSync, HDR;
    public TMP_Dropdown resDropdown, DisplayMode, Shadows, Displays;
    private Resolution[] reversedRes;
    void Start()
    {
       
        
        
        //Preparo la Dropdown per le risoluzioni
        #region Risoluzioni
        
        reversedRes = Screen.resolutions.Reverse().ToArray();
      
        //Rigiro la lista delle risoluzioni altrimenti me le propone dalla più bassa
        foreach (var resolution in reversedRes)
        {
            resDropdown.options.Add((new TMP_Dropdown.OptionData() {text=resolution.ToString()}));
        }
        resDropdown.itemText.text = Screen.currentResolution.ToString();
        resDropdown.RefreshShownValue();
        #endregion

        //Preparo la Dropdown per le ombre
        #region Ombre
            foreach (var resolution in Enum.GetValues(typeof(ShadowResolution)))
            {
                Shadows.options.Add((new TMP_Dropdown.OptionData() {text=resolution.ToString()}));
            }
            Shadows.value = (int)QualitySettings.shadowResolution;
            Shadows.RefreshShownValue();
        #endregion

        #region HDR

        try
        {
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
        }
        catch (InvalidOperationException e)
        {
            //hdr è disabilitato dalle impostazioni del progetto
            HDR.interactable = false;
            HDR.transform.GetChild(1).GetComponent<TMP_Text>().faceColor = new Color32(39, 39, 39, 255);
        }

        #endregion
        //Mi assicuro d'impostare il vsync, che ci sia o meno
        OnVSyncChange();
        
        
        
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
            VSync.transform.GetChild(1).GetComponent<TMP_Text>().text = "On";
            //Sincronizzo con il framerate del monitor?
            
            Framerate.interactable = false;
            Framerate.transform.GetChild(0).GetComponent<TMP_Text>().text = String.Empty;;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            VSync.transform.GetChild(1).GetComponent<TMP_Text>().text = "Off";
            
            Framerate.interactable = true;
            Framerate.transform.GetChild(0).GetComponent<TMP_Text>().text = ((int)Framerate.value).ToString(); 
            //richiamo il metodo per settare il framerate altrimenti schizza alle stelle
            OnFramerateChange();
        }
    }

    public void OnFramerateChange()
    {
        Framerate.transform.GetChild(0).GetComponent<TMP_Text>().text = Framerate.value.ToString();
        Application.targetFrameRate = (int)Framerate.value;
        //Per sicurezza tolgo il vsync
        QualitySettings.vSyncCount = 0;
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
            
        }
        else
        {
            HDR.transform.GetChild(1).GetComponent<TMP_Text>().text = "Off";
            HDROutputSettings.displays[Display.activeEditorGameViewTarget].RequestHDRModeChange(false);
            
        }
    }
}


