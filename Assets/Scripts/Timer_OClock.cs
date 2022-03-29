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
    public static UnityEvent TimeEnd = new UnityEvent();

    public float StartTime;
    public bool Going;

    private float currentTime;
    private float finalTime;
    private TextMeshProUGUI timer;

    private void OnEnable()
    {
        Script_WaitTheShip.GamePlayer_FinalePhase.AddListener(() => { Going = true; });
        //TimeEnd.AddListener(() => { Going = false; });
    }
    private void OnDisable()
    {
        Script_WaitTheShip.GamePlayer_FinalePhase.RemoveListener(() => { Going = true; });
        //TimeEnd.RemoveListener(() => { Going = false; });
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
            if (currentTime == 0)
            {
                //TimeEnd.Invoke();
                SceneManager.LoadScene("");
            }
        }
    }
}
