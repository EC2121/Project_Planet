using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIMGR : MonoBehaviour
{

    GameObject currentMenu;
    GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject audioMenu;
    public GameObject videoMenu;
    GameObject prevMenu;


    private void Awake()
    {
        mainMenu = transform.GetChild(2).gameObject;
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

    public void OnOptionsPressed()
    {
        currentMenu.SetActive(false);
        currentMenu = optionsMenu;
        currentMenu.SetActive(true);
    }

    public void OnAudioPressed()
    {

        currentMenu.SetActive(false);
        currentMenu = audioMenu;
        currentMenu.SetActive(true);
    }
    public void OnVideoPressed()
    {
        currentMenu.SetActive(false);
        currentMenu = videoMenu;
        currentMenu.SetActive(true);
    }

    public void OnResume()
    {
        currentMenu.SetActive(false);
        currentMenu = null;
    }

    public void OnQuit()
    {
        //Application.Quit();
    }
    public void OnBackPressed()
    {
        currentMenu.SetActive(false);
        if (currentMenu == optionsMenu)
        {
            currentMenu = mainMenu;
        }
        else if (currentMenu == audioMenu || currentMenu == videoMenu)
        {
            currentMenu = optionsMenu;
        }
        currentMenu.SetActive(true);
    }
}
