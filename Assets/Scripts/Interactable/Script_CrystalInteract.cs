using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_CrystalInteract : MonoBehaviour
{
    public GameObject Crystal_GuiInteractWrite;
    public GameObject Crystal_ConnectedTemple;

    [SerializeField] private int alienTestNumber;

    private bool crystal_IsActivaded;
    private BoxCollider crystal_Collider;

    private void OnEnable()
    {
        Player_State_Machine.canCrystal.AddListener(() =>
        {
            OnInteraction();
            crystal_Collider.enabled = !crystal_IsActivaded;
        });
    }

    private void OnDisable()
    {
        Player_State_Machine.canCrystal.RemoveListener(() =>
        {
            OnInteraction();
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
        crystal_IsActivaded = true;
    }

    private void Update()
    {
        if (Crystal_GuiInteractWrite.activeInHierarchy)
            Crystal_GuiInteractWrite.transform.position = Camera.main.WorldToScreenPoint(crystal_Collider.bounds.center + Vector3.up);
    }

    private void OnInteraction()
    {
        if (crystal_IsActivaded)
        {
            switch (alienTestNumber)
            {
                case 0:
                    SceneManager.LoadScene("Scene_AlienTest_1");
                    break;
                case 1:
                    SceneManager.LoadScene("Scene_AlienTest_2");
                    break;
                case 2:
                    SceneManager.LoadScene("Scene_AlienTest_3");
                    break;
            }
            
            Crystal_GuiInteractWrite.SetActive(false);
            crystal_IsActivaded = false;
        }
    }
}

