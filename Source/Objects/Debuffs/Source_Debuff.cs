using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New debuff", menuName = "Dripping Mind/Фобии/Новая фобия")]
public class Source_Debuff : ScriptableObject
{
    public new string name;//Название дебафа.
    public Texture2D icon;//Иконка дебафа.
    public int loseHealth;//Сколько здоровья отнимается.
}
