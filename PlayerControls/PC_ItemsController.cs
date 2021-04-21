using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_ItemsController : MonoBehaviour
{
    //Ссылки на объекты.
    private Object_Player playerData;//Данные игрока.
    private PC_MainUseLogic mainUseLogic;//Скрипт который и вызывает взаимодействие.

    private void Start()
    {
        playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Ссылка на данные игрока.
        mainUseLogic = Camera.main.GetComponent<PC_MainUseLogic>();//Ссылка на использователь объектов.
    }

    private void Update()
    {
        if (playerData != null)//Если есть контроллер игрока.
        {
            if (playerData.unit != null)//Если у игрока есть подконтрольный юнит.
            {
                GameObject[] sceneItems = GameObject.FindGameObjectsWithTag("Object_Item");//Получаем список всех предметов на сцене.
                foreach (GameObject item in sceneItems)//Для каждого предмета.
                {
                    if (Vector3.Distance(playerData.unit.transform.position, item.transform.position) < Source_Constants.CONST_GAMEPLAY_PLAYER_PICKUP_ITEM_DISTANCE)//Если игрок дотягивается до предмета.
                    {
                        mainUseLogic.AddUseItem(item.gameObject, UseData_ObjType.Item);//Добавляем предмет в список использования.
                    }
                }
            }
        }
    }

    public void OnUseInput(Object_Item item)
    {
        if (playerData.unit.AddItem(item.GetComponent<Object_Item>().Data)//Добавляем предмет в инвентарь.
                                )
        {
            if (item.GetComponent<Object_Item>().pickUpEvents.Length > 0)
            {
                GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(item.GetComponent<Object_Item>().pickUpEvents);
            }
            if(item.GetComponent<Object_Item>().pickUpEventsObjs.Length > 0)
            {
                GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(item.GetComponent<Object_Item>().pickUpEventsObjs);
            }
            Destroy(item.gameObject);//Удаляем объект со сцены.
        }
    }
}
