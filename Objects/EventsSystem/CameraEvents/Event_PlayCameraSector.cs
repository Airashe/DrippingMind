using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_PlayCameraSector : Object_Event
{
    public int sectorId = -1;//Сектор который будет проигрываться.
    public override void Initialize()
    {
        base.Initialize();
        GameObject.FindGameObjectWithTag("CameraManager").GetComponent<Source_CameraManager>().PlaySector(sectorId);//Приказываем менеджеру камер проиграть сектор.
    }
}
