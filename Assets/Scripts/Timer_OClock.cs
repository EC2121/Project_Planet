using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Saves_Scripts;

[Serializable]
public struct TimerStats
{
    public bool Going;
    public float CurrentTime;
}

public class Timer_OClock : MonoBehaviour
{
    public float StartTime;
    public GameObject CutScene;
    public GameObject Ship;
    private bool going = false;

    private float currentTime;
    private float finalTime;
    private TextMeshProUGUI timer;

    public float CurrentTime
    {
        get => currentTime;
    }

    public bool Going
    {
        get => going;
    }

    private void OnEnable()
    {
        Player_State_Machine.gamePlayerFinalePhase.AddListener(() => { going = true; });
        SaveSystem.OnSave += SaveSystemOnOnSave;
        SaveSystem.OnLoad += SaveSystemOnOnLoad;
    }

    private void SaveSystemOnOnSave(object sender, EventArgs e)
    {
        SaveSystem.SaveData(gameObject, true);
    }

    private void SaveSystemOnOnLoad(object sender, EventArgs e)
    {
        GameData data = SaveSystem.LoadPlayer(true);

        currentTime = data.timer.CurrentTime;
        going = data.timer.Going;
        if (!data.timer.Going)
        {
            timer.SetText("");
        }
    }

    private void OnDisable()
    {
        Player_State_Machine.gamePlayerFinalePhase.RemoveListener(() => { going = true; });
        SaveSystem.OnSave -= SaveSystemOnOnSave;
        SaveSystem.OnLoad -= SaveSystemOnOnLoad;
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
        if (going)
        {
            currentTime -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            timer.SetText($"{time.ToString(@"mm\:ss\:fff")}");
            if (currentTime <= 0)
            {
                CutScene.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}