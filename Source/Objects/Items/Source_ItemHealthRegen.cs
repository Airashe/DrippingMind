using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Dripping Mind/Предметы/Регенератор здоровья")]
public class Source_ItemHealthRegen : Source_ItemBase
{
    public float percentRegen = 0;//Регенерация по процентам.
    public int healthRegen = 0;//Регенерация по четкому значению.

    public override void UseEffect(Object_Unit unit, int groupId, int itemPos)
    {
        base.UseEffect(unit, groupId, itemPos);
            if (percentRegen > 0)//Если есть процент регена.
            {
                float newHp = (float)unit.MaxHealth * (percentRegen / 100);
                unit.currentHealth += (int)newHp;
            }
    }
}
