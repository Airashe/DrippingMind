using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_AddUnitTip : Object_Event
{
    public Source_Tip newTip;//Новое задание.

    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<PC_Notepad>().AddUnitTip(newTip);
    }
}
