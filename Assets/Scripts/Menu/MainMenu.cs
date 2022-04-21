using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Saves_Scripts;

public class MainMenu : MonoBehaviour
{
    public void PlayScene()
    {
        SaveSystem.saveType = SaveSystem.SaveType.SaveOnNewGame;
        // Scene scene = SceneManager.GetSceneByName("Gameplay_Scene");
        // SceneManager.SetActiveScene(scene);
        GameObject.FindGameObjectWithTag("SceneMGR").name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // private IEnumerator _LoadScene()
    // {
    //     // yield return null;
    //     // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //     //AsyncOperation asyncOperation = 
    //     // asyncOperation.allowSceneActivation = false;
    //     //
    //     // while (!asyncOperation.isDone)
    //     // {
    //     //     if (asyncOperation.progress >= 0.9f)
    //     //     {
    //     //
    //     //         asyncOperation.allowSceneActivation = true;    
    //     //
    //     //     }
    //     //
    //     //     yield return null;
    //     //}
    // }



    private void OnEnable()
    {
        GameObject.FindGameObjectWithTag("SceneMGR").name = SceneManager.GetActiveScene().name;
    }

    public void LoadScene()
    {
        SaveSystem.saveType = SaveSystem.SaveType.Load;
        GameObject.FindGameObjectWithTag("SceneMGR").name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Debug.Log("LoadScene: "+ GameObject.FindGameObjectWithTag("SceneMGR").name);
    }
    

    public void QuitGame()
    {
        Application.Quit();
    }
}
