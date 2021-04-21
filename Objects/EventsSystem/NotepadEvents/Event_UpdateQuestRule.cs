using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_UpdateQuestRule : Object_Event
{
    public int questId;
    public int ruleId;
    public Quest_Status status;

    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<PC_Notepad>().UpdateQuestRule(questId, ruleId, status);
    }
}
