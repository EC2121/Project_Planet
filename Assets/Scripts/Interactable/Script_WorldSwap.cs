using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Saves_Scripts;

public class Script_WorldSwap : MonoBehaviour
{
    public static UnityEvent AlienTest1Done = new UnityEvent();
    public static UnityEvent AlienTest2Done = new UnityEvent();
    public static UnityEvent AlienTest3Done = new UnityEvent();

    public GameObject ActiveGameOBj;
    public GameObject DeactiveGameOBj;

    public static int DeadChompys;
    private float timer = 6;
    private bool endProve = false;
    private AsyncOperation async;
    private BoxCollider worldTrigger;
    private bool scene_Alien2;

    private string alienScene2 = "Scene_AlienTest_2";
    private string alienScene3 = "Scene_AlienTest_3";
    private void Awake()
    {
        worldTrigger = GetComponent<BoxCollider>();
        if (SceneManager.GetActiveScene().name == alienScene2)
        {
            worldTrigger.enabled = false;
            scene_Alien2 = true;
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
                SaveSystem.saveType = SaveSystem.SaveType.Load;
                switch (SceneManager.GetActiveScene().name)
                {
                    case "Scene_AlienTest_1":
                        AlienTest1Done.Invoke();
                        SaveSystem.PassedTrials[0] = true;
                        break;
                    case "Scene_AlienTest_2":
                        AlienTest2Done.Invoke();
                        SaveSystem.PassedTrials[1] = true;
                        break;
                    case "Scene_AlienTest_3":
                        AlienTest3Done.Invoke();
                        SaveSystem.PassedTrials[2] = true;
                        break;
                }

                async.allowSceneActivation = true;
            }
        }
        print(DeadChompys);
        if (scene_Alien2)
        {
            if (DeadChompys >= 20)
                worldTrigger.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActiveGameOBj.SetActive(true);
            DeactiveGameOBj.SetActive(false);
            endProve = true;
        }

    }

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
}
