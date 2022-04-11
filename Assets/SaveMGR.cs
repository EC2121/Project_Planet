using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Saves_Scripts;

public class SaveMGR : MonoBehaviour
{
    //TODO remove
    //Editor test bools
    public bool SaveData = false, LoadData = false;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //Load();
    }

    private void Start()
    {
        //dontdestroyonload save number
        if(!SaveSystem.newGame)
            Load();
        else
            Save();
    }

    public static void Save()
    {
        SaveSystem.InvokeOnSave();    
    }
    public static void Load()
    {
        SaveSystem.InvokeOnLoad();    
    }

    void Update()
    {
        if (SaveData)
        {
            SaveSystem.InvokeOnSave();
            SaveData = false;
        }
        
        if (LoadData)
        {
            SaveSystem.InvokeOnLoad();
            LoadData = false;
        }
    }
}
