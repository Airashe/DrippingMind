using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Source_Quest
{
    public int uniqueId;//Уникальный id.
    public string name;//Название задания.
    public List<Source_QuestRule> rules;//Задачи задания.
    public Quest_Status Status
    {
        get
        {
            int completedRules = 0;//Количество выполненых задач.
            int faliedRules = 0;//Количество просраных задач.
            if (rules.Count > 0)//Если есть задачи.
            {
                foreach (Source_QuestRule rule in rules)//Для каждой задачи в списке задач.
                {
                    switch (rule.status)
                    {
                        case Quest_Status.Completed:
                            completedRules += 1;
                            break;
                        case Quest_Status.Failed:
                            faliedRules += 1;
                            break;
                    }
                }
                if (faliedRules > 0)
                {
                    return Quest_Status.Failed;
                }
                if (completedRules == rules.Count)
                {
                    return Quest_Status.Completed;
                }
            }
            return Quest_Status.inProgress;
        }
    }

    public string Name
    {
        get
        {
            string resultName = name;
            switch(Status)
            {
                case Quest_Status.Completed:
                    resultName += " (Completed)";
                    break;
                case Quest_Status.Failed:
                    resultName += " (Failed)";
                    break;
            }
            return resultName;//Возврщаем результативное имя.
        }
    }

    public Source_Quest(string name, List<Source_QuestRule> rules)
    {
        this.name = name;
        this.rules = rules;
    }
}
public enum Quest_Status { inProgress, Completed, Failed}
