using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Dripping Mind/Предметы/Оружие")]
public class Source_ItemWeapon : Source_ItemBase
{
    public List<AudioClip> shotSounds;
    public int damage;//Урон от оружия.
    public override void UseEffect(Object_Unit unit, int groupId, int itemPos)
    {
        base.UseEffect(unit, groupId, itemPos);
        /*Vector3 shootPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        GameObject child = unit.inventory.groups[groupId].items[itemPos].childrenGO;//Ссылка на дочерний объект.
        child.transform.LookAt(shootPoint);//Направление стрельбы.
        bool mouseLeft = Input.mousePosition.x < Screen.width / 2;//Мышка в левой части экрана или вправой.
       
        */
        bool unitLeft = unit.transform.forward == new Vector3(1, 0, 0);//Юнит смотрит налево.
        bool mouseLeft = Input.mousePosition.x < Screen.width / 2;//Левая часть экрана.

        if (unit.inventory.groups[groupId].items[itemPos].childrenGO != null)//Если есть дочерний объект
        {
            unit.inventory.groups[groupId].items[itemPos].childrenGO.GetComponent<ItemChildren_Weapon>().Fire(damage, !(unitLeft == mouseLeft), shotSounds[Random.Range(0, shotSounds.Count - 1)]);//Выстрел.
        }
    }
}
