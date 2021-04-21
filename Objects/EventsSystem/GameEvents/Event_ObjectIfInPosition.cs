using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ObjectIfInPosition : Object_Event
{
    [Header("Event ingame data")]
    public GameObject ingameObject;//Объект, позиция которого проверяется.

    public int[] trueEventsIds;//Id событий, если проверка верна.
    public Object_Event[] trueEventsObjs;//Объекты событий, если проверка верна.
    public int[] falseEventsIds;//Id событий, если проверка не верна.
    public Object_Event[] falseEventsObjs;//Объекты событий, если проверка не верна.

    public bool callOnUpdate = false;//Вызывать события ложности в Update;
    private bool onUpdateCalled = false;//Были ли вызваны события ложности в Update;
    private bool initializing = false;//Находиться ли событие в процессе инициализации.

    private bool ObjectInside//Проверка находиться ли объект внутри.
    {
        get
        {
            if (ingameObject != null)
            {
                if (ingameObject.transform.position.x >= (transform.position.x - (transform.localScale.x / 2)) && ingameObject.transform.position.x <= (transform.position.x + (transform.localScale.x / 2)))//Если игрок попадает под скейлы по х.
                {
                    if (ingameObject.transform.position.z >= (transform.position.z - (transform.localScale.z / 2)) && ingameObject.transform.position.z <= (transform.position.z + (transform.localScale.z / 2)))//Если игрок попадает под скейлы z.
                    {
                        if (ingameObject.transform.position.y >= (transform.position.y - (transform.localScale.y / 2)) && ingameObject.transform.position.y <= (transform.position.y + (transform.localScale.y / 2)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    [Header("Editor settings")]
    public Color eventColor;//Цвет события, в редакторе.

    public override void Initialize()
    {
        base.Initialize();
        if (ObjectInside)//Если объект находиться внутри на момент инициализации триггера.
        {
            if (callOnUpdate)//Если нужно вызывать в обновлении.
            {
                onUpdateCalled = false;//Устанавливаем, что события в Update не были вызваны.
            }
            if(trueEventsIds.Length > 0)//Если есть верные события, которые нужно вызывать.
            {
                GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(trueEventsIds);//Вызываем верные события по id;
            }
            if (trueEventsObjs.Length > 0)//Если есть верные события, которые нужно вызывать.
            {
                GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(trueEventsObjs);//Вызываем верные события по объектам;
            }
        }
        else//Если объект не в зоне события.
        {
            if(callOnUpdate)//Если нужно вызывать в обновлении.
            {
                onUpdateCalled = true;//Помечаем, что событие уже вызыванно в Update, потому что мы выполним его здесь.
                if (falseEventsIds.Length > 0)//Если есть ложные события, которые нужно вызывать.
                {
                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(falseEventsIds);//Вызываем ложные события по id;
                }
                if (falseEventsObjs.Length > 0)//Если есть ложные события, которые нужно вызывать.
                {
                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(falseEventsObjs);//Вызываем верные ложные по объектам;
                }
            }
        }
    }

    public void Update()
    {
        if(!ObjectInside && callOnUpdate && !onUpdateCalled)//Если не в процессе реализации и нужно вызывать ложные события и события не были взываны.
        {
            if (falseEventsIds.Length > 0)//Если есть ложные события, которые нужно вызывать.
            {
                GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(falseEventsIds);//Вызываем ложные события по id;
            }
            if (falseEventsObjs.Length > 0)//Если есть ложные события, которые нужно вызывать.
            {
                GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(falseEventsObjs);//Вызываем верные ложные по объектам;
            }
            onUpdateCalled = true;//Событие в Update было вызывано.
        }
    }

    private void OnDrawGizmos()//При рисовании гизмо.
    {
        Gizmos.color = eventColor;//Устанавливаем цвет для кубика.
        Gizmos.DrawWireCube(transform.position, transform.localScale);//Рисуем территорию, которую охватывает триггер.
        Gizmos.color = Color.white;//Возвращаем стандартный цвет гизмо.
    }
}
