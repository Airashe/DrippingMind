using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Dripping Mind/Диалоги/Древо диалогов")]
public class Source_DialogueTree : ScriptableObject
{
    public new string name;//Имя древа диалогов.
    public List<Source_DT_Data> branches;//Данные древа диалогов.

    public int GetBranchIndex(int unitUniqueId)//Получение позиции ветки диалогов, для конкретного юнита.
    {
        for(int i = 0;i < branches.Count; i++)//Для каждой ветки диалогов.
        {
            if(branches[i].unitUniqueID == unitUniqueId)//Если нужный нам юнит.
            {
                return i;//Возвращаем позицию диалогов.
            }
            continue;
        }
        return -1;//Иначе возвращаем пустоту.
    }

    public int GetDialogueId(int branchId, int currentStage)//Получаем конкретный диалог.
    {
        for(int i = 0; i < branches[branchId].dialogues.Count; i++)//Для каждого диалога.
        {
            if(branches[branchId].dialogues[i].stageRequirement == currentStage)//Текущее состояние = требуемому.
            {
                return i;//Возвращаем позицию диалога.
            }
            continue;
        }
        return -1;//Иначе возвращаем, что диалога не существует.
    }

    public string[] GetUnitLine(int branchId, int dialogueId)//Возвращает фразу юнита, из конкретного диалога.
    {
        return branches[branchId].dialogues[dialogueId].characterLine;//Возвращаем фразу юнита.
    }

    public List<string> GetAnswers(int branchId, int dialogueId)
    {
        List<string> result = new List<string>();
        foreach(Source_DialogueAnswer answer in branches[branchId].dialogues[dialogueId].answers)
        {
            result.Add(answer.longVariant);
        }
        return result;
    }
}
