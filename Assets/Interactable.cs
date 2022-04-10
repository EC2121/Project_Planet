using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public delegate void OnKeyTaken();
    public static OnKeyTaken OnKeyTakenDel;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnKeyTakenDel?.Invoke();
            this.gameObject.SetActive(false);
        }
    }
   
    // Update is called once per frame
    void Update()
    {

    }
}
