using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : GenericWindow
{
    public float closeDelay = 7f;

    private float delay;
    private bool closing;

    private Text textInstance;

    public string text
    {
        set
        {
            textInstance.text = value;
        }
    }

    protected override void Awake()
    {
        textInstance = GetComponentInChildren<Text>();

        base.Awake();
    }

    public override void Open()
    {
        //do this cuz if we open window more then once it needs to close corectly
        base.Open();
        closing = true;
        delay = 0;
    }

    private void Update()
    {
        if(closing)
        {
            delay += Time.deltaTime;
            if(delay >= closeDelay)
            {
                Close();
                closing = false;
            }
        }
    }

}
