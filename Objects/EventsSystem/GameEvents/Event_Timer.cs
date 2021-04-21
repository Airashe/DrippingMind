using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Timer : Object_Event
{
    public float time = 4;//Таймер.
    public bool active = false;
    public int[] eventsAfter;
    public Object_Event[] eventsAfterObj;
    private float timer;

    public override void Initialize()
    {
        base.Initialize();
        active = true;
        timer = time;
    }
    private void Update()
    {
        if(active)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if(eventsAfter.Length > 0)
                {
                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(eventsAfter);
                }
                if (eventsAfterObj.Length > 0)
                {
                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(eventsAfterObj);
                }
                active = false;
            }
        }
    }
}
