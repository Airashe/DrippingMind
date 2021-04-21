using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Dripping Mind/Предметы/Пустой предмет")]
public class Source_ItemBase : ScriptableObject
{
    public new string name;//Название предмета.
    public int weight;//Сколько слотов инвентаря занимает предмет.
    public Texture2D icon;//Иконка предмета.
    public string description;//Описание предмета.
    public Transform prefab;//Префаб предмета.
    public float useTime;//Время для использования предмета.
    public bool donRemove;//Нужно ли удалять предмет, после окончания стаков.
    public Transform childPrefab;//Префаб дочернего объекта.
    public Source_Model animationSetInHand;//Набор анимаций с предметом в руке.
    public Source_Model animationUse;//Набор анимаций использования предмета.
    public List<Source_Model> alterAnimationList;//Список альтернативных анимаций.
    public Source_ItemBase neededItem;//Если нужен предмет для использования.

    public virtual void UseEffect(Object_Unit unit, int groupId, int itemPos)//Какое действие будет совершаться при использовании предмета.
    {
        Debug.Log(name + " использован юнитом " + unit.transform.name + " находясь в группе " + groupId);
    }
}
