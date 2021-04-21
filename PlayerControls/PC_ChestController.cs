using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_ChestController : MonoBehaviour
{
    //Ссылки на объекты.
    private Object_Player playerData;//Данные игрока.
    private Source_GameManager gameManager;//Гейм менеджер.
    private PC_MainUseLogic mainUseLogic;//Скрипт который и вызывает взаимодействие.

    public Object_Chest activeChest;//Сундук с которым ведеться взаимодействие.

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
                if (activeChest == null)//Если мы не взаимодействуем с сундуком.
                {
                    foreach (Object_Chest chest in gameManager.chests)//Для каждого сундука.
                    {
                        if (Vector3.Distance(playerData.unit.transform.position, chest.transform.position) <= Source_Constants.CONST_GAMEPLAY_PLAYER_USE_CHEST_DISTANCE)//Если игрок дотягивается до сундука.
                        {
                            if(chest != activeChest)
                            {
                                mainUseLogic.AddUseItem(chest.gameObject, UseData_ObjType.Chest);//Добавление сундука в список использования.
                            }
                        }
                    }
                }
                else//Если взаимодействуем с сундуком.
                {
                    if (Vector3.Distance(playerData.unit.transform.position, activeChest.transform.position) > Source_Constants.CONST_GAMEPLAY_PLAYER_USE_CHEST_DISTANCE)//Если игрок отошел слишком далеко.
                    {
                        activeChest = null;//Обнуляем используемый сундук.
                    }
                }
            }
        }
    }

    public void OnUseInput(Object_Chest chest)
    {
        if(activeChest != chest)
        {
            if (chest.open || chest.requestItem == null)//Если сундук открыт или открыт по умолчанию.
            {
                activeChest = chest;//Запоминаем открываемый сундук.
            }
            else//Если сундук закрыт.
            {
                Vector2Int requestItemPos = playerData.unit.HaveItem(chest.requestItem);//Позиция запрашиваемого предмета в инвентаре юнита.
                if (requestItemPos != new Vector2Int(-1, -1))//Если у юнита есть нужный предмет.
                {
                    chest.open = true;//Открываем сундук.
                    activeChest = chest;//Запоминаем открываемый сундук.
                    playerData.unit.RemoveItem(requestItemPos.x, requestItemPos.y);//Удаляем требуемый предмет.
                }
                else
                {
                    playerData.unit.Say(chest.closedQuote);
                    Camera.main.GetComponent<PI_IngameGUI>().AddSayTrackForUnit(playerData.unit);
                }
            }
        }
        else
        {
            activeChest = null;//Обнуляем используемый сундук.
        }
    }
}
