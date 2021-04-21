using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ShowBuild : Object_Event
{
    public float liveTime;

    public override void Initialize()
    {
        base.Initialize();
        Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        System.DateTime buildDate = new DateTime(2000, 1, 1)
                                .AddDays(version.Build).AddSeconds(version.Revision * 2);
        string displayableVersion = version.ToString() + "(" + buildDate.ToString() + ")";
        Camera.main.GetComponent<Object_Player>().ShowIngameToolTip(displayableVersion, liveTime);

    }
}
