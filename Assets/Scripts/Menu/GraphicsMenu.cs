using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

public class GraphicsMenu : MonoBehaviour
{
    public Toggle VSync;
    public TMP_Dropdown resDropdown;
    private Resolution[] reversedRes;
    void Start()
    {
        reversedRes = Screen.resolutions.Reverse().ToArray();
        //Rigiro la lista delle risoluzioni altrimenti me le propone dalla più bassa
        foreach (var resolution in reversedRes)
        {
            resDropdown.options.Add((new TMP_Dropdown.OptionData() {text=resolution.ToString()}));
        }
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
}
