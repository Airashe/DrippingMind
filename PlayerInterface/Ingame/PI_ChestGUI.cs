using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PI_ChestGUI : MonoBehaviour
{
    //Ссылки на другие скрипты.
    private Object_Player playerData;//Данные игрока.
    private PC_ChestController chestController;//Скрипт взаимодействия с сундуками.

    private int takeItemPos = -1;//id предмета для взятия.

    private void Start()
    {
        playerData = Camera.main.GetComponent<Object_Player>();//Получаем ссылку на данные игрока.
        chestController = Camera.main.GetComponent<PC_ChestController>();//Получаем ссылку на контроллер сундуков.
    }

    private void Update()
    {
        if (Input.GetKeyUp(Source_Constants.userInputs["INVENTORY_MAIN_ACTION"]))//Взятие предмета.
        {
            if (takeItemPos != -1)//Если есть предмет для взятия.
            {
                if (playerData.unit.AddItem(chestController.activeChest.items[takeItemPos]))//Если добавление предмета к игроку прошло успешно.
                {
                    chestController.activeChest.RemoveItem(takeItemPos);//Удаляем предмет из сундука.
                }
            }
        }
    }

    private void OnGUI()//Отрисовка интерфейса.
    {
        if(chestController.activeChest != null)//Если есть сундук для взаимодействия.
        {
            Camera.main.GetComponent<Object_Player>().showCursorWithoutCheck = true;//Показываем курсор.
            takeItemPos = -1;//Обнуляем выбранный предмет.
            bool toolTip_draw = false;//Нужно ли рисовать подсказку.
            Source_ItemBase toolTip_ItemData = null;//Данные предмета для подсказки.
            Vector3 chestGUIWorldPosition = chestController.activeChest.transform.position;//Позиция сундука на сцене.
            Vector3 chestInventoryPosition = Camera.main.WorldToScreenPoint(chestGUIWorldPosition);//Получаем позицию сундука на экране.
            int countRows = chestController.activeChest.maxWeight / Source_Constants.CONST_INTERFACE_CHEST_INVENTORY_MAX_ROW_LENGTH + 1;//Считаем количество строк.
            Vector2 cellSize = new Vector2(30,30);//Размелы слотов сундука.
            int rowCellCounter = 0;//Счетчик ячеек.
            Vector2 windowSize = new Vector2(4 + (Source_Constants.CONST_INTERFACE_CHEST_INVENTORY_MAX_ROW_LENGTH* cellSize.x), 30 + (countRows*cellSize.y));//Размеры окна.
            Vector2 cellPosition = new Vector2(chestInventoryPosition.x + 2, chestInventoryPosition.y + 25);//Позиция ячейки.
            if (chestController.activeChest.interfaceStyle == null)//Если у сундука нет собственного интерфейса.
            {
                GUI.Box(new Rect(chestInventoryPosition.x, chestInventoryPosition.y, windowSize.x, windowSize.y), chestController.activeChest.chestName);//Окно сундука.
            }
            else
            {
                GUI.DrawTexture(new Rect(chestInventoryPosition.x, chestInventoryPosition.y, windowSize.x, windowSize.y), chestController.activeChest.interfaceStyle.windowBackground);//Окно сундука.
            }
            //Ячейки с предметами.
            for(int chest_itemId = 0; chest_itemId < chestController.activeChest.items.Count; chest_itemId++)//Для каждого предмета в сундуке.
            {
                Source_ItemBase item = chestController.activeChest.items[chest_itemId].data;//Предмет.
                if (chestController.activeChest.interfaceStyle == null)//Если у сундука нет собственного интерфейса.
                {
                    GUI.Box(new Rect(cellPosition, cellSize), "", playerData.interfaceSkin.customStyles[3]);//Рисуем ячейку.
                }
                else
                {
                    GUI.DrawTexture(new Rect(cellPosition, cellSize), chestController.activeChest.interfaceStyle.cellBackground);//Рисуем ячейку.
                }

                if(chestController.activeChest.items[chest_itemId].data != null)//Если есть данные предмета.
                {
                    GUI.DrawTexture(new Rect(cellPosition, cellSize), item.icon);//Отрисовываем иконку предмета в ячейке.
                }

                if (new Rect(cellPosition, cellSize).Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))//Если курсор мыши над этой ячейкой.
                {
                    toolTip_draw = true;//Помечаем, что подсказку нужно отрисовывать.
                    toolTip_ItemData = item;//Предмет для которого будет нарисована подсказка.
                    takeItemPos = chest_itemId;//Запоминаем предмет, который хотим взять.
                }

                cellPosition.x += cellSize.x;//Передвигаем.
                rowCellCounter += 1;//Прибавляем счетчик ячеек.
                if(rowCellCounter >= Source_Constants.CONST_INTERFACE_CHEST_INVENTORY_MAX_ROW_LENGTH)//Если вышли за пределы.
                {
                    rowCellCounter = 0;//Обнуляем счетчик.
                    cellPosition.x = chestInventoryPosition.x + 2;//Обнуляем позицию.
                    cellPosition.y += cellSize.y;//Передвигаем на новую строку.
                }
            }
            
            //Ячейки для оставшегося места.
            for(int cell_id = 0; cell_id < chestController.activeChest.LeftWeight; cell_id++)//Для каждой пустой ячейки.
            {
                if (chestController.activeChest.interfaceStyle == null)//Если у сундука нет собственного интерфейса.
                {
                    GUI.Box(new Rect(cellPosition, cellSize), "", playerData.interfaceSkin.customStyles[3]);//Рисуем ячейку.
                }
                else
                {
                    GUI.DrawTexture(new Rect(cellPosition, cellSize), chestController.activeChest.interfaceStyle.cellBackground);//Рисуем ячейку.
                }
                cellPosition.x += cellSize.x;//Передвигаем.
                rowCellCounter += 1;//Прибавляем счетчик ячеек.
                if (rowCellCounter >= Source_Constants.CONST_INTERFACE_CHEST_INVENTORY_MAX_ROW_LENGTH)//Если вышли за пределы.
                {
                    rowCellCounter = 0;//Обнуляем счетчик.
                    cellPosition.x = chestInventoryPosition.x + 2;//Обнуляем позицию.
                    cellPosition.y += cellSize.y;//Передвигаем на новую строку.
                }
            }

            if (toolTip_draw && toolTip_ItemData != null)//Если нужно рисовать подсказку.
            {
                Vector2 toolTip_position = new Vector2(Input.mousePosition.x + 10, Screen.height - Input.mousePosition.y);
                GUI.Box(new Rect(toolTip_position.x, toolTip_position.y, 200, 100), "", playerData.interfaceSkin.customStyles[4]);//Рамка окошка подсказки.
                GUI.Button(new Rect(toolTip_position.x, toolTip_position.y, 30, 30), "", playerData.interfaceSkin.customStyles[3]);//Рамка для иконки предмета.
                GUI.DrawTexture(new Rect(toolTip_position.x + 1, toolTip_position.y + 1, 30, 30), toolTip_ItemData.icon);//Иконка предмета.
                GUI.Label(new Rect(toolTip_position.x + 31, toolTip_position.y + 1, 168, 30), toolTip_ItemData.name, playerData.interfaceSkin.customStyles[5]);//Название предмета.
                GUI.Label(new Rect(toolTip_position.x + 1, toolTip_position.y + 32, 198, 167), toolTip_ItemData.description, playerData.interfaceSkin.customStyles[6]);//Описание предмета.
            }
        }
        else
        {
            Camera.main.GetComponent<Object_Player>().showCursorWithoutCheck = false;//Скрываем курсор.
        }
    }
}
