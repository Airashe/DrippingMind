using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_TriggerUseController : MonoBehaviour
{
    //Ссылки на объекты.
    private Object_Player playerData;//Данные игрока.
    private Source_GameManager gameManager;//Гейм менеджер.
    private PC_MainUseLogic mainUseLogic;//Скрипт который и вызывает взаимодействие.

    private void Start()
    {
        playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Ссылка на данные игрока.
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Получаем ссылку на гейм манагер.
        mainUseLogic = Camera.main.GetComponent<PC_MainUseLogic>();//Ссылка на использователь объектов.
    }

    private void Update()
    {
        if (playerData != null)//Если есть контроллер игрока.
        {
            if (playerData.unit != null)//Если у игрока есть подконтрольный юнит.
            {
                foreach (Object_Trigger trigger in gameManager.triggers)//Для каждого предмета.
                {
                    if(trigger.onUseEvents.Length > 0 || trigger.onUseEventsObj.Length > 0)//Если есть события, которые можно вызвать.
                    {
                        if (Vector3.Distance(playerData.unit.transform.position, trigger.transform.position) <= trigger.transform.localScale.z || Vector3.Distance(playerData.unit.transform.position, trigger.transform.position) <= trigger.transform.localScale.x)//Если игрок дотягивается до предмета.
                        {
                            if (trigger.useTime != 0)
                            {
                                mainUseLogic.AddUseItem(trigger.gameObject, UseData_ObjType.Trigger);//Добавляем триггер к списку использования.
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnUseInput(Object_Trigger trigger)
    {
        trigger.OnPlayerUsed();//Вызываем использование.
    }
}
