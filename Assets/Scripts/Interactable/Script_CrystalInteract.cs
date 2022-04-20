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

    private readonly string player = "Player";
    private readonly string Templescne1 = "Environment_TempleArea_1";
    private readonly string Templescne2 = "Environment_TempleArea_2";
    private readonly string Templescne3 = "Environment_TempleArea_3";

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
        SaveSystem.SaveData(gameObject, true);
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
        if (other.CompareTag(player))
        {
            Crystal_GuiInteractWrite.SetActive(true);
            playerCollideWithMe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(player))
        {
            Crystal_GuiInteractWrite.SetActive(false);
            playerCollideWithMe = false;
        }
    }

    private void Awake()
    {
        crystal_Collider = GetComponent<BoxCollider>();
        TempleArea1 = GameObject.Find(Templescne1);
        TempleArea2 = GameObject.Find(Templescne2);
        TempleArea3 = GameObject.Find(Templescne3);
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
        if (crystal_IsActivaded && playerCollideWithMe)
        {
            SaveSystem.InvokeOnSave();
            SceneManager.LoadScene(alienTestNumber);
            Crystal_GuiInteractWrite.SetActive(false);
            crystal_IsActivaded = false;
        }
    }
}