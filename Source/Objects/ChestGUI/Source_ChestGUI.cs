using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New model", menuName = "Dripping Mind/GUI/Текстура сундука")]
public class Source_ChestGUI : ScriptableObject
{
    public new string name;//Название набора.
    public Texture2D windowBackground;//Текстурка фона.
    public Texture2D cellBackground;//Текстурка ячейки.
}
