using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Source_QuestRule
{
    public string text;//Текс задачи.
    public string Name
    {
        get
        {
            string resultText =  "-" + text;
            switch (status)
            {
                case Quest_Status.Completed:
                    resultText += " (Completed)";
                    break;
                case Quest_Status.Failed:
                    resultText += " (Failed)";
                    break;
            }
            return resultText;//Возврщаем результативное имя.
        }
    }
    public Quest_Status status;//Выполнена ли задача.

    public Source_QuestRule(string text, Quest_Status status)
    {
        this.text = text;
        this.status = status;
    }
}
