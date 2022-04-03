using UnityEngine;

public class Script_Chest : MonoBehaviour
{
    public GameObject Box_GuiInteractWrite;
    public Transform Player_Chest;

    private bool box_IsTaken;
    private BoxCollider box_Collider;
    private void OnEnable()
    {
        Player_State_Machine.takeTheBox.AddListener(() => box_IsTaken = !box_IsTaken);
    }
    private void OnDisable()
    {
        Player_State_Machine.takeTheBox.RemoveListener(() => box_IsTaken = !box_IsTaken);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Box_GuiInteractWrite.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Box_GuiInteractWrite.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !Box_GuiInteractWrite.activeInHierarchy) Box_GuiInteractWrite.SetActive(true);
    }

    private void Awake()
    {
        box_Collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        Box_GuiInteractWrite.SetActive(false);
    }

    private void Update()
    {
        if (Box_GuiInteractWrite.activeInHierarchy)
            Box_GuiInteractWrite.transform.position = Camera.main.WorldToScreenPoint(box_Collider.bounds.center + ( Vector3.up * 0.5f ));

        if (box_IsTaken)
            OnInteraction();
    }

    private void OnInteraction()
    {
        Box_GuiInteractWrite.SetActive(false);
        transform.SetPositionAndRotation(Player_Chest.position, Player_Chest.rotation);
    }
}
