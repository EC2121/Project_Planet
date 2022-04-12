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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator _LoadScene()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
           // m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //m_Text.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                
               
                //SaveSystem.InvokeOnLoad();
                if (Keyboard.current.spaceKey.isPressed)
                {
                    //GameObject.Find("Test").
                    //SaveSystem.InvokeOnLoad();
                    
                    asyncOperation.allowSceneActivation = true;    
                }
            }

            yield return null;
        }
    }
    
    public void LoadScene()
    {
        //StartCoroutine(_LoadScene());
        SaveSystem.saveType = SaveSystem.SaveType.Load;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
