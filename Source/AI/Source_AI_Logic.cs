using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Source_AI_Logic : MonoBehaviour
{
    [Header("Main AI data")]
    public Object_Player playerData;//Данные игрока.
    public NavMeshAgent navMeshAgent;//Агент передвижения.

    private void Start()
    {
        playerData = Camera.main.GetComponent<Object_Player>();//Получаем ссылку на данные игрока.
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();//Получаем ссылку на управление передвижением.
    }

    private void Update()
    {
        if(playerData != null)
        {
            if (playerData.unit != null && navMeshAgent != null)//Если есть юнит который принадлежит игроку.
            {
                AI_Logic();//Выполняем логику.
            }
        }
        else
        {
            playerData = Camera.main.GetComponent<Object_Player>();//Получаем ссылку на данные игрока.
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();//Получаем ссылку на управление передвижением.
        }
    }

    public virtual void AI_Logic()
    {

    }

    public virtual void AI_GetDamage(int damage)
    {
        
    }
}
