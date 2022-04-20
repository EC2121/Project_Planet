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

    private Player_Controller input;
    private bool escapeButton = false;
    private bool isEscapePressed = false;
    private void Awake()
    {
        mainMenu = Menu.transform.GetChild(0).gameObject;
        music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        sfx = FMODUnity.RuntimeManager.GetBus("bus:/SFX");

        input = new Player_Controller();

        input.Player.Menù.started += OnMenu;
        input.Player.Menù.canceled += OnMenu;
    }

    private void OnMenu(InputAction.CallbackContext context)
    {
        escapeButton = context.ReadValueAsButton();
        isEscapePressed = false;
    }
    private void Update()
    {
        Debug.Log(escapeButton);
        if (escapeButton && !isEscapePressed)
        {
            isEscapePressed = true;
            if (currentMenu == null)
            {
                mainMenu.SetActive(true);
                currentMenu = mainMenu;
                Time.timeScale = 0f;
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

   

    public void OnResume()
    {
        for (int i = 0; i < Menu.transform.childCount; i++)
        {
            Menu.transform.GetChild(i).gameObject.SetActive(false);
        }
        currentMenu = null;
        Time.timeScale = 1f;
    }

    public void OnQuit()
    {
        SceneManager.LoadScene("UI_MenuScene");
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}
