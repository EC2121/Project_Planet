using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Saves_Scripts;

public class SaveMGR : MonoBehaviour
{
    //TODO remove
    //Editor test bools
    public bool SaveData = false, LoadData = false;
    public GameObject sceneMGR ;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
        if (GameObject.FindGameObjectWithTag("SceneMGR") is null)
        {
            sceneMGR = GameObject.Instantiate(Resources.Load("sceneMGR") as GameObject);
            sceneMGR.name = sceneMGR.scene.name;
        }
    }

    private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //UI_MenuScene
        //Debug.Log("OnSceneLoaded: "+ GameObject.FindGameObjectWithTag("SceneMGR").name);
        if (//arg0.name =="Scene_AlienTest_1" ||arg0.name =="Scene_AlienTest_2" ||arg0.name =="Scene_AlienTest_3"  ||
            sceneMGR.name == "Scene_AlienTest_1" || sceneMGR.name =="Scene_AlienTest_2" || sceneMGR.name =="Scene_AlienTest_3") //se sto andando in una di queste scene o se sto tornando
            // GameObject.FindGameObjectWithTag("SceneMGR") != null &&
            // GameObject.FindGameObjectWithTag("SceneMGR").name != "UI_MenuScene" ||
            // SaveSystem.saveType == SaveSystem.SaveType.SaveOnNewGame ||
            // SaveSystem.saveType == SaveSystem.SaveType.Save) 
        {
            //SaveSystem.InvokeOnSave(); //se sto andando    //todo
            if (arg0.name == "Gameplay_Scene") //se sto tornando per abilitare
            {
                
                SaveSystem.InvokeOnLoad();
                //SaveSystem.InvokeOnSave(); NO
            }
            Time.timeScale = 1f;
            print("SceneManagerOnsceneLoaded");
            GameObject.FindGameObjectWithTag("SceneMGR").name = arg0.name;
        }
        Time.timeScale = 1f;
        
    }

    private void Start()
    {
        //dontdestroyonload save number
        if (SaveSystem.saveType == SaveSystem.SaveType.SaveOnNewGame ||
            SaveSystem.saveType == SaveSystem.SaveType.Save)
        {
            Save();        
        }
        else if (SaveSystem.saveType == SaveSystem.SaveType.Load ) //|| GameObject.FindGameObjectWithTag("SceneMGR").name.Contains("AlienTest")
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
