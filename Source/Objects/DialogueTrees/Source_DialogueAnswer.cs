using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Source_DialogueAnswer
{
    public string longVariant;//Что скажет персонаж.
    public int[] evenetsId;//ID событий, которые должны быть активированы.
}
