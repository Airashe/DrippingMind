using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Source_DialogueObject
{
    public int stageRequirement;//На каком этапе должен находиться игрок, чтобы начать этот диалог.
    public string[] characterLine;//То что говорит юнит.
    public List<Source_DialogueAnswer> answers;//Варианты ответа игрока.
}
