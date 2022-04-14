using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameUIMGR : MonoBehaviour
{

    GameObject currentMenu;
    public GameObject Menu;
    private GameObject mainMenu;

    private FMOD.Studio.Bus music;
    private FMOD.Studio.Bus sfx;


    private void Awake()
    {
        mainMenu = Menu.transform.GetChild(0).gameObject;
        music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        sfx = FMODUnity.RuntimeManager.GetBus("bus:/SFX");

        music.setVolume(0);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (currentMenu == null)
            {
                mainMenu.SetActive(true);
                currentMenu = mainMenu;
            }
            else
            {
                OnResume();
            }
            
        }
    }

    public void OnVideoQualityChange(int value)
    {
        QualitySettings.SetQualityLevel(value);
    }

    public void OnSFXValueChange(float value)
    {
        sfx.setVolume(value);
    }

    public void OnMusicValueChanged(float value)
    {
        music.setVolume(value);
    }

    public void OnSaveButtonPressed()
    {
        SaveMGR.Save();
    }

    public void OnLoadButtonPressed()
    {
        SaveMGR.Load();
    }

    //public void OnOptionsPressed()
    //{
    //    currentMenu.SetActive(false);
    //    currentMenu = optionsMenu;
    //    currentMenu.SetActive(true);
    //}

    //public void OnAudioPressed()
    //{

    //    currentMenu.SetActive(false);
    //    currentMenu = audioMenu;
    //    currentMenu.SetActive(true);
    //}
    //public void OnVideoPressed()
    //{
    //    currentMenu.SetActive(false);
    //    currentMenu = videoMenu;
    //    currentMenu.SetActive(true);
    //}

    public void OnResume()
    {
        for (int i = 0; i < Menu.transform.childCount; i++)
        {
            Menu.transform.GetChild(i).gameObject.SetActive(false);
        }
        currentMenu = null;
    }

    public void OnQuit()
    {
        SceneManager.LoadScene("UI_MenuScene");
    }
    //public void OnBackPressed()
    //{
    //    currentMenu.SetActive(false);
    //    if (currentMenu == optionsMenu)
    //    {
    //        currentMenu = mainMenu;
    //    }
    //    else if (currentMenu == audioMenu || currentMenu == videoMenu)
    //    {
    //        currentMenu = optionsMenu;
    //    }
    //    currentMenu.SetActive(true);
    //}
}
