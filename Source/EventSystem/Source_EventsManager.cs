using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source_EventsManager : MonoBehaviour
{
    public List<Object_Event> events;//Все события на сцене.
    public bool awaitingStartEvent = true;//Ожидаем первый ивент на сцене.

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);//Не уничтожать между сценами.
    }

    private void Start()
    {
        events = new List<Object_Event>();//Создаем список.
    }

    private void Update()
    {
        InitializeForScene();
        if(awaitingStartEvent)//Если ожидаем стартовый ивент.
        {
            EngageEvent(0);//Запускаем стартовый ивент.
        }
    }

    public void InitializeForScene()
    {
        events = new List<Object_Event>();//Новый список событий.
        GameObject[] eventObjects = GameObject.FindGameObjectsWithTag("Object_Event");//Получаем список всех ивентов.
        foreach (GameObject eventObject in eventObjects)//Для каждого объекта.
        {
            AddEvent(eventObject.GetComponent<Object_Event>());//Пытаемся добавить ивент.
        }
    }

    public bool AddEvent(Object_Event newEvent)//Добавление события к списку.
    {
        if(!events.Contains(newEvent))//Если события нет в списке
        {
            events.Add(newEvent);//Добавляем событие в список.
            return true;
        }
        return false;
    }

    public bool EngageEvent(int eventId)//Выполнить событие.
    {
        foreach (Object_Event eventt in events)//Для каждого события в списке событий.
        {
            if (eventt != null)
            {
                if (eventt.uniqueId == eventId)//Если это нужный нам ивент.
                {
                    eventt.Initialize();//Выполнить событие.
                    return true;
                }
            }
            else
            {
                events.Remove(eventt);//Удаляем объект.
                return false;
            }
        }
        return false;
    }
    public bool EngageEventObjects(Object_Event[] envetsObjs)
    {
        foreach(Object_Event eventI in envetsObjs)
        {
            if(eventI != null)
            {
                eventI.Initialize();
            }
        }
        return true;
    }

    public bool CheckForEvents(int[] eventsId)
    {
        int checkedCounter = 0;
        foreach (int eventId in eventsId)
        {
            foreach (Object_Event eventt in events)//Для каждого события в списке событий.
            {
                if(eventt.uniqueId == eventId)
                {
                    checkedCounter += 1;
                }
            }
        }
        return checkedCounter == eventsId.Length;
    }

    public bool EngageEvent(int[] eventIds)//Выполнить несколько событий.
    {
        bool everyThingGood = true;
        foreach(int eventId in eventIds)
        {
            if(!EngageEvent(eventId))
            {
                everyThingGood = false;
            }
        }
        return everyThingGood;
    }
}
