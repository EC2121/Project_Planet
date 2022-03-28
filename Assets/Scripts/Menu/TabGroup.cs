using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton_Custom> tabButtons;
    /*
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    */
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;
    public TabButton_Custom SelectedTab;

    public List<GameObject> Panels;

    public void Subscribe(TabButton_Custom button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton_Custom>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton_Custom button)
    {
        ResetTabs();
        if (SelectedTab == null || button != SelectedTab)
        {
            button.GetComponent<Image>().color = tabHover;    
        }
    }
    public void OnTabExit(TabButton_Custom button)
    {
        ResetTabs();
    }
    public void OnTabSelected(TabButton_Custom button)
    {
        if (SelectedTab != null)
        {
            SelectedTab.Deselect();
        }
        SelectedTab = button;
        
        SelectedTab.Select();
        
        ResetTabs();
        button.GetComponent<Image>().color = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < Panels.Count; i++)
        {
            if (i == index)
            {
                Panels[i].SetActive(true);       
            }
            else
            {
                Panels[i].SetActive(false); 
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton_Custom button in tabButtons)
        {
            
            if (EventSystem.current.currentSelectedGameObject == button && 
                SelectedTab != null && SelectedTab == button)
            {
                continue;
            }
            button.GetComponent<Image>().color = tabIdle;
        }
    }
    
}
