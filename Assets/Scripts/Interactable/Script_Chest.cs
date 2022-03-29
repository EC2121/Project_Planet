using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Events;

public class Script_Chest : MonoBehaviour
{
    public GameObject Box_GuiInteractWrite;
    public bool TakeMe;
    public Transform Player_Chest;

    private SphereCollider chest_InteractableCollider;
    private void OnEnable()
    {
        Player_State_Machine.TakeTheBox.AddListener(() => TakeMe = !TakeMe);
    }
    private void OnDisable()
    {
        Player_State_Machine.TakeTheBox.RemoveListener(() => TakeMe = !TakeMe);
    }

    private void OnInteraction()
    {
        //chest_InteractableCollider.enabled = !chest_InteractableCollider.enabled;
        Box_GuiInteractWrite.SetActive(!Box_GuiInteractWrite.activeInHierarchy);
        transform.position = Player_Chest.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Box_GuiInteractWrite.SetActive(true);

        //if(other.CompareTag("SpaceShip"))

    }
    private void OnTriggerExit(Collider other)
    {
        Box_GuiInteractWrite.SetActive(false);
    }

    private void Awake()
    {
        chest_InteractableCollider = GetComponent<SphereCollider>();
    }
    void Start()
    {
        Box_GuiInteractWrite.SetActive(false);
    }

    void Update()
    {
        if (Box_GuiInteractWrite.activeInHierarchy)
            Box_GuiInteractWrite.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1f, 0));

        if (TakeMe)
        {
            OnInteraction();
        }
    }
}
