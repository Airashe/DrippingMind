using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Meduse : Source_AI_Logic
{
    public float teleportDistantion;//Дистанция до телепортации.
    public int[] onCloseEvents;//События, когда медуза добарлась до игрока.
    public Object_Event[] onCloseEventsObjs;//События, когда медуза добралась до игрока.

    private Object_Unit self;//Ссылка на себя.
    public List<AudioClip> idleSounds;//
    public List<AudioClip> electricSounds;
    public float idleVolume = 0.3f;
    public float electricVolume = 0.3f;
    public bool calledTeleportation = false;

    private void Start()
    {
        self = gameObject.GetComponent<Object_Unit>();//Ссылка на объекта юнита.
    }

    public override void AI_Logic()
    {
        if(!self.stepsSoundMaker.source.isPlaying)
        {
            self.stepsSoundMaker.source.clip = idleSounds[Random.Range(0, idleSounds.Count - 1)];
            self.stepsSoundMaker.volumeBorder = idleVolume;
            self.stepsSoundMaker.source.Play();
        }
        if(!self.damageSoundMaker.source.isPlaying)
        {
            self.damageSoundMaker.source.clip = electricSounds[Random.Range(0, electricSounds.Count - 1)];
            self.damageSoundMaker.volumeBorder = electricVolume;
            self.damageSoundMaker.source.Play();
        }
        if(Vector3.Distance(transform.position, playerData.unit.transform.position) > teleportDistantion)//Если медуза не дошла.
        {
            navMeshAgent.enabled = true;//Включаем AI передвижения.
            navMeshAgent.SetDestination(playerData.unit.transform.position);//Устанавливаем точку движения - позиция игрока.
        }
        else
        {
            navMeshAgent.enabled = false;//Отключаем AI передвижения.
            if(!calledTeleportation)
            {
                if(onCloseEvents.Length > 0)
                {
                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(onCloseEvents);//Выполняем ивенты по приблежению к игроку.
                }
                if(onCloseEventsObjs.Length > 0)
                {
                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(onCloseEventsObjs);//Выполняем ивенты по приблежению к игроку.
                }
                calledTeleportation = true;
            }
            
        }
    }
}
