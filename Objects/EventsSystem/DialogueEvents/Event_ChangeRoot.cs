using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ChangeRoot : Object_Event
{
    public int uniqueUnitId = -1;//Для какого юнита.
    public int addingValue = 0;//Сколько рута прибавить

    public override void Initialize()
    {
        base.Initialize();
        //Object_Player playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Ссылка на данные игрока.
        Source_GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Ссылка на гейм менеджер.
        //PC_DialogueController dialogueController = Camera.main.GetComponent<PC_DialogueController>();//Ссылка на контроллер диалогов.
        gameManager.AddRootForUnit(uniqueUnitId, addingValue);//Изменяем руты для юнита.
    }
}
