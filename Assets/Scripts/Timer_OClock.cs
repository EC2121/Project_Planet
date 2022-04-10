using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Timer_OClock : MonoBehaviour
{

    public float StartTime;
    public GameObject CutScene;
    public GameObject Ship;
    private bool Going=false;

    private float currentTime;
    private float finalTime;
    private TextMeshProUGUI timer;

    private void OnEnable()
    {
        Player_State_Machine.gamePlayerFinalePhase.AddListener(() => { Going = true; });
    }
    private void OnDisable()
    {
        Player_State_Machine.gamePlayerFinalePhase.RemoveListener(() => { Going = true; });
    }

    private void Awake()
    {
        timer = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        currentTime = StartTime;
    }

    void Update()
    {
        if (Going)
        {
            currentTime -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            timer.SetText($"{time.ToString(@"mm\:ss\:fff")}");
            if (currentTime <= 0)
            {
                //currentTime = 0f;
                CutScene.SetActive(true);
                gameObject.SetActive(false);
                //Ship.SetActive(true);
            }
            // if (currentTime <= 0) //<= altrimenti riparte
            // {
            //    // CutScene.gameObject.SetActive(false);
            //     //StartTime = 0f;
            //     time = TimeSpan.FromSeconds(0);
            //     //SceneManager.LoadScene("UI_MenuScene");
            // }
        }
    }
}
