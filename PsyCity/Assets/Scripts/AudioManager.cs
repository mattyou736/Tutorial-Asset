using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource BGM;
    public AudioClip overworld, battle;
    float targetVolume = 0.2f;
    float duration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ChangeBGM(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBGM(bool inBattle)
    {
        BGM.Stop();
        if (inBattle)
        {
            BGM.clip = battle;
            BGM.Play();
        }
        else
        {
            BGM.clip = overworld;
            BGM.Play();
        }
        
    }

    
}
