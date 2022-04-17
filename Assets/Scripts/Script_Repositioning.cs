using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Script_Repositioning : MonoBehaviour
{
    private readonly Dictionary<GameObject, Tuple<Vector3, Quaternion>> templePiece = new Dictionary<GameObject, Tuple<Vector3, Quaternion>>();
    private bool isCrystal;
    public bool isCrystalActive;
    private GameObject[] gameObjectID;
    private int[] randomrotation;
    private int count;
    private Vector3 vectorTry = Vector3.zero;
    private readonly float floatTry = 0;
    public Transform PointToRotate;

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
    }

    private void Start()
    {
        for (int i = 0; i < gameObjectID.Length; i++)
        {
            gameObjectID[i].transform.position = UnityEngine.Random.insideUnitSphere * 20 + PointToRotate.position;
            //gameObjectID[i].transform.position = UnityEngine.Random. * 20 + PointToRotate.position
            gameObjectID[i].transform.rotation = UnityEngine.Random.rotation;
            //randomrotation[i] = UnityEngine.Random.Range(-3, 3);
        }

    }


    private void Update()
    {
        if (isCrystalActive)
        {
            StartCoroutine(LerpToPosition());
            //for (int i = 0; i < gameObjectID.Length; i++)
            //{
            //    templePiece.TryGetValue(gameObjectID[i], out Tuple<Vector3, Quaternion> value);
            //    gameObjectID[i].transform.position = Vector3.SmoothDamp(gameObjectID[i].transform.position, value.Item1, ref vectorTry, 4);

            //    //if (Quaternion.Angle(gameObjectID[i].transform.rotation, value.Item2) <= -0.5f || Quaternion.Angle(gameObjectID[i].transform.rotation, value.Item2) >= 0.5)
            //    //    gameObjectID[i].transform.rotation = Quaternion.Slerp(gameObjectID[i].transform.rotation, value.Item2, Time.deltaTime);
            //    //float anglex = Mathf.SmoothDampAngle(gameObjectID[i].transform.eulerAngles.x, value.Item2.eulerAngles.x, ref floatTry, 4);
            //    //float angley = Mathf.SmoothDampAngle(gameObjectID[i].transform.eulerAngles.y, value.Item2.eulerAngles.y, ref floatTry, 4);
            //    //float anglez = Mathf.SmoothDampAngle(gameObjectID[i].transform.eulerAngles.z, value.Item2.eulerAngles.z, ref floatTry, 4);
            //    //gameObjectID[i].transform.rotation = Quaternion.Euler(anglex, angley, anglez);
            //}
            isCrystal = true;
            isCrystalActive=false;
        }
        else if (!isCrystal)
        {
            for (int i = 0; i < gameObjectID.Length; i++)
            {
                //gameObjectID[i].transform.position = UnityEngine.Random.insideUnitSphere * 20 + PointToRotate.position;
                //gameObjectID[i].transform.rotation = UnityEngine.Random.rotation;
                //gameObjectID[i].transform.RotateAround(PointToRotate.position, Vector3.one, randomrotation[i] * Time.deltaTime);
            }
        }
    }

    private IEnumerator LerpToPosition()
    {
        float fixCount = 0;
        while (fixCount < 1)
        {
            fixCount += Time.deltaTime;
            for (int i = 0; i < gameObjectID.Length; i++)
            {
                templePiece.TryGetValue(gameObjectID[i], out Tuple<Vector3, Quaternion> value);
                gameObjectID[i].transform.position = Vector3.SmoothDamp(gameObjectID[i].transform.position, value.Item1, ref vectorTry, 4);
                print(value.Item1);
                print(gameObjectID[i].name);
                print(gameObjectID.Length);
                //gameObjectID[i].transform.position = Vector3.Lerp(gameObjectID[i].transform.position, value.Item1, fixCount);
                //gameObjectID[i].transform.rotation = Quaternion.Slerp(gameObjectID[i].transform.rotation, value.Item2, fixCount);
            }
            yield return null;
        }
    }
}
