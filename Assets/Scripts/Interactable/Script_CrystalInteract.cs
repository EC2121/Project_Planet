using UnityEngine;

public class Script_CrystalInteract : MonoBehaviour
{
    public GameObject Crystal_GuiInteractWrite;
    private bool crystal_IsActivaded;
    private BoxCollider crystal_Collider;

    private void OnEnable()
    {
        Player_State_Machine.canCrystal.AddListener(() =>
        {
            crystal_IsActivaded = !crystal_IsActivaded;
            crystal_Collider.enabled = !crystal_IsActivaded;
        });
    }

    private void OnDisable()
    {
        Player_State_Machine.canCrystal.RemoveListener(() =>
        {
            crystal_IsActivaded = !crystal_IsActivaded;
            crystal_Collider.enabled = !crystal_IsActivaded;
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Crystal_GuiInteractWrite.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Crystal_GuiInteractWrite.SetActive(false);
    }

    private void Awake()
    {
        crystal_Collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        Crystal_GuiInteractWrite.SetActive(false);
    }

    private void Update()
    {
        if (Crystal_GuiInteractWrite.activeInHierarchy)
            Crystal_GuiInteractWrite.transform.position = Camera.main.WorldToScreenPoint(crystal_Collider.bounds.center + Vector3.up);

        if (crystal_IsActivaded)
            OnInteraction();
    }

    private void OnInteraction()
    {
        Crystal_GuiInteractWrite.SetActive(false);
    }
}
