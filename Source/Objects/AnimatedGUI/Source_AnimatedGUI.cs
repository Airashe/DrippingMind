using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New model", menuName = "Dripping Mind/GUI/Анимированная GUI текстура")]
public class Source_AnimatedGUI : ScriptableObject
{
    public new string name;//Название файла.
    public List<Texture2D> frames;//Кадры анимации.
}
