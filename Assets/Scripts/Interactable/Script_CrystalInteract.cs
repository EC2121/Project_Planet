using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects.Saves_Scripts;

public class Script_CrystalInteract : MonoBehaviour
{
    public GameObject Crystal_GuiInteractWrite;
    public GameObject Crystal_ConnectedTemple;

    [SerializeField] private int alienTestNumber;

    private bool crystal_IsActivaded;
    private BoxCollider crystal_Collider;
    private bool playerCollideWithMe;

    private GameObject TempleArea1, TempleArea2, TempleArea3;

    private void OnEnable()
    {
        SaveSystem.OnSave += SaveSystemOnOnSave;
        SaveSystem.OnLoad += SaveSystemOnOnLoad;
        Player_State_Machine.canCrystal.AddListener(() =>
        {
            OnInteraction();
            crystal_Collider.enabled = !crystal_IsActivaded;
        });
    }

    private void SaveSystemOnOnLoad(object sender, EventArgs e)
    {
        GameData data = SaveSystem.LoadPlayer(true);
        TempleArea1.GetComponent<Script_Repositioning>().isCrystalActive = data.Trials[0].HasPassedTrial;
        TempleArea2.GetComponent<Script_Repositioning>().isCrystalActive = data.Trials[1].HasPassedTrial;
        TempleArea3.GetComponent<Script_Repositioning>().isCrystalActive = data.Trials[2].HasPassedTrial;
    }

    private void SaveSystemOnOnSave(object sender, EventArgs e)
    {
        SaveSystem.SaveData(this.gameObject, true);
    }

    private void OnDisable()
    {
        SaveSystem.OnSave -= SaveSystemOnOnSave;
        SaveSystem.OnLoad -= SaveSystemOnOnLoad;
        Player_State_Machine.canCrystal.RemoveListener(() =>
        {
            OnInteraction();
            crystal_Collider.enabled = !crystal_IsActivaded;
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Crystal_GuiInteractWrite.SetActive(true);
            playerCollideWithMe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Crystal_GuiInteractWrite.SetActive(false);
            playerCollideWithMe = false;
        }
    }

    private void Awake()
    {
        crystal_Collider = GetComponent<BoxCollider>();
        TempleArea1 = GameObject.Find("Environment_TempleArea_1");
        TempleArea2 = GameObject.Find("Environment_TempleArea_2");
        TempleArea3 = GameObject.Find("Environment_TempleArea_3");
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

        Debug.Log(this.alienTestNumber + "" + this.gameObject.name);

    }

    private void OnInteraction()
    {
        Debug.Log(gameObject.name);
        if (crystal_IsActivaded && playerCollideWithMe)
        {
            SaveSystem.InvokeOnSave();
            SceneManager.LoadScene(this.alienTestNumber);
            Crystal_GuiInteractWrite.SetActive(false);
            crystal_IsActivaded = false;
        }
    }
}