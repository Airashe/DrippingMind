using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_SaveUnitInventory : Object_Event
{
    public override void Initialize()
    {
        base.Initialize();
        if(Camera.main.GetComponent<Object_Player>().unit != null)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>().Save_UnitInventory();
        }
    }
}
