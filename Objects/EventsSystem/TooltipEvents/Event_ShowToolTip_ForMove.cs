using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ShowToolTip_ForMove : Object_Event
{
    public float liveTime;

    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<Object_Player>().ShowIngameToolTip("Press " +
            Source_Constants.userInputs["MOVEMENT_MOVEFORWARD"] + " " +
            Source_Constants.userInputs["MOVEMENT_MOVELEFT"] + " " +
            Source_Constants.userInputs["MOVEMENT_MOVEBACKWARD"] + " " +
            Source_Constants.userInputs["MOVEMENT_MOVERIGHT"] + " " +
             " to move.", liveTime);
    }
}
