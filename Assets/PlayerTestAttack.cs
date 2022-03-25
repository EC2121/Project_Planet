using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTestAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Enemy.OnDamageTaken.Invoke(10, this.gameObject, false);
        }
        else if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Enemy.OnDamageTaken.Invoke(10, this.gameObject, true);
        }
    }
}
