using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Script_WaitTheShip : MonoBehaviour
{
    public static UnityEvent GamePlayer_FinalePhase = new UnityEvent();

    private bool TakeMe = false;

    private void OnEnable()
    {
        Player_State_Machine.takeTheBox.AddListener(() => TakeMe = !TakeMe);
    }
    private void OnDisable()
    {
        Player_State_Machine.takeTheBox.RemoveListener(() => TakeMe = !TakeMe);
    }
}
