using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ChangeMainUnit : Object_Event
{
    public Object_Unit unit;

    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<Object_Player>().unit = unit;
    }
}
