using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PI_Map : MonoBehaviour
{
    private Object_Player playerData;//Данные игрока.
    private void Start()
    {
        playerData = Camera.main.GetComponent<Object_Player>();//Получаем ссылку на объект игрока.
    }
    public void MapGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), playerData.mapTexture);//Рисуем текстуру карты.
    }
}
