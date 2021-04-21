using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ChangePlayerPCI : Object_Event
{
    public bool PC_CharacterController;
    public bool PI_IgInventory;
    public bool PC_ItemsController;
    public bool PC_DialogueController;
    public bool PI_Dialogue;
    public bool PI_IngameGUI;
    public bool PC_TriggerUseController;
    public bool PC_ChestController;
    public bool PI_ChestGUI;
    public bool PC_Notepad;
    public bool PI_Notepad;
    public bool PI_Map;
    public bool PC_MainUseLogic;
    public bool avaibleIngameSounds;
    public bool avaibleCharacterSound;
    public bool PC_DoorController;
    public bool PC_TeleportController;
    public bool aura;
    public bool PC_DetectiveMode;

    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<PC_CharacterController>().enabled = PC_CharacterController;
        Camera.main.GetComponent<PI_IgInventory>().enabled = PI_IgInventory;
        Camera.main.GetComponent<PC_ItemsController>().enabled = PC_ItemsController;
        Camera.main.GetComponent<PC_DialogueController>().enabled = PC_DialogueController;
        Camera.main.GetComponent<PI_Dialogue>().enabled = PI_Dialogue;
        Camera.main.GetComponent<PI_IngameGUI>().enabled = PI_IngameGUI;
        Camera.main.GetComponent<PC_TriggerUseController>().enabled = PC_TriggerUseController;
        Camera.main.GetComponent<PC_ChestController>().enabled = PC_ChestController;
        Camera.main.GetComponent<PI_ChestGUI>().enabled = PI_ChestGUI;
        Camera.main.GetComponent<PC_Notepad>().enabled = PC_Notepad;
        Camera.main.GetComponent<PI_Notepad>().enabled = PI_Notepad;
        Camera.main.GetComponent<PI_Map>().enabled = PI_Map;
        Camera.main.GetComponent<PC_MainUseLogic>().enabled = PC_MainUseLogic;
        Camera.main.GetComponent<PC_DoorController>().enabled = PC_DoorController;
        Camera.main.GetComponent<PC_TeleportController>().enabled = PC_TeleportController;
        Camera.main.GetComponent<PC_SoundsController>().avaibleIngameSounds = avaibleIngameSounds;
        Camera.main.GetComponent<PC_SoundsController>().avaibleCharacterSound = avaibleCharacterSound;
        Camera.main.GetComponent<AuraAPI.Aura>().enabled = aura;
        Camera.main.GetComponent<UnityStandardAssets.ImageEffects.Grayscale>().enabled = false;
        Camera.main.GetComponent<PC_DetectiveMode>().enabled = PC_DetectiveMode;
        if(PC_DetectiveMode)
        {
            Camera.main.GetComponent<PC_DetectiveMode>().active = false;//Деактивируем режим детектива.
            Camera.main.GetComponent<PC_DetectiveMode>().cooldownTimer = 0;//Обнуляем время перезарядки.
            Camera.main.GetComponent<PC_DetectiveMode>().useTimer = Camera.main.GetComponent<PC_DetectiveMode>().useTime;//Обнуляем время использования.
        }
        if (aura)
        {
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (object go in allObjects)
            {
                if(((GameObject)go).GetComponent<AuraAPI.AuraLight>() != null)
                {
                    ((GameObject)go).GetComponent<AuraAPI.AuraLight>().enabled = true;
                }
            }
        }
    }
}
