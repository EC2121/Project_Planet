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
    private bool Going=false;

    private float currentTime;
    private float finalTime;
    private TextMeshProUGUI timer;

    private void OnEnable()
    {
        Script_WaitTheShip.GamePlayer_FinalePhase.AddListener(() => { Going = true; });
    }
    private void OnDisable()
    {
        Script_WaitTheShip.GamePlayer_FinalePhase.RemoveListener(() => { Going = true; });
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
            if (currentTime <= 0) //<= altrimenti riparte
            {
                //TimeEnd.Invoke();
                SceneManager.LoadScene("UI_MenuScene");
            }
        }
    }
}
