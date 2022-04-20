using UnityEngine;
using UnityEngine.SceneManagement;

public class Can_End_Game : MonoBehaviour
{
    private string player = "Player";
    private string gameName = "UI_MenuScene";
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(player) && Player_State_Machine.hasBox)
        {
            SceneManager.LoadScene(gameName);
        }
    }
}
