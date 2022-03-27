using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle_Mesh_Sockets : MonoBehaviour
{
    public enum SocketId
    {
        Spine,
        RightHand,
        Free
    }

    private Dictionary<SocketId, Mesh_Socket> socketMap = new Dictionary<SocketId, Mesh_Socket>();

    void Start()
    {
        Mesh_Socket[] sockets = GetComponentsInChildren<Mesh_Socket>();

        foreach (var socket in sockets)
        {
            socketMap[socket.socketId] = socket;
        }
    }

    public void Attach(Transform objectTransform, SocketId socketId)
    {
        socketMap[socketId].Attach(objectTransform);
    }
  
}
