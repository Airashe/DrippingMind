using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_SetWorldBorders : Object_Event
{
    public Vector2 worldStart2D;
    public Vector2 worldEnd2D;

    public override void Initialize()
    {
        base.Initialize();
        GameObject.FindGameObjectWithTag("CameraManager").GetComponent<Source_CameraManager>().worldStart2D = worldStart2D;
        GameObject.FindGameObjectWithTag("CameraManager").GetComponent<Source_CameraManager>().worldEnd2D = worldEnd2D;
    }
}
