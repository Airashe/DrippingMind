using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Starfish : Source_AI_Logic
{
    public StarFishState state = StarFishState.Standing;//Состояние звезды.
    public Object_Unit unit;//Юнит принадлежащий игроку.
    public Object_Unit self;//Собственно звезда.
    public float watchDistantion;//На каком расстоянии звезда видит игрока.
    public Vector3 destPoint;//Точка назначения.
    public float stopDist;//Точка остановки.
    public int damage;//Урон.
    public bool scaredMode;//Напугана ли звезда.
    public string idleAnimation;//Анимация пердения.
    public string movingAnimation;//Анимация движения.
    public string scaredAnimation;//Анимация напугана.
    public string attackAnimation;//Анимация атаки.
    public string eatAnimation;//Анимация поедания.
    public string deathAnimation;//Анимация смерти.
    public string hideAnimation;//Анимация скрытия.
    public int[] callEventsOnHide;
    public int[] callEventsOnEat;//Когда начали жрать.
    public int[] callEventsOnEatCancel;//Когда первали жрать.

    public Source_ItemBase eatItem;//Предмет который надо схавать.
    public GameObject itemToEat;
    public List<AudioClip> attackSounds;

    public float damageCoolDown = 4;
    private float damageTimer = 0;

    private void OnDisable()
    {
        state = StarFishState.Standing;//Сбрасываем состояние.
        scaredMode = false;//Звезда-пезда не напугана.
    }

    public override void AI_Logic()
    {
        base.AI_Logic();
        if(unit != null && self != null)
        {
            if (self.currentHealth > 0)//Если есть жизни.
            {
                if (damageTimer > 0)//Если таймер не истек.
                {
                    damageTimer -= Time.deltaTime;//Отнимаем время.
                }
                navMeshAgent.enabled = true;//Включаем просчеты пути.
                if (itemToEat == null)//Если не нашли мясо.
                {
                    GameObject[] items = GameObject.FindGameObjectsWithTag("Object_Item");//Все предметы на карте.
                    foreach (GameObject item in items)
                    {
                        if (item.GetComponent<Object_Item>() != null)
                        {
                            if (item.GetComponent<Object_Item>().itemData == eatItem)//Если это еда.
                            {
                                itemToEat = item;
                            }
                        }
                    }
                    if (!scaredMode)//Если не напугана.
                    {
                        if (state == StarFishState.Standing || state == StarFishState.WalkTorawrdsDest)//Если мы стоим или идем к цели.
                        {
                            if (Vector3.Distance(unit.position, self.position) < watchDistantion)//Если юнит игрока в пределах радиуса.
                            {
                                if (Vector3.Distance(self.position, unit.position) > stopDist)//Если еще не дошли.
                                {
                                    destPoint = unit.position;//Цель движения - позиция юнита.
                                    state = StarFishState.WalkTorawrdsDest;//Логика - идти к цели.
                                }
                                else//Если дошли до игрока.
                                {
                                    if(unit.state != Property_UnitState.Dead)
                                    {
                                        state = StarFishState.Attack;//Атакуем.
                                    }
                                    else
                                    {
                                        itemToEat = unit.gameObject;
                                    }
                                }
                            }
                            else//Если игрок не в зоне видимости.
                            {
                                state = StarFishState.Standing;//Стоим пердим.
                            }
                        }
                    }
                    else//Если мы напуганы.
                    {
                        if (state == StarFishState.WalkTorawrdsDest)//Если двигаемся к цели.
                        {
                            if (Vector3.Distance(self.position, destPoint) < 0.5f)
                            {
                                state = StarFishState.Hide;//Стоим пердим.
                            }
                            else
                            {
                                destPoint = ClosestWindow();
                                state = StarFishState.WalkTorawrdsDest;//Идем к окну.
                            }
                        }
                    }
                }
                else//Если нашли мясо.
                {
                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(callEventsOnEat);//Вызываем ивенты, после того как начали жрать еду.
                    if (Vector3.Distance(self.position, itemToEat.transform.position) >= 0.32f)//Если не дошли до мяса.
                    {
                        destPoint = itemToEat.transform.position;//Цель движения - позиция юнита.
                        state = StarFishState.WalkTorawrdsDest;//Логика - идти к цели.
                    }
                    else
                    {
                            
                            destPoint = itemToEat.transform.position;//Цель движения - позиция юнита.
                            state = StarFishState.Eat;//Логика - есть пердеть.
                    }
                }
                StateResult();
            }
            else
            {
                state = StarFishState.Dead;
                self.model.SetAnimation(deathAnimation);
                navMeshAgent.enabled = false;
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
        else
        {
            unit = playerData.unit;//Ищем юнита игрока.
            self = transform.GetComponent<Object_Unit>();//Ищем себя.
        }
    }

    public Vector3 ClosestWindow()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Object_Window");//Поиск всех объектов с тегом окно.
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in gos)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin.transform.position;//Возвращаем позицию ближайшего окна.
    }

    public void StateResult()
    {
        if (state == StarFishState.Standing)//Если стоим на месте.
        {
            self.state = Property_UnitState.Idle;
            if (navMeshAgent.enabled)
            {
                navMeshAgent.isStopped = true;//Остнавливаем длижение.
            }
            self.model.SetAnimation(idleAnimation);//Анимация пердения.
        }
        if (state == StarFishState.Hide)
        {
            if (navMeshAgent.enabled)
            {
                navMeshAgent.isStopped = true;
            }
            self.model.SetAnimation(hideAnimation);//Скрытия.
            navMeshAgent.enabled = false;
            self.transform.rotation = Quaternion.Euler(0, 145, 0);
            self.transform.Translate(Vector3.forward*Time.deltaTime*1.2f);
        }
        if(state == StarFishState.Eat)//Если едим.
        {
            self.state = Property_UnitState.Eat;
            if (itemToEat != null)
            {
                navMeshAgent.isStopped = true;//Остнавливаем длижение.
                self.model.SetAnimation(eatAnimation);//Анимация пердения.
            }
            else
            {
                scaredMode = false;
                if(self.position.y >= 1.79f && self.position.y <= 3.2f)
                {
                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(callEventsOnEatCancel);
                    state = StarFishState.Standing;
                }
            }
        }
        if(state == StarFishState.WalkTorawrdsDest)//Если двигаемся к цели.
        {
            self.state = Property_UnitState.Walking;
            navMeshAgent.SetDestination(destPoint);//Движение к юниту.
            navMeshAgent.isStopped = false;//Позволяем движение.
            self.model.SetAnimation(movingAnimation);//Анимация движения.
        }
        if(state == StarFishState.Scared)//Если напуганы.
        {
            navMeshAgent.isStopped = true;//Остнавливаем движение.
            self.model.SetAnimation(scaredAnimation);//Анимация пугания.
            if(self.model.isCurrentMainAnimEnded)//Если анимация закончена.
            {
                destPoint = ClosestWindow();//Цель - ближайшее окно.
                state = StarFishState.WalkTorawrdsDest;//Идти к цели.
            }
        }
        if(state == StarFishState.Attack)
        {
            self.state = Property_UnitState.Attack;
            if (damageTimer <= 0)//Если можем таковать.
            {
                navMeshAgent.isStopped = true;//Остнавливаем движение.
                self.model.SetAnimation(attackAnimation);//Анимация пугания.
                if (self.model.isCurrentMainAnimEnded)//Если анимация закончена.
                {
                    unit.GetDamage(damage);//Наносим урон, после последнего кадра.
                    state = StarFishState.Standing;//Стоять пердеть.
                    Debug.Log(damageTimer + " before");
                    damageTimer = damageCoolDown;//Устанавливаем таймер заново.
                    Debug.Log(damageTimer + " after");
                }
                else
                {
                    if(!self.stepsSoundMaker.source.isPlaying)
                    {
                        self.stepsSoundMaker.source.clip = attackSounds[Random.Range(0, attackSounds.Count - 1)];
                        self.stepsSoundMaker.source.Play();
                    }
                }
            }
            else//Если не можем атаковать.
            {
                self.model.SetAnimation(idleAnimation);//Анимация пугания.
            }
        }
    }

    public void Self_UponTakingDamage()//При получении урона.
    {
        state = StarFishState.Scared;//Напугана.
        scaredMode = true;
    }
}
public enum StarFishState { Standing, WalkTorawrdsDest, Scared, Attack, Eat, Dead, Hide}