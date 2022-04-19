using UnityEngine;

public class Script_CloseDoor : MonoBehaviour
{
    public OcclusionPortal Door_Portal;
    private int animator_Ash_CloseDoor;

    private void Awake()
    {
        animator_Ash_CloseDoor = Animator.StringToHash("CloseDoor");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger(animator_Ash_CloseDoor);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void ActivateOcclusionPortal()
    {
        Door_Portal.open = !Door_Portal.open;
    }
}
