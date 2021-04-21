using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PI_IgInventory : MonoBehaviour
{
    public IngameGUI_Tabs activeTab = IngameGUI_Tabs.Inventory;//Открытая вкладка интерфейса.
    public bool Active
    {
        get
        {
            if(!playerController.enabled)//Если управление персонажем недоступно.
            {
                if(!dialogueController.enabled)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }

    //Звуки.
    public AudioClip tabClick;//Звук тыканья по вкладкам инвентаря.
    public AudioClip changeState;//Звук открытия закрытия инвентаря.

    //Ссылки на объекты.
    private Object_Player playerData;//Данные игрока.
    private PC_CharacterController playerController;//Скрипт управления юнитом.
    private PC_DialogueController dialogueController;//Скрипт управления диалогами.
    private PC_ChestController chestController;//Скрипт управления сундуками.
    private PC_TriggerUseController triggerController;//Скрипт управления триггерами.
    private PC_ItemsController itemsController;//Скрипт подбирания предметов.
    private PC_MainUseLogic mainUseLogic;//Скрипт отвечающий за взаимодействие с объектами.
    private PC_CharacterNoizes soundInterface;//Управление звуком игрока.s
    private PC_DoorController doorController;//Скрипт отвечающий за поиск дверей.
    private PC_TeleportController teleportController;//Скрипт отвечающий за использование телепортов.
    private PI_IngameGUI ingameInterface;//Внутриигровой интерфейс.
    private PI_ChestGUI chestInterface;//Интерфейс сундуков.
    private PI_Notepad notepadInterface;//Скрипт интерфейса блокнотика.
    private PI_Dialogue dialogueInterace;//Скрипт интерфейса диалогов.
    private PI_Map mapInterface;//Скрипт интерфейса карты.
    private PC_DetectiveMode detectiveMode;//Скрипт отвечающий за режим детектива.
    
    //Элементы сцены инвентаря.
    private Transform inventoryScene;//Сцена инвентаря.
    private Object_Model playerModel;//Моделька игрока.

    //Переменные выскакивающего меню.
    private bool itemMenu_draw = false;//Рисовать ли меню для предмета.
    private int itemMenu_groupId = -1;//id группы, в которой лежит предмет.
    private int itemMenu_itemPos = -1;//Позиция предмета в группе, для которого открыто меню.
    private Vector2 itemMenu_startPosition = new Vector2(0, 0);//Позиция меню предмета.

    //Переменные перетаскивания предметов.
    private int transition_start_groupId = -1;//id группы из которой забирается предмет.
    private int transition_start_itemPos = -1;//Позиция предмета в группе.

    //Контроллеры анимированного GUI.
    public Source_AnimationController loadingBubbleAC;//Контроллер анимации сердца.
    private Vector3 CameraRotationBefore;

    private void Awake()
    {
        playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Ссылка на данные игрока.
        playerController = gameObject.GetComponent<PC_CharacterController>();//Получаем ссылку на управление юнитом.
        dialogueController = gameObject.GetComponent<PC_DialogueController>();//Получаем ссылку на контроллер диалогов.
        chestController = Camera.main.gameObject.GetComponent<PC_ChestController>();//Получаем ссылку на контроллер сундуков.
        triggerController = Camera.main.gameObject.GetComponent<PC_TriggerUseController>();//Скрипт управления триггерами.
        notepadInterface = gameObject.GetComponent<PI_Notepad>();//Получаем ссылку на контроллер заданий.
        ingameInterface = gameObject.GetComponent<PI_IngameGUI>();//Получаем ссылку на внутриигровой интерфейс.
        chestInterface = gameObject.GetComponent<PI_ChestGUI>();//Получаем ссылку на интерфейс сундуков.
        mapInterface = gameObject.GetComponent<PI_Map>();//Получаем ссылку на карту.
        soundInterface = gameObject.GetComponent<PC_CharacterNoizes>();//Скрипт отвечающие за звуки.
        itemsController = gameObject.GetComponent<PC_ItemsController>();//Скрипт отвечающий за подбирание предметов.
        dialogueInterace = gameObject.GetComponent<PI_Dialogue>();//Скрипт интерфейса диалогов.
        mainUseLogic = gameObject.GetComponent<PC_MainUseLogic>();//Скрипт интерфейса диалогов.
        doorController = gameObject.GetComponent<PC_DoorController>();//Скрипт управления дверьми.
        teleportController = gameObject.GetComponent<PC_TeleportController>();//Скрипт управления телепортами.
        detectiveMode = gameObject.GetComponent<PC_DetectiveMode>();//Ссылка на скрипт детективного режима.
        inventoryScene = GameObject.FindGameObjectWithTag("Subscene_Inventory").transform;//Получаем ссылку на родительский объект сцены инвентаря.
        playerModel = inventoryScene.transform.Find("InventoryModel").GetComponent<Object_Model>();//Модель игрока на сцене инвентаря.
        ChangeSceneVisability(false);//Скрываем интерфейс, при старте сцены.
    }

    private void Update()
    {
        if(Input.GetKeyDown(Source_Constants.userInputs["INVENTORY_CHANGESTATE"]))//Если отпущена кнопка инвентаря.
        {
            ChangeSceneVisability(playerController.enabled);//Имзенение видимости инвентаря, в зависимости от его текущего состояния.
            activeTab = IngameGUI_Tabs.Inventory;//Активная вкладка инвентарь.
            soundInterface.Play_InventorySoundEffect(changeState, 0.5f);
        }
        if(loadingBubbleAC != null)//Если есть контроллер анимации.
        {
            loadingBubbleAC.Play();//Проигрываем анимацию сердца.
        }
        inventoryScene.transform.position = Camera.main.transform.position;
    }

    private void OnGUI()
    {
        if(!playerController.enabled)//Если управление игроком отключено.
        {
            if(playerData.interfaceSkin != null && playerData.healthAnimation != null)//Если есть основной скин.
            {
                if (activeTab == IngameGUI_Tabs.Inventory)
                {
                    GUI_Inventory();//Рисуем инвентарь
                }
                if (activeTab == IngameGUI_Tabs.Notepad)
                {
                    notepadInterface.NotepadGUI();//Рисуем блокнот.
                }
                if(activeTab == IngameGUI_Tabs.Map)
                {
                    mapInterface.MapGUI();//Рисуем карту.
                }
                GUI_Tabs();//Вкладки
            }
        }
    }
    //Subscene_Inventory_CamPos

    public void ChangeSceneVisability(bool visible)
    {
        if(visible)//Если делаем видимым.
        {
            CameraRotationBefore = Camera.main.transform.eulerAngles;//Запоминаем текущий поворот камеры.
            Camera.main.transform.eulerAngles = Vector3.zero;//Устанавливаем нулевой поворот камеры.
        }
        else//Если делаем невидимым.
        {
            Camera.main.transform.eulerAngles = CameraRotationBefore;//Восстанавливаем поворот камеры.
        }
        playerController.enabled = !visible;//Отключаем/включаем управление юнитом.
        dialogueController.enabled = !visible;
        chestController.enabled = !visible;
        chestInterface.enabled = !visible;
        ingameInterface.enabled = !visible;
        triggerController.enabled = !visible;
        itemsController.enabled = !visible;
        dialogueInterace.enabled = !visible;
        mainUseLogic.enabled = !visible;
        doorController.enabled = !visible;
        teleportController.enabled = !visible;

        for (int i = 0; i < inventoryScene.childCount; i++)//Пока i меньше количества дочерних объектов.
        {
            inventoryScene.GetChild(i).gameObject.SetActive(visible);//Активируем или деактивируем объекты сцены.
        }

        playerData.activeWorldLayerMask = !visible ? Camera.main.GetComponent<UnityStandardAssets.ImageEffects.Grayscale>().enabled ? playerData.detectiveMask : playerData.worldMask : playerData.interfaceMask;//Отображаем нужную маску в зависимости от состояния игрока.
        if(!visible)
        {
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.Grayscale>().enabled = Camera.main.GetComponent<PC_DetectiveMode>().active ? true : false;
        }
        else
        {
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.Grayscale>().enabled = false;
        }
        playerData.cameraState = !visible ? Camera_State.Await : Camera_State.Busy;//Переводим камеру в нужное состояние.

        if(visible)//Если нужно показывать инвентарь.
        {
            playerModel.SetModel(playerData.unit.model.modelData, "Idle0");//Устанавливаем такую же модель, как у игрока.
        }
    }

    private void GUI_Tabs()
    {
        if (GUI.Button(new Rect(Screen.width / 2, 0, 180, 30), "Map", playerData.interfaceSkin.button))
        {
            activeTab = IngameGUI_Tabs.Map;
            soundInterface.Play_InventorySoundEffect(tabClick, 0.047f);
        }
        if (GUI.Button(new Rect(Screen.width / 2 + 180, 0, 180, 30), "Main menu", playerData.interfaceSkin.button))
        {
            soundInterface.Play_InventorySoundEffect(tabClick, 0.047f);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>().GoToNextScene(4);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 360, 0, 180, 30), "Inventory", playerData.interfaceSkin.button))
        {
            activeTab = IngameGUI_Tabs.Inventory;
            soundInterface.Play_InventorySoundEffect(tabClick, 0.047f);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 180, 0, 180, 30), "Notepad", playerData.interfaceSkin.button))
        {
            activeTab = IngameGUI_Tabs.Notepad;
            soundInterface.Play_InventorySoundEffect(tabClick, 0.047f);
        }
    }

    private void GUI_Inventory()
    {
        Rect windowStats = new Rect(Screen.width / 2 - 457, Screen.height / 2 - 157, 314, 314);//Позиция окна.

        GUI.Box(windowStats, "", playerData.interfaceSkin.box);//Рамка
        if (loadingBubbleAC != null)//Если есть объект контроллера.
        {
            float speedRate = 1 + ((1 - ((float)playerData.unit.currentHealth / (float)playerData.unit.MaxHealth)) * 2f);//Скорость дыхания.
            loadingBubbleAC.animationSpeed = 5 * speedRate;
            GUI.DrawTexture(new Rect(windowStats.x + windowStats.width - 49, windowStats.y + 8, 41, 54), playerData.interfaceSkin.customStyles[0].hover.background);//Граница сердца.
            GUI.DrawTexture(new Rect(windowStats.x + windowStats.width - 47, windowStats.y + 10, 37, 50), loadingBubbleAC.CurrentFrame);//Рисуем сердце.

        }
        else
        {
            loadingBubbleAC = new Source_AnimationController(playerData.healthAnimation, 5, true);//Создаем контроллер анимации сердца.
        }
        windowStats.x += 7;
        windowStats.y += 7;
        windowStats.width -= 14;
        windowStats.height -= 14;

        GUI.DrawTexture(new Rect(windowStats.x + 8, windowStats.y + 8, 61, 54), playerData.interfaceSkin.customStyles[0].active.background);//Рисуем границу мозга.
        GUI.DrawTexture(new Rect(windowStats.x + 10, windowStats.y + 10, 57, 50), playerData.interfaceSkin.customStyles[0].normal.background);//Рисуем иконку мозга.

        windowStats.y += 61;//Новая стартовая позиция фобий.
        windowStats.width -= 5;

        for (int phobId = 0; phobId < playerData.unit.debuffs.Count; phobId++)//Для каждой фобии.
        {
            Source_Debuff debuff = playerData.unit.debuffs[phobId];//Фобия.
            if (debuff != null)
            {
                GUI.Button(new Rect(windowStats.x + 7, windowStats.y, windowStats.width-4, 30), "", playerData.interfaceSkin.customStyles[7]);//Рамка.
                GUI.DrawTexture(new Rect(windowStats.x + 2, windowStats.y, 30, 30), debuff.icon);//Рисуем картинку фобии.
                GUI.Label(new Rect(windowStats.x + 2, windowStats.y + 5, windowStats.width-4 + 5, 20), debuff.name, playerData.interfaceSkin.customStyles[1]);//Название фобии.

                windowStats.y += 30;//Переход на следующую строку.
            }
        }

        //Рисование фобий.

        bool toolTip_draw = false;//Рисовать ли подсказку для предмета.
        Source_ItemBase toolTip_ItemData = null;//Для какого предмета рисуется подсказка.

        for(int groupId = 0; groupId < playerData.unit.inventory.groups.Count; groupId++)//Для каждой группы слотов.
        {
            Vector3 groupStartPosition = Camera.main.WorldToScreenPoint(
                playerModel.transform.position + playerData.unit.inventory.groups[groupId].scenePosition);//Определяем стартовую позицию группы на экране.

            string groupName = playerData.unit.inventory.groups[groupId].name;//Имя группы.
            
            GUI.Label(new Rect(groupStartPosition.x, Screen.height - groupStartPosition.y, groupName.Length*8, 20), groupName, playerData.interfaceSkin.customStyles[2]);

            Rect cellPosition = new Rect(groupStartPosition.x, Screen.height - groupStartPosition.y + 20, 30, 30);//Позиция иконки на экране.
            int cellCounter = 0;//Сколько ячеек уже было отрисовано.

            for (int p = 0; p < playerData.unit.inventory.groups[groupId].items.Count; p++)//Для каждого предмета.
            {
                Source_ItemBase item = playerData.unit.inventory.groups[groupId].items[p].data;//Предмет.
                if (item != null)
                {
                    GUI.Box(cellPosition, "", playerData.interfaceSkin.customStyles[3]);//Отрисовываем ячейку предмета.
                    if (transition_start_groupId != groupId || transition_start_itemPos != p)//Если этот предмет не перетаскивается в данный момент.
                    {
                        GUI.DrawTexture(new Rect(cellPosition.x+5, cellPosition.y+5, cellPosition.width-10, cellPosition.height-10), item.icon);//Отрисовываем иконку предмета в ячейке.
                    }

                    if (cellPosition.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))//Если курсор мыши над этой ячейкой.
                    {
                        toolTip_draw = !itemMenu_draw;//Отрисовывать подсказку, если не открыто меню.
                        toolTip_ItemData = item;//Предмет, для которго отображается подсказка.
                        if(Input.GetKeyUp(Source_Constants.userInputs["INVENTORY_ITEMMENU_SHOW"]))//Если нажата клавиша открытия меню.
                        {
                            itemMenu_draw = true;//Отрисовывать меню предмета - да.
                            itemMenu_groupId = groupId;//Запоминаем группу.
                            itemMenu_itemPos = p;//Запоминаем позицию предмета.
                            itemMenu_startPosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);//Стартовая позиция для меню предметов.
                        }
                        if (Input.GetKeyDown(Source_Constants.userInputs["INVENTORY_MAIN_ACTION"]))//Начало перемещение предмета.
                        {
                            transition_start_groupId = groupId;//Запоминаем группу из которой делается перемещение.
                            transition_start_itemPos = p;//Запоминаем позицию предмета в группе.
                        }
                        if (Input.GetKeyUp(Source_Constants.userInputs["INVENTORY_MAIN_ACTION"]))//Конец перемещения предмета.
                        {
                            if (transition_start_groupId >= 0 && transition_start_groupId < playerData.unit.inventory.groups.Count)
                            {
                                if (transition_start_itemPos >= 0 && transition_start_itemPos < playerData.unit.inventory.groups[transition_start_groupId].items.Count)
                                {
                                    playerData.unit.SwitchItems(transition_start_groupId, transition_start_itemPos, groupId, p);//Меняем предметы местами.
                                }
                            }
                            transition_start_groupId = -1;//Обнуляем перетаскиваемый предмет в случаи успеха.
                            transition_start_itemPos = -1;//Обнуляем перетаскиваемый предмет в случаи успеха.
                        }
                    }
                    cellPosition.x += 30;//Передвигаем следующую ячейку на 1 вперед по сетке.
                    cellCounter += 1;//Прибавляем ячейку.
                    if (cellCounter >= Source_Constants.CONST_INTERFACE_INVENTORY_GROUP_MAX_ROW_LENGTH)//Если вышли за пределы.
                    {
                        cellCounter = 0;//Обнуляем счетчик.
                        cellPosition.x = groupStartPosition.x;//Обнуляем позицию по х.
                        cellPosition.y += 30;//Переходим на следующую строку.
                    }
                }
            }
            if (playerData.unit.inventory.groups[groupId].maxWeight > 0)//Если в слоте есть ограниченное количество предметов.
            {
                for (int i = 0; i < playerData.unit.inventory.groups[groupId].LeftWeight; i++)//Для каждого слота в группе.
                {
                    GUI.Box(cellPosition, "", playerData.interfaceSkin.customStyles[3]);//Отрисовываем пустую ячейку.

                    if (cellPosition.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))//Если курсор мыши над этой ячейкой.
                    {
                        if (Input.GetKeyUp(Source_Constants.userInputs["INVENTORY_MAIN_ACTION"]))//Конец перемещения предмета.
                        {
                            if (transition_start_groupId >= 0 && transition_start_groupId < playerData.unit.inventory.groups.Count)
                            {
                                if (transition_start_itemPos >= 0 && transition_start_itemPos < playerData.unit.inventory.groups[transition_start_groupId].items.Count)
                                {
                                    playerData.unit.SwitchItems(transition_start_groupId, transition_start_itemPos, groupId, -1);//Меняем предметы местами.
                                }
                            }
                            transition_start_groupId = -1;//Обнуляем перетаскиваемый предмет в случаи успеха.
                            transition_start_itemPos = -1;//Обнуляем перетаскиваемый предмет в случаи успеха.
                        }
                    }

                    cellPosition.x += 30;//Передвигаем следующую ячейку на 1 вперед по сетке.
                    cellCounter += 1;//Прибавляем ячейку.
                    if (cellCounter >= Source_Constants.CONST_INTERFACE_INVENTORY_GROUP_MAX_ROW_LENGTH)//Если вышли за пределы.
                    {
                        cellCounter = 0;//Обнуляем счетчик.
                        cellPosition.x = groupStartPosition.x;//Обнуляем позицию по х.
                        cellPosition.y += 30;//Переходим на следующую строку.
                    }
                }
            }
            else//Если в группе неограниченный вес.
            {
                if(playerData.unit.inventory.groups[groupId].items.Count == 0)//Если в неограниченной группе не лежит предметов.
                {
                    GUI.Box(cellPosition, "", playerData.interfaceSkin.customStyles[3]);//Отрисовываем пустую ячейку.
                    if (cellPosition.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))//Если курсор мыши над этой ячейкой.
                    {
                        if (Input.GetKeyUp(Source_Constants.userInputs["INVENTORY_MAIN_ACTION"]))//Конец перемещения предмета.
                        {
                            if (transition_start_groupId >= 0 && transition_start_groupId < playerData.unit.inventory.groups.Count)
                            {
                                if (transition_start_itemPos >= 0 && transition_start_itemPos < playerData.unit.inventory.groups[transition_start_groupId].items.Count)
                                {
                                    playerData.unit.SwitchItems(transition_start_groupId, transition_start_itemPos, groupId, -1);//Меняем предметы местами.
                                }
                            }
                            transition_start_groupId = -1;//Обнуляем перетаскиваемый предмет в случаи успеха.
                            transition_start_itemPos = -1;//Обнуляем перетаскиваемый предмет в случаи успеха.
                        }
                    }
                }
            }
        }

        if(transition_start_groupId != -1 && transition_start_itemPos != -1)//Если есть группа из которой переноситься предмет.
        {
            if (transition_start_groupId >= 0 && transition_start_groupId < playerData.unit.inventory.groups.Count)
            {
                if (transition_start_itemPos >= 0 && transition_start_itemPos < playerData.unit.inventory.groups[transition_start_groupId].items.Count)
                {
                    GUI.DrawTexture(new Rect(
                                    new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y),
                                    new Vector2(30, 30)),
                                   playerData.unit.inventory.groups[transition_start_groupId].items[transition_start_itemPos].data.icon);//Отрисовываем иконку предмета у курсора мыши.
                }
                else
                {
                    transition_start_groupId = -1;
                    transition_start_itemPos = -1;
                }
            }
            else
            {
                transition_start_groupId = -1;
                transition_start_itemPos = -1;
            }
        }

        if(itemMenu_draw)//Если нужно отрисовать меню предмета.
        {
            bool outsideOfMenu = false;//Находился ли курсор за пределами меню.
            Vector2 itemMenu_Size = new Vector2(100,0);//Размеры меню предмета.
            //Отрисовывать специальное меню.

            //Отрисовка стандартного меню, для всех предметов.
            if (GUI.Button(new Rect(itemMenu_startPosition, new Vector2(100, 20)), "Drop out"))//Если нажата кнопка "выкинуть предмет".
            {
                playerData.unit.DropItem(itemMenu_groupId, itemMenu_itemPos);//Выбрасывание предмета из инвентаря.
                itemMenu_draw = false;//Скрываем меню.
            }
            itemMenu_Size.y += 20;//Прибавляем к ширине меню высоту одной кнопки.

            outsideOfMenu = !new Rect(itemMenu_startPosition, itemMenu_Size).Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));//Если курсор мыши над этой ячейкой.

            if (Input.GetKeyUp(Source_Constants.userInputs["INVENTORY_MAIN_ACTION"]))//Если нажата кнопка основного действия.
            {
                if(outsideOfMenu)//Если курсор за пределами меню.
                {
                    itemMenu_draw = false;//Скрываем меню.
                }
            }
        }

        if(toolTip_draw && toolTip_ItemData != null)//Если нужно рисовать подсказку.
        {
            Vector2 toolTip_position = new Vector2(Input.mousePosition.x + 10, Screen.height - Input.mousePosition.y);
            GUI.Box(new Rect(toolTip_position.x, toolTip_position.y, 200, 100), "", playerData.interfaceSkin.customStyles[4]);//Рамка окошка подсказки.
            GUI.Button(new Rect(toolTip_position.x, toolTip_position.y, 30, 30), "", playerData.interfaceSkin.customStyles[3]);//Рамка для иконки предмета.
            GUI.DrawTexture(new Rect(toolTip_position.x + 1, toolTip_position.y + 1, 30, 30), toolTip_ItemData.icon);//Иконка предмета.
            GUI.Label(new Rect(toolTip_position.x + 31, toolTip_position.y + 1, 168, 30), toolTip_ItemData.name, playerData.interfaceSkin.customStyles[5]);//Название предмета.
            GUI.Label(new Rect(toolTip_position.x + 1, toolTip_position.y + 32, 198, 167), toolTip_ItemData.description, playerData.interfaceSkin.customStyles[6]);//Описание предмета.
        }
    }
}

public enum IngameGUI_Tabs { Inventory, Map, Notepad}