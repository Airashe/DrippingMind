using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_KillUnit : Object_Event
{
    public Object_Unit unit;

    public override void Initialize()
    {
        base.Initialize();
        if(unit != null)
        {
            unit.Kill();
        }
    }
}
