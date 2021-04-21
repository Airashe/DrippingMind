using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_AddNewQuest : Object_Event
{
    public Source_Quest newQuest;//Новое задание.

    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<PC_Notepad>().AddQuest(newQuest);
    }
}
