using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Event : MonoBehaviour
{
    public int uniqueId;//Уникальный id ивента.
    public string debugString;

    public virtual void Initialize()//Выполнение кода ивента.
    {
        if (debugString.Length > 0)
        {
            Debug.Log(uniqueId + " говорит: " + debugString);//Сообщаем, что событие было вызвано.
        }
    }
}
