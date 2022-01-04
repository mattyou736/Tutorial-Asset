﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericWindow : MonoBehaviour
{
    public static WindowManager manager;
    public Windows nextWindow;
    public Windows previousWindow;
    public GameObject firstSelected;

    protected EventSystem eventSystem
    {
        get
        {
            return GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
    }

    public virtual void OnFocus()
    {
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    protected virtual void Display(bool value)
    {
        gameObject.SetActive(value);
    }

    public virtual void Open()
    {
        Display(true);
        OnFocus();
    }

    public virtual void Close()
    {
        Display(false);
    }

    // Use this for initialization
    protected virtual void Awake ()
    {
        Close();
	}
    //open enum for next window
    public void OnNextWindow()
    {
        manager.Open((int)nextWindow - 1);
    }

    public void OnPreviousWindow()
    {
        manager.Open((int)previousWindow - 1);
    }
}
