using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ChangeWhiteState : Object_Event
{
    public bool newState = false;
    public Object_Event[] eventsObjs;
    public int[] events;
    public override void Initialize()
    {
        base.Initialize();
        if(eventsObjs.Length > 0)
        {
            GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(eventsObjs);
        }
        if (events.Length > 0)
        {
            GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(events);
        }
        Camera.main.GetComponent<PI_IngameGUI>().whiteState = newState;
    }
}
