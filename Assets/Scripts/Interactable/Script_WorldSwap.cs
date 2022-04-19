using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Script_WorldSwap : MonoBehaviour
{
    public GameObject ActiveGameOBj;
    public GameObject DeactiveGameOBj;
    float timer = 6;
    bool endProve = false;
    AsyncOperation async;

    private IEnumerator LoadAsyncProcess()
    {
        async = SceneManager.LoadSceneAsync("Gameplay_Scene");
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            Debug.Log("Loading progress: " + ( async.progress * 100 ) + "%");

            if (async.progress >= 0.9f)
            {
                Debug.Log("EndAsync");
            }

            yield return null;
        }
    }

    private void Start()
    {
        StartCoroutine(LoadAsyncProcess());
    }

    private void Update()
    {
        if (endProve)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                async.allowSceneActivation = true;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        ActiveGameOBj.SetActive(true);
        DeactiveGameOBj.SetActive(false);
        endProve = true;
    }
}
