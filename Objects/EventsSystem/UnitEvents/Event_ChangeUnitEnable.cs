using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ChangeUnitEnable : Object_Event
{
    public Object_Unit unit;//Юнит.
    public bool enabledU;//Активен ли юнит.
    public bool dontChangePositionAfterEnabled = false;
    public Vector3 positionAfterEnabled = Vector3.zero;
    public override void Initialize()
    {
        base.Initialize();
        if(unit != null)//Если есть юнит.
        {
            if(enabledU)//Если включаем юнита.
            {
                if(!dontChangePositionAfterEnabled)//Если меняем позицию юнита после включения.
                {
                    unit.transform.position = positionAfterEnabled;//Устанавливаем новую позицию.
                }
            }
        }
        unit.gameObject.SetActive(enabledU);//Устанавливаем новое значение.
    }
}
