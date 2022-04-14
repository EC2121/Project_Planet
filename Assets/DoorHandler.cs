using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public GameObject DoorText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player_State_Machine psm = other.GetComponent<Player_State_Machine>();

            if (psm.HasKey)
            {
                GetComponent<Animator>().SetTrigger("OpenDoor");
                GetComponent<BoxCollider>().enabled = false;
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
