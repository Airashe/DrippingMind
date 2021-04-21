using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Starfish_v2 : Source_AI_Logic
{
    [Header("Starfish AI data")]
    public StarFishState_v2 state = StarFishState_v2.Idle;//Текущая задача звезды.
    public Object_Unit Self//Данные самой звезды.
    {
        get
        {
            return gameObject.GetComponent<Object_Unit>();//Возвращаем скрипт юнита, прикрепленный к объекту.
        }
    }
    public Object_Unit Target//Текущая цель звезды.
    {
        get
        {
            Object_Player playerData = Camera.main.GetComponent<Object_Player>();//Данные игрока.
            if(playerData != null)//Если есть данные игрока.
            {
                return Camera.main.GetComponent<Object_Player>().unit;//Возвращаем текущий юнит под контролем игрока.
            }
            return null;//Если данных нет, возвращаем пустоту.
        }
    }
    public bool scaredMode;//Напугана ли звезда.

    [Header("Starfish unique unit data")]
    public float healthRegenSpeed = 1;//Скорость восстановления по умолчанию.

    [Header("Starfish target data")]
    public Vector3 destanationPoint;//Точка, к которой должна дойти звезда.

    [Header("Starfish radius settings")]
    public float detectionRadius;//Радиус обнаружения юнита игрока.
    public float attackRadius;//Радиус атаки юнита.
    public float windowRadius;//Радиус остановки у окна.
    public float eatRadius;//Радиус поедания еды.

    [Header("Eat settings")]
    public Source_ItemBase food;//Еда, которую будет искать звезда.
    public float upPositionFix;//На сколько сместить нужную позицию еды вверх.
    private GameObject foodGO;//Игровой объект еды.

    [Header("Attack settings")]
    public float attackDelay;//Задержка между атаками.
    public int damage;//Урон, который наноситься за удар.
    private float attackDelayTimer;//Таймер расчитывающий время между атаками.

    [Header("Floor settings")]
    public float topBorder;//Расстояние до верхней границы.
    public float bottomBorder;//Расстояние до нижней границы.
    public float TopBorder//Верхняя граница относительно юнита.
    {
        get
        {
            return Self.position.y + topBorder;//Возвращаем текущую позицию юнита + расстояние до верхней границы.
        }
    }
    public float BottomBorder//Нижняя граница относительно юнита.
    {
        get
        {
            return Self.position.y - bottomBorder;//Возвращаем текущую позицию юнита - расстояние до нижней границы.
        }
    }

    [Header("Calling events")]
    public Object_Event[] eatingEventsObjs;//Объекты ивентов, которые вызываются, когда звезда ест.
    public Object_Event[] notEatingEvents;//Вызываемые ивенты, когда не ест.
    public Object_Event[] onFirstHide;//События при первом съебывании в окно.

    private bool onFirstHideCalled = false;//Были ли вызваны события при первом исчезновении.

    public override void AI_Logic()
    {
        if(Target != null && Self != null)//Если есть ссылки на себя и на цель.
        {
            if (Self.currentHealth > 0)//Если у звезды есть здоровье.
            {
                navMeshAgent.speed = Self.movementSpeed;//Присваем скорость передвижения из данных своего юнита.
                if (attackDelayTimer > 0)//Если есть задержка между атаками.
                {
                    attackDelayTimer -= Time.deltaTime;//Отнимаем от таймера задержки атаки значения.
                }
                if (!scaredMode)//Если звезда не напугана.
                {
                    if (foodGO == null)//Если нет объекта еды.
                    {
                        if (notEatingEvents.Length > 0)//Если есть ивенты, которые вызываются когда не едим пищу.
                        {
                            GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(notEatingEvents);//Вызываем события при любом другом действии кроме еды.
                        }
                        foodGO = GetFoodObject();//Ищем еду независимо от того, чем занимаемся.
                        if (state == StarFishState_v2.WalkingToEat || state == StarFishState_v2.Eating)//Если звезда ест, пока нет объекта еды.
                        {
                            SetState(StarFishState_v2.Idle);//Текущая задача - ожидание.
                        }
                        if (state == StarFishState_v2.Idle || state == StarFishState_v2.WalkingWhileSeek)//Пока мы не обнаружили игрока и занимаемся своими делами.
                        {
                            if (Vector3.Distance(Target.position, Self.position) <= detectionRadius)//Если юнит игрока в радиусе обнаружения.
                            {
                                SetState(StarFishState_v2.WalkingToUnit);//Устанавливаем задачу, дойти до юнита.
                            }
                        }
                        if (state == StarFishState_v2.Appearence)//Если появляемся.
                        {
                            if (Self.model.isCurrentMainAnimEnded)//Если анимация завершилась.
                            {
                                SetState(StarFishState_v2.Idle);//Переводим звезду в режим ожидания.
                            }
                        }
                        if (state == StarFishState_v2.AwaitAppearence)//Если ожидаем появления.
                        {
                            if (Self.currentHealth < Self.MaxHealth)//Если здоровье звезды не восстановлено.
                            {
                                Self.currentHealth += Time.deltaTime * healthRegenSpeed;//Прибавляем здоровье звезде.
                            }
                            if (Self.currentHealth == Self.MaxHealth)//Если здоровье звезды восстановлено.
                            {
                                GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Object_Window");//Ищем все окна на сцене.
                                foreach (GameObject spawnPoint in spawnPoints)//Для каждой точки спавна на сцене.
                                {
                                    Self.position = spawnPoint.transform.position;//Перемещаем звезду проверяемую точку.
                                    if (TargetAndSelfAtSameLevel(Target.position))//Если звезда и цель на одном уровне.
                                    {
                                        SetState(StarFishState_v2.Appearence);//Переводим в режим поиска.
                                        break;//Отанавливаем цикл.
                                    }
                                }
                            }
                        }
                        if (state == StarFishState_v2.InWindow)//Если залезаем в окно.
                        {
                            if (Self.model.isCurrentMainAnimEnded)//Если анимация закончилась.
                            {
                                SetState(StarFishState_v2.AwaitAppearence);//Переводим в скрытый режим.
                            }
                        }
                        if (state == StarFishState_v2.WalkingToWindow)//Если двигаемся к окну.
                        {
                            if (TargetAndSelfAtSameLevel(Target.position))//Если юнит и звезда на одном этаже.
                            {
                                SetState(StarFishState_v2.WalkingToUnit);//Устанавливаем задачу, дойти до юнита.
                            }
                            else//Если юнит не на одном этаже со звездой.
                            {
                                if (Vector3.Distance(destanationPoint, Self.position) <= windowRadius)//Если дошли до окна.
                                {
                                    SetState(StarFishState_v2.InWindow);//Устанавливаем задачу, выйти в окно.
                                }
                            }
                        }
                        if (state == StarFishState_v2.WalkingToUnit)//Если двигаемся к юниту.
                        {
                            if (TargetAndSelfAtSameLevel(Target.position))//Если юнит и звезда на одном этаже.
                            {
                                if (Vector3.Distance(Target.position, Self.position) <= attackRadius)//Если дошли до юнита и можем атаковать.
                                {
                                    if (attackDelayTimer <= 0)//Если задержки нет.
                                    {
                                        SetState(StarFishState_v2.Attack);//Задача - атаковать.
                                    }
                                    else//Если задержка есть.
                                    {
                                        SetState(StarFishState_v2.Idle);//Задача - ожидать окончания задержки в режиме поиска игрока.
                                    }
                                }
                                else//Если не дошли до юнита и не можем атаковать.
                                {
                                    SetState(StarFishState_v2.WalkingToUnit);//Задача - идти в сторону юнита.
                                    destanationPoint = Target.position;//Цель похода - позиция юнита.
                                }
                            }
                            else//Если звезда и юнит не на одном этаже.
                            {
                                destanationPoint = ClosestWindowPosition();//Получаем позицию ближайшего окна.
                                SetState(StarFishState_v2.WalkingToWindow);//Двигаемся к окну.
                            }
                        }
                        if (state == StarFishState_v2.Attack)//Если в режиме атаки.
                        {
                            if (Vector3.Distance(Target.position, Self.position) <= attackRadius)//Если дистанция до юнита позволяет атаковать.
                            {
                                if (attackDelayTimer <= 0)//Если задержка между атаками прошла.
                                {
                                    if (Self.model.isCurrentMainAnimEnded)//Завершена ли анимация атаки.
                                    {
                                        Target.GetDamage(damage);//Наносим урон юниту.
                                        attackDelayTimer = attackDelay;//Устанавливаем задержку до следующей возможной атаки.
                                    }
                                }
                                else//Если задержка не прошла.
                                {
                                    state = StarFishState_v2.Idle;//Задача - перейти в режим ожидания.
                                }
                            }
                            else//Если мы не дошли до юнита и не можем атаковать.
                            {
                                SetState(StarFishState_v2.WalkingToUnit);//Начинаем передвижение к юниту.
                            }
                        }
                    }
                    else//Если еда найдена.
                    {
                        if (state == StarFishState_v2.Idle || state == StarFishState_v2.Attack || state == StarFishState_v2.WalkingToUnit || state == StarFishState_v2.WalkingWhileSeek)//Если что-то не связанное с едой.
                        {
                            destanationPoint = foodGO.transform.position + Vector3.up * upPositionFix;//Цель движения - позиция еды.
                            SetState(StarFishState_v2.WalkingToEat);//Переводим звезду в состояние движения к еде.
                        }
                        if (state == StarFishState_v2.WalkingToEat)//Если задача - двигаться к пище.
                        {
                            if (TargetAndSelfAtSameLevel(foodGO.transform.position))//Если еда и звезда на одном этаже.
                            {
                                if (Vector3.Distance(Self.position, foodGO.transform.position + Vector3.up * upPositionFix) <= eatRadius)//Если в радиусе поедания.
                                {
                                    SetState(StarFishState_v2.Eating);//Задача - съект пищу.
                                }
                                else//Если не в радиусе поедания.
                                {
                                    SetState(StarFishState_v2.WalkingToEat);//Задача - дойти до пищи.
                                    destanationPoint = foodGO.transform.position + Vector3.up * upPositionFix;//Цель движения - позиция еды.
                                }
                            }
                            else//Если звезда и еда на разных этажах.
                            {
                                destanationPoint = ClosestWindowPosition();//Получаем позицию ближайшего окна.
                                SetState(StarFishState_v2.WalkingToWindow);//Двигаемся к окну.
                            }
                        }
                        if (state == StarFishState_v2.WalkingToWindow)//Если двигаемся к окну.
                        {
                            if (TargetAndSelfAtSameLevel(foodGO.transform.position + Vector3.up * upPositionFix))//Если еда и звезда на одном этаже.
                            {
                                SetState(StarFishState_v2.WalkingToEat);//Устанавливаем задачу, дойти до еды.
                            }
                            else//Если еда не на одном этаже со звездой.
                            {
                                if (Vector3.Distance(destanationPoint, Self.position) <= windowRadius)//Если дошли до окна.
                                {
                                    SetState(StarFishState_v2.InWindow);//Устанавливаем задачу, выйти в окно.
                                }
                            }
                        }
                        if (state == StarFishState_v2.Appearence)//Если появляемся.
                        {
                            if (Self.model.isCurrentMainAnimEnded)//Если анимация завершилась.
                            {
                                SetState(StarFishState_v2.WalkingToEat);//Переводим звезду в режим идти к еде.
                            }
                        }
                        if (state == StarFishState_v2.AwaitAppearence)//Если ожидаем появления.
                        {
                            if (Self.currentHealth < Self.MaxHealth)//Если здоровье звезды не восстановлено.
                            {
                                Self.currentHealth += Time.deltaTime * healthRegenSpeed;//Прибавляем здоровье звезде.
                            }
                            if (Self.currentHealth == Self.MaxHealth)//Если здоровье звезды восстановлено.
                            {
                                GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Object_Window");//Ищем все окна на сцене.
                                foreach (GameObject spawnPoint in spawnPoints)//Для каждой точки спавна на сцене.
                                {
                                    Self.position = spawnPoint.transform.position;//Перемещаем звезду проверяемую точку.
                                    if (TargetAndSelfAtSameLevel(foodGO.transform.position + Vector3.up * upPositionFix))//Если звезда и еда на одном уровне.
                                    {
                                        SetState(StarFishState_v2.Appearence);//Переводим в режим поиска.
                                        break;//Отанавливаем цикл.
                                    }
                                }
                            }
                        }
                        if (state == StarFishState_v2.InWindow)//Если залезаем в окно.
                        {
                            if (Self.model.isCurrentMainAnimEnded)//Если анимация закончилась.
                            {
                                SetState(StarFishState_v2.AwaitAppearence);//Переводим в скрытый режим.
                            }
                        }
                        if (state == StarFishState_v2.Eating)//Если задача - съесть пищу.
                        {
                            if (Vector3.Distance(Self.position, foodGO.transform.position + Vector3.up * 0.5f) > eatRadius)//Если не в радиусе поедания.
                            {
                                SetState(StarFishState_v2.WalkingToEat);//Задача - идти к пище.
                            }
                            else//Если мы едим еду.
                            {
                                if (eatingEventsObjs.Length > 0)//Если есть ивенты, которые вызываются при поедании пищи.
                                {
                                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(eatingEventsObjs);//Вызываем события при поедании пищи.
                                }
                            }
                        }
                    }
                }
                else//Если она напугана.
                {
                    if (state == StarFishState_v2.Scared)//Если напуганы.
                    {
                        if (Self.model.isCurrentMainAnimEnded)//Если последний кадр анимации.
                        {
                            destanationPoint = ClosestWindowPosition();//Цель похода - ближайшее окно.
                            SetState(StarFishState_v2.WalkingToWindow);//Двигаемся к окну.
                        }
                    }
                    if (state == StarFishState_v2.Appearence)//Если появляемся.
                    {
                        if (Self.model.isCurrentMainAnimEnded)//Если анимация завершилась.
                        {
                            SetState(StarFishState_v2.Idle);//Переводим звезду в режим ожидания.
                        }
                    }
                    if (state == StarFishState_v2.AwaitAppearence)//Если ожидаем появления.
                    {
                        if (Self.currentHealth < Self.MaxHealth)//Если здоровье звезды не восстановлено.
                        {
                            Self.currentHealth += Time.deltaTime * healthRegenSpeed;//Прибавляем здоровье звезде.
                        }
                        if (Self.currentHealth == Self.MaxHealth)//Если здоровье звезды восстановлено.
                        {
                            scaredMode = false;//Звезда больше не боиться.
                            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Object_Window");//Ищем все окна на сцене.
                            foreach (GameObject spawnPoint in spawnPoints)//Для каждой точки спавна на сцене.
                            {
                                Self.position = spawnPoint.transform.position;//Перемещаем звезду проверяемую точку.
                                if (TargetAndSelfAtSameLevel(Target.position))//Если звезда и цель на одном уровне.
                                {
                                    SetState(StarFishState_v2.Appearence);//Переводим в режим поиска.
                                    break;//Отанавливаем цикл.
                                }
                            }
                        }
                    }
                    if (state == StarFishState_v2.InWindow)//Если залезаем в окно.
                    {
                        if (Self.model.isCurrentMainAnimEnded)//Если анимация закончилась.
                        {
                            SetState(StarFishState_v2.AwaitAppearence);//Переводим в скрытый режим.
                            if (!onFirstHideCalled)
                            {
                                if (onFirstHide.Length > 0)
                                {
                                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(onFirstHide);
                                    onFirstHideCalled = true;
                                }
                            }
                        }
                    }
                    if (state == StarFishState_v2.WalkingToWindow)//Если двигаемся к окну.
                    {
                        if (Vector3.Distance(destanationPoint, Self.position) <= windowRadius)//Если дошли до окна.
                        {
                            SetState(StarFishState_v2.InWindow);//Устанавливаем задачу, выйти в окно.
                        }
                    }
                }
            }
            else
            {
                navMeshAgent.enabled = false;
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                Self.enabled = false;
                this.enabled = false;
            }
        }
    }

    public override void AI_GetDamage(int damage)//При получении урона.
    {
        base.AI_GetDamage(damage);
        SetState(StarFishState_v2.Scared);//Устанавливаем режим боязни.
    }

    public GameObject GetFoodObject()//Получаем позицию еды.
    {
        GameObject[] itemsAtScene = GameObject.FindGameObjectsWithTag("Object_Item");//Получаем список всех предметов на карте.
        foreach(GameObject item in itemsAtScene)//Для каждого предмета на сцене.
        {
            if(item.GetComponent<Object_Item>().itemData == food)//Если предмет можно съесть.
            {
                return item;//Возвращаем объект еды.
            }
        }
        return null;//Возвращаем пустоту.
    }

    public bool TargetAndSelfAtSameLevel(Vector3 targetPosition)//Находиться ли цель там же, где и звезда.
    {
        Debug.Log("Target: " + targetPosition.y + "<=" + TopBorder + " and " + targetPosition.y + ">=" + BottomBorder);//Проверка пройдена.
        return (targetPosition.y <= TopBorder && targetPosition.y >= BottomBorder);//Возвращаем результат проверки.
    }

    public Vector3 ClosestWindowPosition()//Позиция ближайшего окна.
    {
        GameObject[] windowns = GameObject.FindGameObjectsWithTag("Object_Window");//Получаем список всех окон на сцене.
        List<GameObject> windownsOnSameFloor = new List<GameObject>();//Список окон на одном этаже с звездой.
        foreach (GameObject window in windowns)//Для каждого окна в списоке окон.
        {
            if (window.transform.position.y <= TopBorder && window.transform.position.y >= BottomBorder)//Если окно находиться на том же этаже, что и юнит.
            {
                Debug.Log(window.name + ": " + window.transform.position.y + "<=" + TopBorder + " and " + window.transform.position.y + ">=" + BottomBorder);//Проверка пройдена.
                windownsOnSameFloor.Add(window);//Добавляем окно, что на том же этаже, что и звезда.
            }
            continue;
        }
        GameObject closestWindow = null;//Переменная ближайшего окна.
        foreach(GameObject window in windownsOnSameFloor)//Для каждого окна в списоке окон.
        {
            if(closestWindow != null)//Если есть с чем сравнивать.
            {
                if (Vector3.Distance(Self.position, window.transform.position) <= Vector3.Distance(Self.position, closestWindow.transform.position))//Если расстояние до текущего окна, меньше чем до ближайшего окна.
                {
                    closestWindow = window;//Ближайшее окно - текущее окно.
                }
            }
            else//Если нет.
            {
                closestWindow = window;//Устанавливаем первое окно, как ближайшее.
            }
        }
        if(closestWindow != null)//Если есть ближайшее окно.
        {
            Debug.Log(closestWindow.name);//Дебаг ближайшего окна.
            return closestWindow.transform.position;//Возвращаем позицию текущего окна.
        }
        return Vector3.zero;//Возвращаем пустоту в случаи неудачи.
    }

    public bool SetState(StarFishState_v2 newState)//Установка новой задачи для звезды.
    {
        state = newState;//Устанавливаем новую задачу.

        switch(newState)//Для каждого значения новой задачи.
        {
            case StarFishState_v2.Idle://Если задача - стоять на месте.
                Self.SetState(Property_UnitState.Idle);//Состояние юнита - стоять на месте.
                Self.Visible = true;
                navMeshAgent.enabled = false;//Отключаем скрипт передвижения.
                break;
            case StarFishState_v2.WalkingToUnit://Если задача идти к юниту.
                Self.SetState(Property_UnitState.Walking);//Состояние юнита - движение.
                Self.Visible = true;
                navMeshAgent.enabled = true;//Включаем скрипт передвижения.
                navMeshAgent.SetDestination(destanationPoint);//Устанавливаем точку, в сторону которой нужно двигаться.
                break;
            case StarFishState_v2.WalkingToWindow://Если задача идти к окну.
                Self.SetState(Property_UnitState.Walking);//Состояние юнита - движение.
                Self.Visible = true;
                navMeshAgent.enabled = true;//Включаем скрипт передвижения.
                navMeshAgent.SetDestination(destanationPoint);//Устанавливаем точку, в сторону которой нужно двигаться.
                break;
            case StarFishState_v2.Attack://Если задача атаковать.
                Self.SetState(Property_UnitState.Attack);//Состояние юнита - атака врага.
                Self.Visible = true;
                navMeshAgent.enabled = false;//Отключаем скрипт передвижения.
                break;
            case StarFishState_v2.Scared://Если звезду напугали.
                Self.SetState(Property_UnitState.OnGetDamage);//Устанавливаем состояние юнита, на получение урона.
                navMeshAgent.enabled = false;//Отключаем скрипт передвижения.
                Self.Visible = true;
                scaredMode = true;//Включаем поведение когда напуганы.
                break;
            case StarFishState_v2.InWindow://Если задача выйти в окно.
                Self.SetState(Property_UnitState.OnInWindow);//Переводим юнита в состояния - выход в окно.
                Self.Visible = true;
                navMeshAgent.enabled = false;//Отключаем скрипт передвижения.
                break;
            case StarFishState_v2.AwaitAppearence://Если задача - ожидать выхода из окна.
                Self.Visible = false;//Скрываем модель звезды.
                navMeshAgent.enabled = false;//Отключаем скрипт передвижения.
                break;
            case StarFishState_v2.Appearence://Если задача - выйти из окна.
                Self.SetState(Property_UnitState.OnOutWindow);//Переводим юнита в состояния - выход из окна.
                Self.Visible = false;//Скрываем модель звезды.
                navMeshAgent.enabled = false;//Отключаем скрипт передвижения.
                break;
            case StarFishState_v2.WalkingToEat://Если двигаемся к еде.
                Self.SetState(Property_UnitState.Walking);//Переводим юнита в состояние движения.
                Self.Visible = true;//Показываем модель.
                navMeshAgent.enabled = true;//Включаем скрипт передвижения.
                navMeshAgent.SetDestination(destanationPoint);//Устанавливаем точку, в сторону которой нужно двигаться.
                break;
            case StarFishState_v2.Eating://Если поедаем пищу.
                Self.SetState(Property_UnitState.Eat);//Переводим юнита в режим еды.
                Self.Visible = true;
                navMeshAgent.enabled = false;//Отключаем скрипт передвижения.
                break;
        }

        return true;//Возвращаем, что задача была установлена.
    }
}

public enum StarFishState_v2 { Idle, WalkingWhileSeek, WalkingToUnit, WalkingToEat, Attack, Scared, WalkingToWindow, InWindow, AwaitAppearence, Appearence, Eating}
