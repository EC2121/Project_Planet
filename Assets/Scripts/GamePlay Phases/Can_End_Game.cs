using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Can_End_Game : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Player_State_Machine.hasBox)
        {
            SceneManager.LoadScene("UI_MenuScene");
        }
    }
}
