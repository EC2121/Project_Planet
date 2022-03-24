using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class OnClick : MonoBehaviour
{
    private Button myButton;
    private Image myImage;
    private Sprite highlighteSprite;
    private void Awake()
    {
        myButton = GetComponent<Button>();
        myImage = GetComponent<Image>();
        
        
    }

    private void Start()
    {
        SpriteAtlas atlas;
        
        atlas = Resources.Load<SpriteAtlas>("Sprites/UI/Menu/Start_Menu.png");
            highlighteSprite = atlas.GetSprite("StartMenu_ButtonHighlighted");
    }

    public void OnClicked()
    {
        Debug.Log("Button clicked!");

        myImage.sprite = highlighteSprite;

    }
    
    
}
