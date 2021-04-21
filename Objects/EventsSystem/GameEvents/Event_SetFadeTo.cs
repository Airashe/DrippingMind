using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_SetFadeTo : Object_Event
{
    public float newValue;
    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<PI_IngameGUI>().fadeAlpha = newValue;
        Camera.main.GetComponent<PI_IngameGUI>().fadeState = newValue == 1;
    }
}
