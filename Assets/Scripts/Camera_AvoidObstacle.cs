using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_AvoidObstacle : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    private RaycastHit hit;
    private Vector3 cameraOffset;
    void Start()
    {
        cameraOffset = _cameraTransform.localPosition;

    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.position + cameraOffset);

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {

            _cameraTransform.localPosition = new Vector3(0, 0, -Vector3.Distance(transform.position, hit.point));
        }
        else
        {
            _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, cameraOffset, Time.deltaTime);
        }
        Debug.DrawLine(ray.origin, ray.direction, Color.yellow);
    }
}
