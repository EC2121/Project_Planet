using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Script_WaitTheShip : MonoBehaviour
{
    public static UnityEvent GamePlayer_FinalePhase = new UnityEvent(); 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")||other.CompareTag("Roby") /*have Box Equipped true*/)
            GamePlayer_FinalePhase.Invoke();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
