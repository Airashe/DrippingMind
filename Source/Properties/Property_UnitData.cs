using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Property_UnitData
{
    public int unitUniqueID;//Уникальный id юнита.
    public Object_Unit data;//Ссылка на объект юнита.
    public int stageId;//На каком этапе находится игрок с этим юнитом.
    public int root;//РУТЫ С ЭТИМ ПЕРСОНАЖЕМ.

    public Property_UnitData(int unitId, Object_Unit data, int stageId, int root)
    {
        this.unitUniqueID = unitId;
        this.data = data;
        this.stageId = stageId;
        this.root = root;
    }
}
