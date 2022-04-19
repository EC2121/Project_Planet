using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Saves_Scripts;

public class DoorHandler : MonoBehaviour
{
    public GameObject DoorText;

    private void OnEnable()
    {
        SaveSystem.OnLoad += SaveSystemOnOnLoad;
    }

    private void SaveSystemOnOnLoad(object sender, EventArgs e)
    {
        GameData data = SaveSystem.LoadPlayer(true);

        if (data.HasKey)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;  
            transform.position = new Vector3(251.029999f, 5.19999981f, 118.370003f);
        }
        else
        {   //Se era scomparso lo riposiziono
            transform.position = new Vector3(251.029999f, 20.4528351f, 118.370003f);
            GetComponent<Animator>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;      
        }
    }

    private void OnDisable()
    {
        SaveSystem.OnLoad -= SaveSystemOnOnLoad;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player_State_Machine psm = other.GetComponent<Player_State_Machine>();

            if (psm.HasKey)
            {
                GetComponent<Animator>().SetTrigger("OpenDoor");
                GetComponent<BoxCollider>().enabled = false;
                SaveMGR.Save();
            }
            else
                DoorText?.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DoorText?.SetActive(false);
    }
}
