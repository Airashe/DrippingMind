using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_StartScene : Object_Event
{
    public GameStage sceneStage = GameStage.Game;
    public bool used = false;
    public int[] startEvents;
    public Object_Event[] startEventsObj;
    public float time = 5;

    private void Start()
    {
        used = false;
    }

    public override void Initialize()
    {
        if (!used)
        {
            if(time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                base.Initialize();
                GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().awaitingStartEvent = false;//Отключаем ожидание стартового ивента.
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>().stage = sceneStage;
                GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(startEvents);//Запускаем первые ивенты сцены.
                GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(startEventsObj);//Запускаем первые ивенты сцены.
                used = true;
            }
        }
    }
}
