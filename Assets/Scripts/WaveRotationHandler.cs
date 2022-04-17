using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveRotationHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Rotation")]
    public bool Rotate;
    public float RotationSpeed;
    [Header("UpAndDownMovement")]
    public bool MoveUpAndDown;
    private float StartHeight;
    public float UpAndDownMaxHeight;
    public float UpAndDownSpeed;
    public bool RandomPosition;

    void Start()
    {
        if (RandomPosition)
        {
            UpAndDownMaxHeight = Random.Range(0.4f, UpAndDownMaxHeight);
            UpAndDownSpeed = Random.Range(0.2f, UpAndDownSpeed);
        }
        StartHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveUpAndDown)
        {
            Vector3 pos = transform.position;
            float newPosY = Mathf.Sin(Time.time * UpAndDownSpeed);
            transform.position = new Vector3(pos.x, StartHeight + ( newPosY * UpAndDownMaxHeight ), pos.z);
        }
        if (Rotate)
        {
            transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
        }
    }
}
