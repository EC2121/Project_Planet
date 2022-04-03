using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_Chest : MonoBehaviour
{
    public GameObject Box_GuiInteractWrite;
    public Transform Player_Chest;
    private Vector3 OldPos;

    private SphereCollider chest_InteractableCollider;

    public bool TakeMe;
    private void OnEnable()
    {
        Player_State_Machine.takeTheBox.AddListener(() => TakeMe = !TakeMe);
    }
    private void OnDisable()
    {
        Player_State_Machine.takeTheBox.RemoveListener(() => TakeMe = !TakeMe);
    }
    private void OnInteraction()
    {
       Box_GuiInteractWrite.SetActive(false);
       OldPos = transform.position;
       transform.position = Player_Chest.position;
       transform.rotation = Player_Chest.rotation;
    }

    private void OnDetach()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Box_GuiInteractWrite.SetActive(true);

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
            Box_GuiInteractWrite.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.6f, 0));

        if (TakeMe)
        {
            OnInteraction();
        }

        if (!TakeMe)
        {
            OnDetach();
        }
    }
}
