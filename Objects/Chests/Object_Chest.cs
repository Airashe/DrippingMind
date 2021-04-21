using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Chest : MonoBehaviour
{
    public string chestName;//Название сундука.
    public Source_ChestGUI interfaceStyle;//Текстуры окошка сундука.
    public bool open;//Открыт ли сундук.
    public Source_ItemBase requestItem;//Предмет необходимый для открытия.
    public List<Source_InventoryItem> items;//Список предметов в сундуке.
    public int maxWeight;//Максимальный вмещаемый вес.
    public int[] eventsOnRemoveItem;//Когда из ящика взят любой предмет.
    public Object_Event[] eventsOnRemoveItemObj;
    public Source_UnitQuote closedQuote;
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

    public bool AddItem(Source_InventoryItem itemData)//Добавление предмета в инвентарь cундука.
    {
        if (itemData != null)//Если есть добавляемый предмет.
        {
            if (LeftWeight >= itemData.data.weight)//Если в группе достаточно места.
            {
                Source_InventoryItem itemToAdd = new Source_InventoryItem(itemData);//Создание копии предмета.
                items.Add(itemToAdd);//Добавляем предмет к группе.
                return true;//Возвращаем успешность.
            }
        }
        return false;
    }

    public bool RemoveItem(int itemPos)//Удаление предмета из инвентаря сундука.
    {
        GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(eventsOnRemoveItem);//Запускаем ивенты при попытке что-то взять.
        GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(eventsOnRemoveItemObj);//Запускаем ивенты при попытке что-то взять.
        if (itemPos >= 0 && itemPos < items.Count)
        {
            items.RemoveAt(itemPos);//Удаляем предмет из инвентаря.
            return true;
        }
        return false;
    }
}
