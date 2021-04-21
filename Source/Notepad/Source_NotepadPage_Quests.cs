using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source_NotepadPage_Quests
{
    public List<Source_Quest> questList;
    public int CurrentLines
    {
        get
        {
            int result = 0;
            foreach(Source_Quest quest in questList)
            {
                foreach(Source_QuestRule rule in quest.rules)
                {
                    result += 1;
                }
                result += 1;
            }
            return result;
        }
    }

    public Source_NotepadPage_Quests()
    {
        questList = new List<Source_Quest>();
    }
}
