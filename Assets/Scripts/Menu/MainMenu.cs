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
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                if (Keyboard.current.spaceKey.isPressed)
                {
                    asyncOperation.allowSceneActivation = true;    
                }
            }

            yield return null;
        }
    }
    
    public void LoadScene()
    {
        SaveSystem.saveType = SaveSystem.SaveType.Load;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
