using System;
using UnityEngine;
using UnityTemplateProjects.Saves_Scripts;

public class Script_Chest : MonoBehaviour
{
    public GameObject Box_GuiInteractWrite;
    public Transform Player_Chest;

    public bool takeMe;

    private void OnEnable()
    {
        Player_State_Machine.TakeTheBox.AddListener(() => takeMe = !takeMe);
        SaveSystem.OnSave += SaveSystemOnOnSave;
        SaveSystem.OnLoad += SaveSystemOnOnLoad;
    }

    private void SaveSystemOnOnLoad(object sender, EventArgs e)
    {
        GameData data = SaveSystem.LoadPlayer(true);
        transform.position = new Vector3(data.CratePosition[0], data.CratePosition[1], data.CratePosition[2]);
        transform.rotation = new Quaternion(data.CrateRotation[0], data.CrateRotation[1], 
                                            data.CrateRotation[2], data.CrateRotation[3]);
    }

    private void SaveSystemOnOnSave(object sender, EventArgs e)
    {
        SaveSystem.SaveData(gameObject, true);
    }

    private void OnDisable()
    {
        Player_State_Machine.TakeTheBox.RemoveListener(() => takeMe = !takeMe);
        SaveSystem.OnSave -= SaveSystemOnOnSave;
        SaveSystem.OnLoad -= SaveSystemOnOnLoad;
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
