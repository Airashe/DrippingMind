using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ChageGameMode : Object_Event
{
    public GameMode mode;//Какой режим игры будет установлен.

    public override void Initialize()
    {
        base.Initialize();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>().gameMode = mode;
    }
}
