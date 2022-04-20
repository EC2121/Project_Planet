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
        SceneManager.activeSceneChanged += SceneManagerOnactiveSceneChanged;
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
    }

    private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (SaveSystem.saveType != SaveSystem.SaveType.None)
        {
            SaveSystem.InvokeOnSave();   
        }
    }

    private void SceneManagerOnactiveSceneChanged(Scene arg0, Scene arg1)
    {
    }
    

    private void Start()
    {
        //dontdestroyonload save number
        if (SaveSystem.saveType == SaveSystem.SaveType.SaveOnNewGame ||
            SaveSystem.saveType == SaveSystem.SaveType.Save)
        {
            Save();        
        }
        else if (SaveSystem.saveType == SaveSystem.SaveType.Load)
        {
            Load();  
        }

        SaveSystem.saveType = SaveSystem.SaveType.None;
    }

    public static void Save()
    {
        SaveSystem.DirectoryCheck();
        SaveSystem.InvokeOnSave();  
        SaveSystem.WriteOnFile();
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
