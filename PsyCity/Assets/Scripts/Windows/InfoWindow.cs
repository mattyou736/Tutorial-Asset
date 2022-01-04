using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoWindow : MonoBehaviour
{
    public GameObject[] window;

    public void GeneralButton(int arrayIndicator)
    {
        for (int i = 0; i < window.Length; i++)
        {
            if(i == arrayIndicator)
            {
                window[i].SetActive(true);
            }
            else
            {
                window[i].SetActive(false);
            }
        }
    }

    public void BackButton()
    {
        this.gameObject.SetActive(false);
    }
}
