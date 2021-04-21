using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ChangeFadeState : Object_Event
{
    public bool newState = false;
    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<PI_IngameGUI>().fadeState = newState;
    }
}
