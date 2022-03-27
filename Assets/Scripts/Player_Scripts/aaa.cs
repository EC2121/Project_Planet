using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class aaa : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction ciao;
    private Player_Controller aspetta;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        ciao = playerInput.actions["Anto"];
    }

    // Update is called once per frame
    void Update()
    {
        
        if ( ciao.triggered)
        {
            Debug.Log("ciao");
        };
    }
}
