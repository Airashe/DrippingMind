using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_BlockDoor : Object_Event
{
    public Object_DoorConnecter doorConnecter;

    public override void Initialize()
    {
        base.Initialize();
        if (doorConnecter != null)
        {
            doorConnecter.totalyLocked = true;
        }
    }
}
