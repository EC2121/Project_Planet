using System;
using UnityEngine;
using UnityTemplateProjects.Saves_Scripts;

public class Script_Chest : MonoBehaviour
{
    public GameObject Box_GuiInteractWrite;
    public Transform Player_Chest;

    private bool box_IsTaken;
    private BoxCollider box_Collider;
    private Rigidbody rb;

    private void OnEnable()
    {
        Player_State_Machine.takeTheBox.AddListener(() =>
        {
            box_IsTaken = !box_IsTaken;
            box_Collider.enabled = !box_IsTaken;
        });

        SaveSystem.OnSave += SaveSystemOnOnSave;
        SaveSystem.OnLoad += SaveSystemOnOnLoad;
    }

    private void OnDisable()
    {
        Player_State_Machine.takeTheBox.RemoveListener(() =>
        {
            box_IsTaken = !box_IsTaken;
            box_Collider.enabled = !box_IsTaken;
        });

        SaveSystem.OnSave -= SaveSystemOnOnSave;
        SaveSystem.OnLoad -= SaveSystemOnOnLoad;
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
        rb = GetComponent<Rigidbody>();
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

    /// <summary>
    /// Se non è presa dal giocatore (nel salvataggio) riposiziono la scatola dov'era
    /// </summary>
    private void SaveSystemOnOnLoad(object sender, EventArgs e)
    {
        GameData data = SaveSystem.LoadPlayer(true);

        if (!data.HasBox)
        {
            // posiziona la box
            transform.position = new Vector3(data.CratePosition[0], data.CratePosition[1], data.CratePosition[2]);
            transform.rotation = new Quaternion(data.CrateRotation[0], data.CrateRotation[1],
                data.CrateRotation[2], data.CrateRotation[3]);
        }
    }

    private void SaveSystemOnOnSave(object sender, EventArgs e)
    {
        SaveSystem.SaveData(gameObject, true);
    }

    private void OnInteraction()
    {
        Box_GuiInteractWrite.SetActive(false);
        transform.position = Player_Chest.position;
        transform.rotation = Player_Chest.rotation;
    }
}
