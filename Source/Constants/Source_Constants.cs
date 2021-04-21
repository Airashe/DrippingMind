using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Source_Constants
{
    public static float CONST_METRIC_WORLD_DISNTACE_IN_UNITS = 1;//На сколько сокращается дистанция расстояния.

    public static float CONST_GAMEPLAY_PLAYER_PICKUP_ITEM_DISTANCE = 0.5f;//Дистанция, на которой игрок может поднимать предметы.
    public static float CONST_GAMEPLAY_PLAYER_DIALOGUE_START_DISTANCE = 0.6f;//Дистанция, на которой игрок может говорить с юнитами.
    public static float CONST_GAMEPLAY_PLAYER_USE_CHEST_DISTANCE = 0.6f;//Дистанция, на которой игрок может взаимодействовать с сундуками.
    public static float CONST_GAMEPLAY_PLAYER_USE_TELEPORT_DISTANCE = 0.5f;//Дистанция на которой игрок может использовать телепорт.
    public static float CONST_GAMEPLAY_PLAYER_USE_DOORCONNECTOR_DISTANCE = 0.5f;//Дистанция на которой игрок может дотянуться до двери.

    public static int CONST_INTERFACE_INVENTORY_GROUP_MAX_ROW_LENGTH = 3;//Максимальная длинна строк группы инвентаря.
    public static int CONST_INTERFACE_CHEST_INVENTORY_MAX_ROW_LENGTH = 4;//Максимальная длинна строк сундука.

    public static Dictionary<string, KeyCode> userInputs = new Dictionary<string, KeyCode>//Горячие клавиши управления.
    {
        { "MOVEMENT_MOVEFORWARD", KeyCode.W }, //Движение вперед.
        { "MOVEMENT_MOVEBACKWARD", KeyCode.S }, //Движение назад.
        { "MOVEMENT_MOVELEFT", KeyCode.A }, //Движение влево.
        { "MOVEMENT_MOVERIGHT", KeyCode.D }, //Движение вправо.
        { "INVENTORY_CHANGESTATE", KeyCode.I }, //Открытие инвентаря.
        { "INVENTORY_ITEMMENU_SHOW", KeyCode.Mouse1 }, //Открыть меню инвентаря.
        { "INVENTORY_PICKUP_ITEM", KeyCode.E }, //Взамодействие с интерактивными объектами.
        { "INVENTORY_MAIN_ACTION", KeyCode.Mouse0 }, //Основное действие.
        { "GAMEPLAY_USE_FASTITEM_1", KeyCode.Q},//Клик левой кнопкой мыши.
        { "DIALOGUE_NEXTLINE", KeyCode.Space }, //Следующая строка диалога.
        { "DETECTIVE_MODE", KeyCode.R }//Переход в детективный режим.
    };

    public static string HotkeyTranslator(string input)
    {
        string result = input;
        if (input == "MOVEMENT_MOVEFORWARD")
        {
            result = "Moving forward";
        }
        if (input == "MOVEMENT_MOVEBACKWARD")
        {
            result = "Moving backward/Walk around";
        }
        if (input == "MOVEMENT_MOVELEFT")
        {
            result = "Moving left/Rotate left";
        }
        if (input == "MOVEMENT_MOVERIGHT")
        {
            result = "Moving right/Rotate right";
        }
        if (input == "INVENTORY_CHANGESTATE")
        {
            result = "Open/close inventory";
        }
        if (input == "INVENTORY_PICKUP_ITEM")
        {
            result = "Interact with objects";
        }
        if (input == "INVENTORY_ITEMMENU_SHOW")
        {
            result = "Show menu strip/Aim";
        }
        if (input == "INVENTORY_MAIN_ACTION")
        {
            result = "Use hands item/Main action";
        }
        if (input == "GAMEPLAY_USE_FASTITEM_1")
        {
            result = "Use second item slot";
        }
        if (input == "DIALOGUE_NEXTLINE")
        {
            result = "Dialogue next line";
        }
        if(input == "DETECTIVE_MODE")
        {
            result = "Detective mode";
        }
        return result + ": ";
    }
}
