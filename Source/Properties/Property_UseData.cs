using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Property_UseData
{
    public GameObject gameObject;//Элемент который будет использоваться.
    public UseData_ObjType type;//Тип элемента.
    public Vector3 position
    {
        get
        {
            if(gameObject != null)
            {
                return gameObject.transform.position;
            }
            return Vector3.zero;
        }
    }

    public Property_UseData(GameObject go, UseData_ObjType type)
    {
        this.gameObject = go;
        this.type = type;
    }

}

public enum UseData_ObjType { Chest, Trigger, Unit, Item, Teleport, Door}