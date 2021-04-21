using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_PlaySound : Object_Event
{
    public Object_SoundMaker soundObject;//
    public int smUniqueId;//Уникальный id

    public override void Initialize()
    {
        base.Initialize();
        if(soundObject != null)
        {
            soundObject.Play();//Проигрываем звук.
        }
        else
        {
            foreach(Object_SoundMaker sm in GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>().soundMakers)
            {
                if(sm.uniqueId == smUniqueId)
                {
                    sm.Play();
                    break;
                }
            }
        }
    }
}
