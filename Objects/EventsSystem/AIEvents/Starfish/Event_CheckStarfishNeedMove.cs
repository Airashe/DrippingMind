using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_CheckStarfishNeedMove : Object_Event
{
    public Object_Unit unit;
    public float start;
    public float end;

    public int[] eventsIfTrue;

    public override void Initialize()
    {
        Debug.Log("Testing event start");
        
        if(unit != null)
        {
            Debug.Log(unit.position.y);
            if(unit.position.y < start || unit.position.y > end)
            {
                GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(eventsIfTrue);
            }
        }
    }
}
