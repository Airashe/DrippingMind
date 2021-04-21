using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ShowToolTip_ForInventory : Object_Event
{
    public float liveTime;

    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<Object_Player>().ShowIngameToolTip("Press " +
            Source_Constants.userInputs["INVENTORY_CHANGESTATE"] + " " +
             " to open inventory.", liveTime);
    }
}
