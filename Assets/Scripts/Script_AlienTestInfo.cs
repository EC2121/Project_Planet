using UnityEngine;

public class Script_AlienTestInfo : MonoBehaviour
{
    private bool Alien1;
    private bool Alien2;
    private bool Alien3;

    private void OnEnable()
    {
        Script_WorldSwap.AlienTest1Done.AddListener(() => Alien1 = true);
        Script_WorldSwap.AlienTest2Done.AddListener(() => Alien2 = true);
        Script_WorldSwap.AlienTest3Done.AddListener(() => Alien3 = true);
    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

    }
}
