using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_Chest : MonoBehaviour
{
    public GameObject prova;
    public bool TakeMe;

    private SphereCollider chest_InteractableCollider;

    private void OnInteraction()
    {
        chest_InteractableCollider.enabled = !chest_InteractableCollider.enabled;
        prova.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        prova.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        prova.SetActive(false);
    }

    private void Awake()
    {
        chest_InteractableCollider = GetComponent<SphereCollider>();
    }
    void Start()
    {
        prova.SetActive(false);
    }

    void Update()
    {
        if (prova.activeInHierarchy)
            prova.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1f, 0));

        if (TakeMe)
        {
            OnInteraction();
            TakeMe = false;
        }
    }
}
