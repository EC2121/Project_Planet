using UnityEngine;

public class Script_Chest : MonoBehaviour
{
    public GameObject Box_GuiInteractWrite;
    public Transform Player_Chest;

    public bool takeMe;

    private void OnEnable()
    {
        Player_State_Machine.TakeTheBox.AddListener(() => takeMe = !takeMe);
    }

    private void OnDisable()
    {
        Player_State_Machine.TakeTheBox.RemoveListener(() => takeMe = !takeMe);
    }

    private void OnInteraction()
    {
        Box_GuiInteractWrite.SetActive(false);
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

    void Start()
    {
        Box_GuiInteractWrite.SetActive(false);
    }

    void Update()
    {
        if (Box_GuiInteractWrite.activeInHierarchy)
            Box_GuiInteractWrite.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.6f, 0));

        if (takeMe)
            OnInteraction();

        if (!takeMe)
            OnDetach();
    }
}
