using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Item : MonoBehaviour
{
    public Source_ItemBase itemData;//Данные предмета.
    public int stacks;
    public int state;
    public int[] pickUpEvents;
    public Object_Event[] pickUpEventsObjs;

    public Source_InventoryItem Data
    {
        get
        {
            return new Source_InventoryItem(itemData, stacks, state, null);
        }
        set
        {
            itemData = value.data;
            stacks = value.stacks;
            state = value.state;
        }
    }

    public void Start()
    {
        transform.Find("Object_Model").GetComponent<Object_Model>().SetAnimation("Idle");//Устанавливаем стандартную анимацию.
    }
}
    