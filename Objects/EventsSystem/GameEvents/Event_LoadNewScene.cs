using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_LoadNewScene : Object_Event
{
    public int nextSceneId = 0;
    public bool initialized;
    public override void Initialize()
    {
        base.Initialize();
        if(!initialized)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>().GoToNextScene(nextSceneId);
            initialized = true;
        }
    }
}
