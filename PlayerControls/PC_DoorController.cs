using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_DoorController : MonoBehaviour
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
                foreach (Object_DoorConnecter doorConnecter in gameManager.doorConnecters)//Для каждой двери.
                {
                        if (Vector3.Distance(playerData.unit.position, doorConnecter.transform.position) < Source_Constants.CONST_GAMEPLAY_PLAYER_USE_DOORCONNECTOR_DISTANCE)//Если игрок дотягивается до двери.
                        {
                        if (!doorConnecter.totalyLocked)
                        {
                            mainUseLogic.AddUseItem(doorConnecter.gameObject, UseData_ObjType.Door);//Добавляем триггер к списку использования.
                        }
                        }
                }
            }
        }
    }

    public void OnUseInput(Object_DoorConnecter doorConnecter)
    {
        if (!doorConnecter.totalyLocked)
        {
            if (doorConnecter.needItemToOpen != null)
            {
                if (playerData.unit.HaveItem(doorConnecter.needItemToOpen) != new Vector2Int(-1, -1))
                {
                    doorConnecter.OnUse();
                }
            }
            else
            {
                doorConnecter.OnUse();
            }
        }
    }
}
