using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class TabButton_Custom : MonoBehaviour,
    ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public TabGroup TabGroup;
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;
    void Start()
    {
        TabGroup.Subscribe(this);
    }



    public void Select()
    {
        if (onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }
    public void Deselect()
    {
        if (onTabDeselected != null)
        {
            onTabDeselected.Invoke();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        TabGroup.OnTabSelected(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        TabGroup.OnTabExit(this);
    }

    public void OnSubmit(BaseEventData eventData) { }

}
