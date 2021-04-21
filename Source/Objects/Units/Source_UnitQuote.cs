using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Dripping Mind/Диалоги/Фраза персонажа")]
public class Source_UnitQuote : ScriptableObject
{
    public string line;//То, что скажет юнит.
    public float existTime;//Сколько будет проигрываться фраза.
    public int[] evenetsId;//ID событий, которые должны быть активированы.
}
