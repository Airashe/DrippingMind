using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_MoveTransform : Object_Event
{
    private bool active = false;
    public Transform transformM;
    public float speed;

    private void Update()
    {
        if(active)
        {
            transformM.Translate(Vector3.down*Time.deltaTime* speed);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        active = true;
    }
}
