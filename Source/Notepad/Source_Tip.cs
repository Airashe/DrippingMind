using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Source_Tip
{
    public string name;//Имя заметки.
    public string description;//Описание заметки.
    public int lineCount;//Сколько линий будет занимать подсказка.

    public Source_Tip(string name, string description, int lineCount)
    {
        this.name = name;//Имя.
        this.description = description;//Описание.
        this.lineCount = lineCount;
    }
}
