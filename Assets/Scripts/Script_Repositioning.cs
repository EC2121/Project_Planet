using System;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Saves_Scripts;



public class Script_Repositioning : MonoBehaviour
{
    public bool isCrystalActive;
    public Transform PointToRotate;

    private readonly Dictionary<GameObject, Tuple<Vector3, Quaternion>> templePiece = new Dictionary<GameObject, Tuple<Vector3, Quaternion>>();
    private bool isCrystal = false;
    private GameObject[] gameObjectID;
    private int[] randomrotation;
    private int count;
    private float fixCount = 0;
    private int templeNumber;

    private void OnEnable()
    {
        SaveSystem.OnLoad += SaveSystemOnOnLoad;
    }
    private void OnDisable()
    {
        SaveSystem.OnLoad -= SaveSystemOnOnLoad;
    }
    private void Awake()
    {
        gameObjectID = new GameObject[gameObject.GetComponentsInChildren<Transform>().Length - 1];
        randomrotation = new int[gameObjectID.Length];
        foreach (Transform piece in gameObject.GetComponentsInChildren<Transform>())
        {
            if (ReferenceEquals(piece.gameObject, gameObject)) continue;
            gameObjectID[count] = piece.gameObject;
            templePiece.Add(gameObjectID[count], new Tuple<Vector3, Quaternion>(piece.position, piece.rotation));
            count++;
        }

        if (transform.name == "Environment_TempleArea_1") templeNumber = 0;
        else if (transform.name == "Environment_TempleArea_2") templeNumber = 1;
            else if (transform.name == "Environment_TempleArea_3") templeNumber = 2;

    }

    private void Start()
    {
        for (int i = 0; i < gameObjectID.Length; i++)
        {
            if (gameObjectID[i].CompareTag("StairsExtension"))
            {
                gameObjectID[i].transform.position = new Vector3(UnityEngine.Random.onUnitSphere.x * 60 + PointToRotate.position.x,
                    PointToRotate.position.y + UnityEngine.Random.Range(0, 30),
                    UnityEngine.Random.onUnitSphere.z * 60 + PointToRotate.position.z);
            }
            else
            {
                gameObjectID[i].transform.position = UnityEngine.Random.insideUnitSphere * 30 + PointToRotate.position;
                gameObjectID[i].transform.rotation = UnityEngine.Random.rotation;
            }
            randomrotation[i] = UnityEngine.Random.Range(1, 6);
        }
    }

    private void Update()
    {
        if (isCrystalActive)
        {
            isCrystal = true;
            if (fixCount * 0.08f < 1)
            {

                fixCount += Time.deltaTime;
                for (int i = 0; i < gameObjectID.Length; i++)
                {
                    templePiece.TryGetValue(gameObjectID[i], out Tuple<Vector3, Quaternion> value);
                    gameObjectID[i].transform.position = Vector3.Lerp(gameObjectID[i].transform.position, value.Item1, Time.deltaTime * 0.5f);
                    gameObjectID[i].transform.rotation = Quaternion.Slerp(gameObjectID[i].transform.rotation, value.Item2, Time.deltaTime * 0.5f);
                }
            }
            else if (fixCount > 1)
            {
                for (int i = 0; i < gameObjectID.Length; i++)
                {
                    templePiece.TryGetValue(gameObjectID[i], out Tuple<Vector3, Quaternion> value);
                    gameObjectID[i].transform.position = value.Item1;
                    gameObjectID[i].transform.rotation = value.Item2;
                }
                isCrystalActive = false;
            }
        }

        if (!isCrystal)
        {
            for (int i = 0; i < gameObjectID.Length; i++)
            {
                if (gameObjectID[i].CompareTag("StairsExtension"))
                    gameObjectID[i].transform.RotateAround(PointToRotate.position, Vector3.up, randomrotation[i] * Time.deltaTime);

                else
                    gameObjectID[i].transform.RotateAround(PointToRotate.position, Vector3.one, randomrotation[i] * Time.deltaTime);
            }
        }
    }

    private void SaveSystemOnOnLoad(object sender, EventArgs e)
    {
        GameData data = SaveSystem.LoadPlayer(true);
        if (data.Trials[templeNumber].HasPassedTrial) //isCrystal?
        {
            isCrystalActive = false;
            isCrystal = true;
            
            for (int i = 0; i < gameObjectID.Length; i++)
            {
                templePiece.TryGetValue(gameObjectID[i], out Tuple<Vector3, Quaternion> value);
                gameObjectID[i].transform.position = value.Item1;
                gameObjectID[i].transform.rotation = value.Item2;
            }
        }
        // else if (SaveSystem.PassedTrials[templeNumber]) //se ho passato la prova dell'oggetto che sta chiamando lo script 
        // {
        //     if (!isCrystalActive)
        //     {
        //         isCrystalActive = true;    
        //     }
        //     
        // }
    }
}
