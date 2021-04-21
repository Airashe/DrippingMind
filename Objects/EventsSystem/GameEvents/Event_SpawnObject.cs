using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_SpawnObject : Object_Event
{
    public string spawnName;//Имя объекта.
    public Transform spawnObject;//Объект, который будет создан.

    public override void Initialize()
    {
        base.Initialize();
        if (spawnObject != null)
        {
            GameObject newObject = Instantiate(spawnObject, transform.position, transform.rotation).gameObject;//Создаем объект.
            newObject.name = spawnName;
            Source_GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Ссылка на менеджер игры.
            gameManager.AddUnitData(newObject);
        }
        /*if(newObject.tag == "Object_AI")
        {
           
        }*/
    }
}
