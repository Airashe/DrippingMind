using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Source_InventoryItem
{
    public Source_ItemBase data;//Данные предмета.
    public int stacks;//Количество оставшихся использований.
    public int state;//В каком состоянии предмет.
    public GameObject childrenGO;//Игровой объект в подчинении предмета.

    public Source_InventoryItem (Source_ItemBase data, int leftStacks, int state, GameObject childrenGO)
    {
        this.data = data;
        this.stacks = leftStacks;
        this.state = state;
        this.childrenGO = childrenGO;
    }
    public Source_InventoryItem(Source_InventoryItem parentData)
    {
        this.data = parentData.data;
        this.stacks = parentData.stacks;
        this.state = parentData.state;
        this.childrenGO = parentData.childrenGO;
    }
}
