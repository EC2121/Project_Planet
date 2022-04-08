using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Saves_Scripts;

public class SaveMGR : MonoBehaviour
{
    //TODO remove
    //Editor test bools
    public bool SaveData = false, LoadData = false;

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
