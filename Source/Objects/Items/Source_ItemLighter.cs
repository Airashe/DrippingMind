using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Dripping Mind/Предметы/Источник света")]
public class Source_ItemLighter : Source_ItemBase
{
    public override void UseEffect(Object_Unit unit, int groupId, int itemPos)
    {
        base.UseEffect(unit, groupId, itemPos);
        if(unit.inventory.groups[groupId].items[itemPos].childrenGO != null)//Если есть дочерний объект.
        {
            Light lightComponent = unit.inventory.groups[groupId].items[itemPos].childrenGO.GetComponent<Light>();//Ссылка на компонент света.
            Light secondLight = unit.inventory.groups[groupId].items[itemPos].childrenGO.transform.Find("SecondLighter").GetComponent<Light>();
            if (lightComponent != null)
            {
                lightComponent.enabled = !lightComponent.enabled;
            }
            if (secondLight != null)
            {
                secondLight.enabled = !secondLight.enabled;
            }
        }
    }
}
