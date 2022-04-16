using UnityEngine;

public class Script_SinglePieceRandomMove : MonoBehaviour
{
    private Vector3 vectorTry = Vector3.zero;
    private Vector3 randomPosition;
    private Quaternion randomRotation;
    private Transform PointToRotate;

    private void Awake()
    {
        PointToRotate = GameObject.Find("CenterToRotate").transform;
    }

    private void Start()
    {
        randomPosition = Random.insideUnitSphere * 20 + PointToRotate.position;
        randomRotation = Random.rotation;
    }

    //private void Update()
    //{
    //    if (!Script_Repositioning.isCrystalActive)
    //    {
    //        if (Vector3.Distance(transform.position, randomPosition) > 0.2f)
    //        {
    //            transform.position = Vector3.SmoothDamp(transform.position, randomPosition, ref vectorTry, 4);
    //        }
    //        else randomPosition = Random.insideUnitSphere * 30 + PointToRotate.position;

    //        if (Quaternion.Angle(transform.rotation, randomRotation) > 1)
    //        {
    //            transform.rotation = Quaternion.Slerp(transform.rotation, randomRotation, Time.deltaTime);
    //        }
    //        else randomRotation = Random.rotation;
    //    }
    //}
}
