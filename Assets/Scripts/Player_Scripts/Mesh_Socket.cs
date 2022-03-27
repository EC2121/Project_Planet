using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_Socket : MonoBehaviour
{
    public Handle_Mesh_Sockets.SocketId socketId;
    private Transform attachPoint;

    void Start()
    {
        attachPoint = transform.GetChild(0);
    }
    public void Attach(Transform objecTransform)
    {
        objecTransform.SetParent(attachPoint,false);
    }
}
