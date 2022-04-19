using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_WorldSwap : MonoBehaviour
{
    public GameObject ActiveGameOBj;
    public GameObject DeactiveGameOBj;

    public static int DeadChompys;
    private float timer = 6;
    private bool endProve = false;
    private AsyncOperation async;
    private BoxCollider worldTrigger;
    private bool scene_Alien2;

    private void Awake()
    {
        worldTrigger = GetComponent<BoxCollider>();
        if (SceneManager.GetActiveScene().name == "Scene_AlienTest_2")
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
