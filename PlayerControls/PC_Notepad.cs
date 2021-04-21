using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Notepad : MonoBehaviour
{
    public List<Source_Tip> unitTips;//Записки Говарда.
    public List<Source_Quest> quests;//Задания Говарда.
    private Object_Player playerData;//Ссылка объект игрока.

    private void Start()
    {
        playerData = Camera.main.GetComponent<Object_Player>();//Получаем ссылку на основной интерфейс игрока.
    }

    public void AddUnitTip(Source_Tip newTip)//Добавление подсказки.
    {
        unitTips.Add(newTip);//Добавление подсказки.
        playerData.ShowIngameToolTip("A new note has been added.", 5);//Отображаем подсказку.
    }

    public void AddQuest(Source_Quest newQuest)//Добавление нового задания.
    {
        quests.Add(newQuest);//Добавляем задание.
        playerData.ShowIngameToolTip("A new quest has been added.", 5);//Отображаем подсказку.
    }

    public void UpdateQuestRule(int uniqueId, int ruldeId, Quest_Status newStatus)
    {
        foreach(Source_Quest quest in quests)
        {
            if(quest.uniqueId == uniqueId)
            {
                if(quest.rules.Count > ruldeId)
                {
                    quest.rules[ruldeId].status = newStatus;
                    switch (quest.Status)
                    {
                        case Quest_Status.inProgress:
                            playerData.ShowIngameToolTip(quest.name + " has been updated.", 5);//Отображаем подсказку.
                            break;
                        case Quest_Status.Completed:
                            playerData.ShowIngameToolTip(quest.name + " completed.", 5);//Отображаем подсказку.
                            break;
                        case Quest_Status.Failed:
                            playerData.ShowIngameToolTip(quest.name + " failed.", 5);//Отображаем подсказку.
                            break;
                    }
                }
            }
        }
    }

    public void AddRuleToQuest(int uniqueId, Source_QuestRule newRule)
    {
        foreach (Source_Quest quest in quests)
        {
            if (quest.uniqueId == uniqueId)
            {
                quest.rules.Add(newRule);
            }
        }
    }

    public void RemoveRuleFromQuest(int uniqueId, int ruleId)
    {
        foreach (Source_Quest quest in quests)
        {
            if (quest.uniqueId == uniqueId)
            {
                if(ruleId < quest.rules.Count)
                {
                    quest.rules.RemoveAt(ruleId);
                }
            }
        }
    }
}
