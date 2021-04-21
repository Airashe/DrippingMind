using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_MainUseLogic : MonoBehaviour
{
    //Ссылки на другие объекты.
    private Object_Player playerData;//Данные игрока.
    private PC_ChestController chestController;//Контроллер сундуков.
    private PC_DialogueController dialogueController;//Контроллер диалога.
    private PC_ItemsController itemsController;//Контроллер предметов.
    private PC_TriggerUseController triggerUseController;//Контроллер триггеров.
    private PC_TeleportController teleportsController;//Контроллер телепортов.
    private PC_DoorController doorController;//Контроллер дверей.

    public List<Property_UseData> useList;//Список объектов с которыми может взаимодействовать игрок.
    public int selectedItem = 0;//Выбарнный предмет взаимодействия.
    public string useKeyCodeName = "INVENTORY_PICKUP_ITEM";//Ключ для кнопки использования.

    public string ToolTip
    {
        get
        {
            if(selectedItem >= 0 && selectedItem < useList.Count)
            {
                switch(useList[selectedItem].type)
                {
                    case UseData_ObjType.Chest:
                        Object_Chest chest = useList[selectedItem].gameObject.GetComponent<Object_Chest>();//Ссылка на копмпонент.
                        return (chestController.activeChest != chest ? " to open " + chest.chestName + ".": " to close " + chest.chestName + ".");
                    case UseData_ObjType.Unit:
                        return "to talk with " + useList[selectedItem].gameObject.GetComponent<Object_Unit>().ingameName + ".";
                    case UseData_ObjType.Item:
                        if (useList[selectedItem].gameObject != null)
                        {
                            return "to pick up " + useList[selectedItem].gameObject.GetComponent<Object_Item>().itemData.name;
                        }
                        else
                        {
                            return "";
                        }
                    case UseData_ObjType.Trigger:
                        return useList[selectedItem].gameObject.GetComponent<Object_Trigger>().triggerTooltip;//Отображаем действие триггера.
                    case UseData_ObjType.Teleport:
                        return useList[selectedItem].gameObject.GetComponent<Object_Teleport>().commentary;//Отображаем действие телепорта.
                    case UseData_ObjType.Door:
                        return useList[selectedItem].gameObject.GetComponent<Object_DoorConnecter>().Open ? "to close the door." : "to open the door.";
                }
            }
            return "N/A";
        }
    }

    public string ToolTipForPosition(int position)
    {
        if (position >= 0 && position < useList.Count)
        {
            switch (useList[position].type)
            {
                case UseData_ObjType.Chest:
                    Object_Chest chest = useList[position].gameObject.GetComponent<Object_Chest>();//Ссылка на копмпонент.
                    return (chestController.activeChest != chest ? " to open " + chest.chestName + "." : " to close " + chest.chestName + ".");
                case UseData_ObjType.Unit:
                    return "to talk with " + useList[position].gameObject.GetComponent<Object_Unit>().ingameName + ".";
                case UseData_ObjType.Item:
                    if (useList[position].gameObject != null)
                    {
                        return "to pick up " + useList[position].gameObject.GetComponent<Object_Item>().itemData.name;
                    }
                    else
                    {
                        return "";
                    }
                case UseData_ObjType.Trigger:
                    return useList[position].gameObject.GetComponent<Object_Trigger>().triggerTooltip;//Отображаем действие триггера.
                case UseData_ObjType.Teleport:
                    return useList[position].gameObject.GetComponent<Object_Teleport>().commentary;//Отображаем действие телепорта.
                case UseData_ObjType.Door:
                    return useList[position].gameObject.GetComponent<Object_DoorConnecter>().Open ? "to close the door." : "to open the door.";
            }
        }
        return "N/A";
    }

    private void Start()
    {
        playerData = Camera.main.GetComponent<Object_Player>();//Получаем данные игрока.
        chestController = Camera.main.GetComponent<PC_ChestController>();//Контроллер сундуков.
        dialogueController = Camera.main.GetComponent<PC_DialogueController>();//Контроллер диалога.
        itemsController = Camera.main.GetComponent<PC_ItemsController>();//Контроллер предметов.
        triggerUseController = Camera.main.GetComponent<PC_TriggerUseController>();//Контроллер триггеров.
        teleportsController = Camera.main.GetComponent<PC_TeleportController>();//Контроллер телепортов.
        doorController = Camera.main.GetComponent<PC_DoorController>();//Контроллер дверей.
    }

    private void Update()
    {
        for(int i = 0; i < useList.Count; i++)//Для каждого объекта, до которого дотягиваемся.
        {
            bool canReach = false;//Можем ли мы дотянуться до объекта.
            switch(useList[i].type)//В зависимости от типа объекта.
            {
                case UseData_ObjType.Chest://Если сундук.
                    if (Vector3.Distance(playerData.unit.transform.position, useList[i].position) <= Source_Constants.CONST_GAMEPLAY_PLAYER_USE_CHEST_DISTANCE)//Если игрок дотягивается до сундука.
                    {
                        canReach = true;//Может дотянуться.
                    }
                    break;
                case UseData_ObjType.Trigger://Если триггер.
                    if (Vector3.Distance(playerData.unit.transform.position, useList[i].position) <= useList[i].gameObject.transform.localScale.z || Vector3.Distance(playerData.unit.transform.position, useList[i].gameObject.transform.position) <= useList[i].gameObject.transform.localScale.x)//Если игрок в зоне триггера.
                    {
                        if (useList[i].gameObject.GetComponent<Object_Trigger>().useTime != 0)
                        {
                            canReach = true;//Может дотянуться.
                        }
                    }
                    break;
                case UseData_ObjType.Unit://Если юнит.
                    if (Vector3.Distance(playerData.unit.transform.position, useList[i].position) < Source_Constants.CONST_GAMEPLAY_PLAYER_DIALOGUE_START_DISTANCE)//Если игрок дотягивается до юнита.
                    {
                        canReach = true;//Может дотянуться.
                    }
                    break;
                case UseData_ObjType.Item://Если предмет.
                    if (Vector3.Distance(playerData.unit.transform.position, useList[i].position) < Source_Constants.CONST_GAMEPLAY_PLAYER_PICKUP_ITEM_DISTANCE)//Если игрок дотягивается до предмета.
                    {
                        canReach = true;//Может дотянуться.
                    }
                    break;
                case UseData_ObjType.Teleport://Если портал.
                    if (Vector3.Distance(playerData.unit.transform.position, useList[i].position) < Source_Constants.CONST_GAMEPLAY_PLAYER_USE_TELEPORT_DISTANCE)//Если игрок дотягивается до портала.
                    {
                        canReach = true;//Может дотянуться.
                    }
                    break;
                case UseData_ObjType.Door://Если дверь.
                    if (Vector3.Distance(playerData.unit.transform.position, useList[i].position) < Source_Constants.CONST_GAMEPLAY_PLAYER_USE_DOORCONNECTOR_DISTANCE)//Если игрок дотягивается до портала.
                    {
                        if(!useList[i].gameObject.GetComponent<Object_DoorConnecter>().totalyLocked)
                        {
                            canReach = true;//Может дотянуться.
                        }
                    }
                    break;
            }
            if(!canReach)//Если игрок не дотягивается до предмета.
            {
                useList.RemoveAt(i);//Удаляем предмет из списка дотягиваемых.
            }
        }
        if (Input.GetKeyUp(Source_Constants.userInputs[useKeyCodeName]))//Если нажата кнопка использования.
        {
            if (selectedItem >= 0 && selectedItem < useList.Count)
            {
                switch (useList[selectedItem].type)//В зависимости от типа объекта.
                {
                    case UseData_ObjType.Chest:
                        chestController.OnUseInput(useList[selectedItem].gameObject.GetComponent<Object_Chest>());//Отправляем использование сундука в контроллер сундуков.
                        break;
                    case UseData_ObjType.Trigger:
                        triggerUseController.OnUseInput(useList[selectedItem].gameObject.GetComponent<Object_Trigger>());//Отправляем использование триггера в контроллер триггеров.
                        break;
                    case UseData_ObjType.Unit:
                        dialogueController.OnUseInput(useList[selectedItem].gameObject.GetComponent<Object_Unit>());//Отправляем начало диалога в контроллер диалогов.
                        break;
                    case UseData_ObjType.Item:
                        itemsController.OnUseInput(useList[selectedItem].gameObject.GetComponent<Object_Item>());//Отправляем начало диалога в контроллер диалогов.
                        break;
                    case UseData_ObjType.Teleport:
                        teleportsController.OnUseInput(useList[selectedItem].gameObject.GetComponent<Object_Teleport>());//Отправляем начало телепортации.
                        break;
                    case UseData_ObjType.Door:
                        doorController.OnUseInput(useList[selectedItem].gameObject.GetComponent<Object_DoorConnecter>());//Отправляем использование двери.
                        break;
                }
            }
        }
    }

    public void AddUseItem(GameObject newItem, UseData_ObjType newType)
    {
        bool added = false;//Есть ли этот объект в списке.
        foreach(Property_UseData useItem in useList)//Для каждого объекта в списке используемых.
        {
            if(useItem.gameObject == newItem)//Если объект уже есть в списке.
            {
                added = true;//Помечаем, что уже добавлен.
            }
        }
        if(!added)//Если предмета нет в списке.
        {
            useList.Add(new Property_UseData(newItem, newType));//Добавляем новый объект.
        }
    }
}
