using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_DoorConnecter : MonoBehaviour
{
    public Object_SoundMaker doorSource;//Источник звука дверей.
    public List<Object_Door> doors;//Список дверей.
    public Source_ItemBase needItemToOpen;//Нужный предмет для открытия.
    public bool totalyLocked;

    private void Start()
    {
        doorSource = transform.Find("Door_SoundMaker").GetComponent<Object_SoundMaker>();//Источник звука двери.
    }

    public bool Open
    {
        get
        {
            if(doors.Count > 0)
            {
                if(doors[0].currentTargetState == Door_TargetState.open)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }

    public void OnUse()
    {
        foreach (Object_Door door in doors)
        {
            door.SetTargetState(door.currentTargetState == Door_TargetState.open ? Door_TargetState.close : Door_TargetState.open);
        }
        doorSource.Play();
    }

    private void Update()
    {
        if(totalyLocked)
        {
            foreach(Object_Door door in doors)
            {
                door.SetTargetState(Door_TargetState.close);
            }
        }
    }
}
