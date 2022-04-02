using UnityEngine;
using UnityEngine.Events;

public class Script_WaitTheShip : MonoBehaviour
{
    public static UnityEvent GamePlayer_FinalePhase = new UnityEvent();

    private bool takeMe = false;

    private void OnEnable()
    {
        Player_State_Machine.TakeTheBox.AddListener(() => takeMe = !takeMe);
    }

    private void OnDisable()
    {
        Player_State_Machine.TakeTheBox.RemoveListener(() => takeMe = !takeMe);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && takeMe)
            GamePlayer_FinalePhase.Invoke();
    }
}
