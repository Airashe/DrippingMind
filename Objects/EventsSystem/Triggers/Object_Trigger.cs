using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Trigger : MonoBehaviour
{
    public int unitUniqueId;//Объект юнита.
    public int[] onEnterEvents;//ID событий, которые должны быть активированы после входа.
    public int[] onExitEvents;//ID событий, которые должны быть активированы после выхода.
    public int[] onStayEvents;//
    public int[] onUseEvents;//ID события вызываемые при нажатии клавиши.
    public Object_Event[] onEnterEventsObj;
    public Object_Event[] onExitEventsObj;
    public Object_Event[] onStayEventsObj;
    public Object_Event[] onUseEventsObj;
    public int useTime = 9999;//Сколько раз можно использовать триггер.
    public string triggerTooltip;//Подсказка триггера.

    private void OnTriggerStay(Collider other)
    {
        if (onStayEvents.Length > 0 || onStayEventsObj.Length > 0)//Если есть события для выполнения.
        {
            if (other.gameObject.tag == "Object_Unit")//Если вошел юнит.
            {
                Object_Unit unit = other.gameObject.GetComponent<Object_Unit>();//Получаем ссылку на данные юнита.
                if (unitUniqueId == 0)//Если нет конкретного юнита на проверку.
                {
                    Object_Player playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Получаем ссылку на данные игрока.
                    if (playerData.unit == unit)//Если этот юнит под управлением игрока.
                    {
                        Source_EventsManager eventsManager = GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>();//Получаем ссылку на менеджер событий.
                        eventsManager.EngageEvent(onStayEvents);//Выполняем вызов всех ивентов.
                        eventsManager.EngageEventObjects(onStayEventsObj);
                    }
                }
                else
                {
                    if (unit.uniqueID == unitUniqueId)//Если нужный юнит.
                    {
                        Source_EventsManager eventsManager = GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>();//Получаем ссылку на менеджер событий.
                        eventsManager.EngageEvent(onStayEvents);//Выполняем вызов всех ивентов.
                        eventsManager.EngageEventObjects(onStayEventsObj);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + " unit entered.");
        if (onEnterEvents.Length > 0 || onEnterEventsObj.Length > 0)//Если есть события для выполнения.
        {
            if (other.gameObject.tag == "Object_Unit")//Если вошел юнит.
            {
                Object_Unit unit = other.gameObject.GetComponent<Object_Unit>();//Получаем ссылку на данные юнита.
                if(unitUniqueId == 0)//Если нет конкретного юнита на проверку.
                {
                    Debug.Log("Nigga");
                    Object_Player playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Получаем ссылку на данные игрока.
                    if (playerData.unit == unit)//Если этот юнит под управлением игрока.
                    {
                        Source_EventsManager eventsManager = GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>();//Получаем ссылку на менеджер событий.
                        eventsManager.EngageEvent(onEnterEvents);//Выполняем вызов всех ивентов.
                        Debug.Log("Test1");
                        eventsManager.EngageEventObjects(onEnterEventsObj);
                    }
                }
                else
                {
                    Debug.Log("Nigga2");
                    if(unit.uniqueID == unitUniqueId)//Если нужный юнит.
                    {
                        Source_EventsManager eventsManager = GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>();//Получаем ссылку на менеджер событий.
                        eventsManager.EngageEvent(onEnterEvents);//Выполняем вызов всех ивентов.
                        eventsManager.EngageEventObjects(onEnterEventsObj);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (onExitEvents.Length > 0 || onExitEventsObj.Length > 0)//Если есть события для выполнения.
        {
            if (other.gameObject.tag == "Object_Unit")//Если вошел юнит.
            {
                Object_Unit unit = other.gameObject.GetComponent<Object_Unit>();//Получаем ссылку на данные юнита.
                if (unitUniqueId == 0)//Если нет конкретного юнита на проверку.
                {
                    Object_Player playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Получаем ссылку на данные игрока.
                    if (playerData.unit == unit)//Если этот юнит под управлением игрока.
                    {
                        Source_EventsManager eventsManager = GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>();//Получаем ссылку на менеджер событий.
                        eventsManager.EngageEvent(onExitEvents);//Выполняем вызов всех ивентов.
                        eventsManager.EngageEventObjects(onExitEventsObj);
                    }
                }
                else
                {
                    if (unit.uniqueID == unitUniqueId)//Если нужный юнит.
                    {
                        Source_EventsManager eventsManager = GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>();//Получаем ссылку на менеджер событий.
                        eventsManager.EngageEvent(onExitEvents);//Выполняем вызов всех ивентов.
                        eventsManager.EngageEventObjects(onExitEventsObj);
                    }
                }
            }
        }
    }

    public void OnPlayerUsed()
    {
        Debug.Log("Called");
        if(useTime > 0 || useTime == -2)//Если триггер можно использовать либо он бесконечен.
        {
            Source_EventsManager eventsManager = GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>();//Получаем ссылку на менеджер событий.
            eventsManager.EngageEvent(onUseEvents);//Выполняем вызов всех ивентов.
            eventsManager.EngageEventObjects(onUseEventsObj);
            if (useTime != -2)
            {
                useTime -= 1;//Отнимает одно использование.
            }
        }
    }
}
