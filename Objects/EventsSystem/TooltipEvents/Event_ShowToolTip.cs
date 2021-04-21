using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ShowToolTip : Object_Event
{
    public string text;
    public float liveTime;

    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<Object_Player>().ShowIngameToolTip(text, liveTime);

    }
}
