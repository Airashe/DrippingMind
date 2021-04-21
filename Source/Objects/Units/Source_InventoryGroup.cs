using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Source_InventoryGroup//Класс группы инвентаря.
{
    public string name;//Название группы.
    public Vector3 scenePosition;//Позиция группы на сцене, относительно модельки игрока.
    public List<Source_InventoryItem> items;//Список предметов в инвентаре.
    public int maxWeight;//Максимальный вмещаемый вес.
    public string hotkeyName;//Имя горячей клавиши.
    //public GameObject groupChildrenObj;//Дочерний объект этой группы.
    public int CurrentWeigth//Текущий вес.
    {
        get
        {
            int resultWeight = 0;//Результативный вес.
            foreach (Source_InventoryItem item in items)//Для каждого предмета в списке.
            {
                if (item != null)
                {
                    resultWeight += item.data.weight;//Прибавляем вес.
                }
            }
            return resultWeight;//Возвращаем результативный вес.
        }
    }
    public int LeftWeight//Оставшееся место.
    {
        get
        {
            return maxWeight - CurrentWeigth;//Возвращаем разницу между максимальным и занятым весом.
        }
    }
}
