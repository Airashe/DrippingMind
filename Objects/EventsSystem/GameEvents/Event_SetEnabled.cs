using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_SetEnabled : Object_Event
{
    public GameObject[] objects;
    public bool newValue;
    public Object_Event[] eventsObjs;
    public int[] events;

    public override void Initialize()
    {
        base.Initialize();
        if (objects.Length > 0)
        {
            for(int i = 0; i < objects.Length; i++)
            {
                if(objects[i] != null)
                {
                    objects[i].SetActive(newValue);
                    if (newValue)
                    {
                        if (objects[i].tag == "Object_Trigger")//Если это триггер.
                        {
                            GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>().AddTriggerData(objects[i]);
                        }
                    }
                    else
                    {
                        if (objects[i].tag == "Object_Trigger")//Если это триггер.
                        {
                            GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>().RemoveTriggerData(objects[i]);
                        }
                    }
                }
            }
        }
        if(events.Length > 0)
        {
            GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(events);
        }
        if(eventsObjs.Length > 0)
        {
            GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(eventsObjs);
        }
    }
}
