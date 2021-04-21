using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_SetBackgroundMusic : Object_Event
{
    public AudioClip clip;
    public float volume;
    public bool loop;
    public bool stop;

    public override void Initialize()
    {
        base.Initialize();
        if(stop)
        {
            Camera.main.GetComponent<PC_SoundsController>().StopBackgroundMusic();
        }
        else
        {
            Camera.main.GetComponent<PC_SoundsController>().PlayBackgroundMusic(clip, volume, loop);
        }
    }
}
