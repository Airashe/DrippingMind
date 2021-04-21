using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_GiveItemToUnit : Object_Event
{
    public int unitUniqueId = -1;//id юнита, которому будет выдаваться предмет.
    public Source_InventoryItem itemData;//Какой предмет будет отдан.

    public override void Initialize()
    {
        base.Initialize();
        Source_GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Ссылка на гейм менеджер.

        Object_Unit unit = gameManager.GetUnitDataByUniqueId(unitUniqueId);//Данные искомого юнита.
        if(unit != null)//Если юнит был найден.
        {
            unit.AddItem(itemData);//Выдаем предмет юниту.
        }
    }
}
