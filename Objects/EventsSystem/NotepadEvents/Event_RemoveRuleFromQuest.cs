using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_RemoveRuleFromQuest : Object_Event
{
    public int questId;
    public int ruleId;

    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<PC_Notepad>().RemoveRuleFromQuest(questId, ruleId);
    }
}
