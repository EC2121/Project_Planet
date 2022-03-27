using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PLAYERUITEST : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider HPSlider;

    public float maxHp;
    private float hp;
    void Start()
    {
        HPSlider.maxValue = maxHp;
        HPSlider.value = maxHp;
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            OnDamageTaken(2);
        }
    }

    public void OnDamageTaken(float damage)
    {
        hp -= damage;
        HPSlider.value = hp;
    }
    

}
