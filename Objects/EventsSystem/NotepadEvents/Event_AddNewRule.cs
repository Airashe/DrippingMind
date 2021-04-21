using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_AddNewRule : Object_Event
{
    public int questId;
    public Source_QuestRule newRule;

    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<PC_Notepad>().AddRuleToQuest(questId, newRule);
    }
}
