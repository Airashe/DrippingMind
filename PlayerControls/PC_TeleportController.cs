using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_TeleportController : MonoBehaviour
{
    //Ссылки на объекты.
    private Object_Player playerData;//Данные игрока.
    private PC_MainUseLogic mainUseLogic;//Скрипт который и вызывает взаимодействие.
    private Source_GameManager gameManager;//Гейм менеджер.

    private Object_Teleport activeTeleportation;//Активная телепортация.
    private float delayTeleport = 0;

    private void Start()
    {
        playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Ссылка на данные игрока.
        mainUseLogic = Camera.main.GetComponent<PC_MainUseLogic>();//Ссылка на использователь объектов.
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Ссылка на контроллер игры.
    }

    private void Update()
    {
        if(activeTeleportation == null)//Если нет активной телепортации.
        {
            if (playerData != null)//Если есть контроллер игрока.
            {
                if (playerData.unit != null)//Если у игрока есть подконтрольный юнит.
                {
                    foreach (Object_Teleport teleport in gameManager.teleports)//Для каждого телепорта.
                    {
                        if (Vector3.Distance(playerData.unit.position, teleport.transform.position) <= Source_Constants.CONST_GAMEPLAY_PLAYER_USE_TELEPORT_DISTANCE)
                        {
                            if (teleport.exit != null)//Если есть исходная точка.
                            {
                                mainUseLogic.AddUseItem(teleport.gameObject, UseData_ObjType.Teleport);//Добавляем телепорт в список использования.
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (Camera.main.GetComponent<PI_IngameGUI>().fadeAlpha == 1)//Если затеменение произошло.
            {
                if (delayTeleport > 0)
                {
                    delayTeleport -= Time.deltaTime;
                }
                else
                {
                    playerData.unit.transform.position = activeTeleportation.exit.transform.position;//Телепортируем юнита.
                    Camera.main.GetComponent<PI_IngameGUI>().fadeState = false;//Проясняем экран.
                    activeTeleportation = null;
                }
            }
        }
    }

    public void OnUseInput(Object_Teleport teleport)
    {
        Camera.main.GetComponent<PI_IngameGUI>().fadeState = true;//Затемняем экран.
        activeTeleportation = teleport;
        delayTeleport = 0.3f;
    }
}
