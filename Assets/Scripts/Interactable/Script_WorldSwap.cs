using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_WorldSwap : MonoBehaviour
{
    public GameObject ActiveGameOBj;
    public GameObject DeactiveGameOBj;

    private void OnTriggerEnter(Collider other)
    {
        ActiveGameOBj.SetActive(true);
        DeactiveGameOBj.SetActive(false);
    }
}
