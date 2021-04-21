using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Source_UnitInventory
{
    public List<Source_InventoryGroup> groups;//Группировки слотов.

    public Source_UnitInventory(Source_UnitInventory based)
    {
        this.groups = based.groups;
    }
}
