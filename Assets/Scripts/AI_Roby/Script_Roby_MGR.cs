using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_Roby_MGR : MonoBehaviour
{
    public Animator Roby_animator { get; private set; }
    public GameObject Mai_Player { get; private set; }
    public NavMeshAgent Roby_agent { get; private set; }

    public float Mai_PlayerDistanceZone { get; private set; }
    public float Mai_PlayerNearZone { get; private set; }
    public float Mai_PlayerNormalZone { get; private set; }
    
    private void Awake()
    {
        Mai_Player = GameObject.FindGameObjectWithTag("Player");
        Roby_animator = GetComponent<Animator>();
        Roby_agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        Mai_PlayerNormalZone = 10;
        Mai_PlayerNearZone = 7;
        Mai_PlayerDistanceZone = 4;
    }

    void Update()
    {

    }
    
}
