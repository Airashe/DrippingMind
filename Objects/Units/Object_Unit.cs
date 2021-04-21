using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Объект хранящие данные юнита.
public class Object_Unit : MonoBehaviour
{
    [Header("Unit main data")]
    public int uniqueID;//Уникальный id юнита.
    public string ingameName;//Имя юнита.
    public Property_UnitState state = Property_UnitState.Idle;//Состояние юнита.
    public Propety_UnitTeam team = Propety_UnitTeam.Unknown;//Команда юнита.
    [Header("Unit movement data")]
    public float movementSpeed = 1;//Скорость передвижения.

    [Header("Unit parameters")]
    public float sMaxHealth = 100;//Здоровье по умолчанию.
    public float currentHealth = 100;//Текущее здоровье.
    public float MaxHealth//Максимальное значение здоровья с учетом всех дебафов.
    {
        get
        {
            float resultHealth = sMaxHealth;//Максимальное количество здоровья.
            foreach (Source_Debuff debuff in debuffs)//Для каждого дебафа.
            {
                if (debuff != null)//Если дебафф существует.
                {
                    resultHealth -= debuff.loseHealth;//Отнимаем очки максимального здоровья.
                }
            }
            return resultHealth < 1 ? 1 : resultHealth;//Возвращаем реально значение макс здоровья.
        }
    }
    [Header("Inventory")]
    public Source_UnitInventory inventory;//Инвентарь юнита.

    public bool itemUse_active = false;//Используется ли предмет.
    
    public int itemUse_groupId = -1;//Id группы используемого предмета.
    
    public int itemUse_itemPos = -1;//Позиция используемого предмета.
    
    public float itemUse_timeLeft = 100;//Сколько времени до использования.
    [Header("Debuffs")]
    public List<Source_Debuff> debuffs;//Дебафы юнита.

    [Header("Model and animation settings")]
    public Source_Model modelData;//Модель юнита.
    public Object_Model model;//Модель юнита.
    public bool animationSpeedByHp;//Изменять скорость анимации в зависимости от хп.
    public float baseAnimationSpeed = 10;//Базовая скорость анимации.

    public Vector3 position//Позиция трансформа.
    {
        get
        {
            return transform.position;//Возвращаем позицию трансформа.
        }
        set
        {
            transform.position = value;//Устанавливаем значение позиции.
        }
    }
    public string startAnimationName;//Анимация юнита, при старте сцены.

    //Отображения юнита для игрока.

    public bool visible;//Виден ли юнит.

    public bool Visible
    {
        get
        {
            return visible;
        }
        set
        {
            if(model != null)//Если есть модель.
            {
                model.mainRenderer.gameObject.layer = value ? 0 : 10;//Скрываем или показываем основной рендер.
                model.secondRenderer.gameObject.layer = value ? 0 : 10;//Скрываем дополнительный рендер.
                visible = value;//Присваиваем значение.
            }
        }
    }

    [Header("Sounds settings")]
    public List<AudioClip> attackSounds;//Звуки атаки.
    public float stepDelay = 1;//Промеждуток между шагами.
    public List<AudioClip> deathSound;//Звуки смерти.
    public bool deathSoundsPlayed = false;//Был ли проигран звук.
    public List<AudioClip> eatSound;
    public List<AudioClip> onTakeDamage;//Звуки при получении урона.
    public List<Source_SoundCollection> stepSounds;//Звуки шагов, для разных поверхностей.
    private float stepTimer;//Таймер запроса звуков шагов.
    public AudioClip CurrentStepSound//Возвращает текущий звук, для проигрывания игроком.
    {
        get
        {
            if (state == Property_UnitState.Walking)//Если юнит двигается.
            {
                RaycastHit checkLayerHit;//Результат попадания.
                Ray checkRay = new Ray(transform.position, transform.up * -1);//Луч вниз юнита.
                if (Physics.Raycast(checkRay, out checkLayerHit))//Выпускаем луч проверки.
                {
                    foreach (Source_SoundCollection soundCollection in stepSounds)//Для каждой коллекции звуков, в списке коллекций.
                    {
                        if (soundCollection.layer == (soundCollection.layer | (1 << checkLayerHit.collider.gameObject.layer)))//Если слои объекта и коллекции совпадают.
                        {
                            return soundCollection.clips[Random.Range(0, soundCollection.clips.Count - 1)];//Возвращаем случайный звук из коллекции.
                        }
                    }
                }
            }
            return null;
        }
    }

    [Header("Sounds makers")]
    public Object_SoundMaker damageSoundMaker;//Звуки получения урона.
    public Object_SoundMaker stepsSoundMaker;//Звуки шагов.
    public Object_SoundMaker deathSoundMaker;//Звуки смерти.
    public Object_SoundMaker eatSoundMaker;//Звуки поедания.
    public Object_SoundMaker attackSoundMaker;//Звуки нанесения урона.

    [Header("Quotes ingame data")]
    //Переменные, для того, чтобы юнит говорил.
    public Source_UnitQuote say_quote;//Что говорит юнит.
    public float say_lineTimer = 0;//Сколько будет показана фраза.
    public float height;//Высота юнита.

    [Header("Events data")]
    public Object_Event[] uponTakingDamageEvents;//Ивенты при получении урона.

    private void Start()
    {
        damageSoundMaker = transform.Find("Damage_SoundMaker").GetComponent<Object_SoundMaker>();//Получаем ссылка на скрипт проигрывания звуков урона.
        stepsSoundMaker = transform.Find("Steps_SoundMaker").GetComponent<Object_SoundMaker>();//Получаем ссылку на скрипт проигрывания шагов.
        deathSoundMaker = transform.Find("Death_SoundMaker").GetComponent<Object_SoundMaker>();//Получаем ссылку на скрипт для проигрывания звуков смерти.
        eatSoundMaker = transform.Find("Eating_SoundsMaker").GetComponent<Object_SoundMaker>();//Получаем ссылку на скрипт для проигрывания звуков поедания.
        attackSoundMaker = transform.Find("Attack_SoundMaker").GetComponent<Object_SoundMaker>();//Получаем ссылку на скрипт для проигрывания звуков атаки.
        model = transform.Find("Object_Model").GetComponent<Object_Model>();//Получаем скрипт модели юнита.
        Debug.Log(gameObject.name + " model=" + model);
        if(model != null)//Если есть модель юнита.
        {
            model.SetModel(modelData, startAnimationName);//Задаем модель и анимацию по умолчанию.\
        }
        Visible = visible;//Обновляем значение.
    }

    private void Update()
    {
        if(state == Property_UnitState.Walking)//Если юнит передвигается.
        {
            if(stepsSoundMaker != null)//Если есть, проигрыватель.
            {
                if(!stepsSoundMaker.source.isPlaying)//Если звук не проигрывается
                {
                    if(stepTimer > 0)//Если есть задержка.
                    {
                        stepTimer -= Time.deltaTime;//Отнимаем вермя с прошлого кадра.
                    }
                    else//Если задержка прошла.
                    {
                        stepsSoundMaker.source.clip = CurrentStepSound;//Текущий звук шага.
                        if (stepsSoundMaker.source.clip != null)
                        {
                            stepsSoundMaker.source.Play();
                        }
                        stepTimer = stepDelay;
                    }
                }
            }
        }
        else//Если делаем что-то кроме ходьбы
        {
            stepTimer = 0;//Обнуляем шаг.
            if(state == Property_UnitState.Eat)
            {
                if (eatSoundMaker != null)
                {
                    if (!eatSoundMaker.source.isPlaying)
                    {
                        eatSoundMaker.source.clip = eatSound[Random.Range(0, eatSound.Count -1)];//Текущий звук поедания.
                        eatSoundMaker.source.Play();
                    }
                }
            }
        }
        Debug.DrawRay(transform.position, transform.forward, Color.cyan);
        if (say_quote != null)//Если есть фраза, которую нужно сказать.
        {
            if (say_lineTimer > 0)//Если есть время показа фразы юнита.
            {
                say_lineTimer -= Time.deltaTime;//Отнимаем таймер.
            }
            if (say_lineTimer <= 0)//Если таймер закончился.
            {
                Source_EventsManager eventsManager = GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>();//Получаем ссылку на менеджер событий.
                eventsManager.EngageEvent(say_quote.evenetsId);//Выполняем вызов всех ивентов.
                say_quote = null;//Обнуляем данные фразы.
                say_lineTimer = 0;//Обнуляем таймер.
            }
        }
        if(itemUse_active)//Если есть используемый предмет
        {
            if(state != Property_UnitState.Walking)//Если не персонаж двигается
            {
                if(itemUse_timeLeft > 0)//Если есть время использования.
                {
                    itemUse_timeLeft -= Time.deltaTime;//Отнимаем счетчик.
                }
                if(itemUse_timeLeft <= 0)//Если таймер закончился.
                {
                    UseItem_End(itemUse_groupId, itemUse_itemPos);//Вызываем использование предмета.
                }
            }
            else//Если двигается.
            {
                UseItem_Cancel(itemUse_groupId, itemUse_itemPos);//Перрываем использование предмета.
            }
        }
        if(model != null)//Если есть модель.
        {
            model.lookDirection = transform.forward;//Модель смотрит прямо от юнита(себя).
            model.parentRight = transform.right;//Правая сторона от модели равна правой стороне юнита.
            if(animationSpeedByHp)//Если скорость анимации зависит от хп.
            {
                float speedRate = 1 + ((1 - ((float)currentHealth / (float)MaxHealth))*1.1f);//Скорость дыхания.
                model.mainAnimationSpeed = baseAnimationSpeed*speedRate;//Умножаем стандартную скорость анимации на нужный.
            }
            else
            {
                model.mainAnimationSpeed = baseAnimationSpeed;//Устанавливаем стандартную скорость.
            }
        }
        StatsControll();//Контролирование параметров юнита.
    }

    private void StatsControll()
    {
        currentHealth = currentHealth < 0 ? 0 : currentHealth > MaxHealth ? MaxHealth : currentHealth;//Если здоровье за пределами.
        if(currentHealth == 0)//Если юнит мертв.
        {
            SetState(Property_UnitState.Dead);//Юнит мертв.
        }
    }

    public bool Kill()//Моментальное убийство юнита.
    {
        currentHealth = 0;//Устанавливаем уровень здоровья на 0.
        return true;
    }

    public bool GetDamage(int inputDamage)//Получение урона.
    {
        currentHealth -= inputDamage;//Входящий урон.
        if (gameObject.GetComponent<Source_AI_Logic>())//Если юнит под управлением AI.
        {
            gameObject.GetComponent<Source_AI_Logic>().AI_GetDamage(inputDamage);//Отправлем ии, что юнит получил урон.
        }
        damageSoundMaker.source.clip = onTakeDamage[Random.Range(0, onTakeDamage.Count - 1)];//Возвращаем случайный звук из коллекции.;//Текущий звук шага.
        if (damageSoundMaker.source.clip != null)
        {
            damageSoundMaker.source.Play();
        }
        if(uponTakingDamageEvents.Length > 0)
        {
            GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(uponTakingDamageEvents);//Выполнение событий при получении урона.
        }
        return true;
    }

    public bool Say(Source_UnitQuote quote)//Добавляем фразу юниту с временем для произнесения.
    {
        say_quote = quote;//Запоминаем фразу.
        say_lineTimer = quote.existTime;//Устанавливаем время на произнесение.
        return true;
    }

    public bool AddPhoby(Source_Debuff debuffData)//Добавление фобии к списочку.
    {
        bool canAdd = true;//Можно ли добавить фобию.
        foreach(Source_Debuff debuff in debuffs)//Для каждой фобии.
        {
            if(debuff.name == debuffData.name)//Если фобия уже в списке.
            {
                canAdd = false;//Нельзя добавить фобию.
            }
        }
        if(canAdd)//Если фобию можно добавить.
        {
            debuffs.Add(debuffData);//Добавляем фобию к списку.
        }
        return canAdd;//Добавляем результат.
    }

    public void UseItem_End(int groupId, int itemPos)//Завершение спользования предмета.
    {
        if(ChangeItemStacks(groupId, itemPos, -1))//Если стаков достаточно.
        {
            inventory.groups[groupId].items[itemPos].data.UseEffect(this, groupId, itemPos);//Вызываем функцию предмета.
            if (inventory.groups[groupId].items[itemPos].data.animationUse != null)//Если есть анимация использования.
            {
                model.SetModel(modelData, startAnimationName);//Меняем модель.
                animationSpeedByHp = true;
            }
            if (inventory.groups[groupId].items[itemPos].stacks == 0 && !inventory.groups[groupId].items[itemPos].data.donRemove)//Если стаки предмета закончились и предмет нужно удалить.
            {
                RemoveItem(groupId, itemPos);//Удаляем предмет из инвентаря.
            }
        }
        itemUse_active = false;//Предмет не используется.
        itemUse_groupId = -1;//Обнуляем группу предмета.
        itemUse_itemPos = -1;//Обнуляем позицию предмета.
    }

    public void UseItem_Cancel(int groupId, int itemPos)//Отпускание кнопки использования.
    {
        if(itemUse_active && itemUse_timeLeft > 0)//Если есть используемый предмет и он еще не использован.
        {
            if(groupId == itemUse_groupId && itemUse_itemPos == itemPos)//Если остановлено использование для этого предмета.
            {
                itemUse_active = false;//Предмет больше не используется.
                itemUse_groupId = -1;
                itemUse_itemPos = -1;
                itemUse_timeLeft = 100;//Устанавливаем таймер на 100 секунд.
                if (inventory.groups[groupId].items[itemPos].data.animationUse != null)//Если есть анимация использования.
                {
                    model.SetModel(modelData, startAnimationName);//Меняем модель.
                    animationSpeedByHp = true;
                }
            }
        }
    }

    public void UseItem_Start(int groupId, int itemPos)//Начало использования предмета - нажатие кнопки.
    {
        if(inventory.groups[groupId].items[itemPos].data.useTime > 0)//Если предмет используется не моментально.
        {
            if (inventory.groups[groupId].items[itemPos].stacks > 0)//Если есть что использовать.
            {
                if (inventory.groups[groupId].items[itemPos].data.neededItem == null || new Vector2Int(-1, -1) != this.HaveItem(inventory.groups[groupId].items[itemPos].data.neededItem))
                {
                    itemUse_active = true;//Устанавливаем, что юнит использует предмет.
                    itemUse_groupId = groupId;
                    itemUse_itemPos = itemPos;
                    itemUse_timeLeft = inventory.groups[groupId].items[itemPos].data.useTime;//Запоминаем время использования.
                    if (inventory.groups[groupId].items[itemPos].data.animationUse != null)//Если есть анимация использования.
                    {

                        Debug.Log(this.HaveItem(inventory.groups[groupId].items[itemPos].data.neededItem));

                        Debug.Log(inventory.groups[groupId].items[itemPos].data.neededItem != null);

                        model.SetModel(inventory.groups[groupId].items[itemPos].data.animationUse, startAnimationName);//Меняем модель.
                        animationSpeedByHp = false;

                    }
                }
            }
        }
        else//Если предмет используется моментально.
        {
            UseItem_End(groupId, itemPos);//Вызываем завершение использвания.
        }
    }

    public void SetState(Property_UnitState newState)
    {
        state = newState;//Устанавливаем новое состояние юнита.
        switch (state)
        {
            case Property_UnitState.Idle://Для состояния покоя.
                                         model.SetAnimation("Idle0");//Устанавливаем анимацию покоя.
                attackSoundMaker.source.clip = null;//Обнуляем клип атаки, чтобы когда вернулись в атакующий режим, клип проигрался снова 1 раз.
                break;
            case Property_UnitState.Walking://Для состояния передвижения.
                                            model.SetAnimation("Walking");//Устанавливаем анимацию движения.
                attackSoundMaker.source.clip = null;//Обнуляем клип атаки, чтобы когда вернулись в атакующий режим, клип проигрался снова 1 раз.
                break;
            case Property_UnitState.UseItem_Test://Для использования тестового предмета.
                                                 model.SetAnimation("Idle0");//Устанавливаем анимацию покоя.
                attackSoundMaker.source.clip = null;//Обнуляем клип атаки, чтобы когда вернулись в атакующий режим, клип проигрался снова 1 раз.
                break;
            case Property_UnitState.Eat://Для поедания
                model.SetAnimation("Eating");//Устанавливаем анимацию смерти.
                attackSoundMaker.source.clip = null;//Обнуляем клип атаки, чтобы когда вернулись в атакующий режим, клип проигрался снова 1 раз.
                break;
            case Property_UnitState.Attack://Для атаки.
                model.SetAnimation("Attack");//Устанавливаем анимацию атаки.
                if(attackSoundMaker.source.clip == null)
                {
                    attackSoundMaker.source.clip = attackSounds[Random.Range(0, attackSounds.Count - 1)];//Возвращем случайный звук из коллекции.
                    attackSoundMaker.source.Play();
                }
                break;
            case Property_UnitState.Dead://Для использования тестового предмета.
                model.SetAnimation("Death");//Устанавливаем анимацию смерти.
                if(!deathSoundsPlayed)
                {
                    deathSoundMaker.source.clip = deathSound[Random.Range(0, deathSound.Count - 1)];//Возвращаем случайный звук из коллекции.
                    deathSoundMaker.source.Play();
                    deathSoundsPlayed = true;
                }
                attackSoundMaker.source.clip = null;//Обнуляем клип атаки, чтобы когда вернулись в атакующий режим, клип проигрался снова 1 раз.
                break;
            case Property_UnitState.OnGetDamage://При получении урона.
                model.SetAnimation("OnGetDamage");//Устанавливаем анимацию получения урона.
                break;
            case Property_UnitState.OnInWindow://При выходе в окно.
                model.SetAnimation("Hide");//Анимация выхода в окно.
                break;
            case Property_UnitState.OnOutWindow://При выходе из окна.
                model.SetAnimation("Appear");//Анимация выхода в окно.
                break;
        }
    }

    public Vector2Int HaveItem(Source_ItemBase checkItem)//Есть ли у юнита нужный предмет.
    {
        for (int groupId = 0; groupId < inventory.groups.Count; groupId++)//Для каждой группы инвентаря юнита.
        {
            for (int itemPos = 0; itemPos < inventory.groups[groupId].items.Count; itemPos++)//Для каждого предмета в группе.
            {
                if (inventory.groups[groupId].items[itemPos].data == checkItem)//Если нужный предмет.
                {
                    return new Vector2Int(groupId, itemPos);//Возвращаем положение предмета.
                }
            }
        }
        return new Vector2Int(-1, -1);//Возвращаем пустоту.
    }

    public bool ChangeItemStacks(int groupId, int itemPos, int changeValue)//Изменение количество стаков предмета.
    {
        if(inventory.groups[groupId].items[itemPos].stacks + changeValue >= 0)//Если стаков достаточно.
        {
            inventory.groups[groupId].items[itemPos].stacks += changeValue;//Меняем значение.
            return true;
        }
        return false;
    }

    public bool SwitchItems(int start_groupId, int start_itemPos, int end_groupId, int end_itemPos)//Менять предметы местами.
    {
        bool endGroupCanInsert = false;//Может ли конечная группа принять предмет.
        bool startGroupCanInsert = false;//Может ли стартовая группа принять предмет.
        if (start_groupId != -1 && end_groupId != -1)//Если есть откуда и куда переносить.
        {
            if ((start_groupId != end_groupId))//Если стартовая и конечная группа - разные.
            {
                if(end_itemPos != -1)//Если перетаскиваем не в пустую ячейку.
                {
                    if (inventory.groups[start_groupId].maxWeight > 0)//Если конечная группа не бесконечна в весе.
                    {
                        startGroupCanInsert =
                    (inventory.groups[start_groupId].LeftWeight + inventory.groups[start_groupId].items[start_itemPos].data.weight) >=
                    inventory.groups[end_groupId].items[end_itemPos].data.weight;
                    }
                    else
                    {
                        startGroupCanInsert = true;
                    }

                    if (inventory.groups[end_groupId].maxWeight > 0)//Если конечная группа не бесконечна в весе.
                    {
                        endGroupCanInsert =
                        (inventory.groups[end_groupId].LeftWeight + inventory.groups[end_groupId].items[end_itemPos].data.weight) >=
                        inventory.groups[start_groupId].items[start_itemPos].data.weight;
                    }
                    else//Если бесконечная.
                    {
                        endGroupCanInsert = true;
                    }
                }
                else//Если перетаскиваем в пустую ячейку.
                {
                    startGroupCanInsert = true;//В стартовую группу может поместиться.
                    if(inventory.groups[end_groupId].maxWeight > 0)//Если конечная группа не бесконечна.
                    {
                        endGroupCanInsert = inventory.groups[end_groupId].LeftWeight >= inventory.groups[start_groupId].items[start_itemPos].data.weight;//В конечную если хватает места.
                    }
                    else
                    {
                        endGroupCanInsert = true;//Если бесконечна, то предмет может поместиться.
                    }
                }

                if (endGroupCanInsert && startGroupCanInsert)//Если обе группы вмещают новые предметы.
                {
                        Source_InventoryItem tempStartItem = inventory.groups[start_groupId].items[start_itemPos];//Записываем стартовый предмет предмет.
                        Source_InventoryItem tempEndItem = end_itemPos != -1 ? inventory.groups[end_groupId].items[end_itemPos] : null;

                        if (inventory.groups[start_groupId].hotkeyName.Length > 0)//Если у группы есть горячая клавиша.
                        {
                            if(inventory.groups[start_groupId].items[0].childrenGO != null)//Если у предмета есть дочерний объект.
                            {
                                Destroy(inventory.groups[start_groupId].items[0].childrenGO);//Уничтожаем дочерний объект.
                            }
                            if(inventory.groups[start_groupId].items[0].data.animationSetInHand != null)//Если есть анимации с предметом в руках.
                            {
                                model.SetModel(modelData, startAnimationName);//Устанавливаем стандартную анимацию.
                            }
                        }
                        if(inventory.groups[end_groupId].hotkeyName.Length > 0)//Если у конечной группы есть горячая клавиша.
                        {
                            if (inventory.groups[end_groupId].items.Count > 0)//Если в конечной группе есть предмет.
                            {
                                if (inventory.groups[end_groupId].items[0].childrenGO != null)//Если у предмета есть дочерний объект.
                                {
                                    Destroy(inventory.groups[end_groupId].items[0].childrenGO);//Уничтожаем дочерний объект.
                                }
                                if (inventory.groups[end_groupId].items[0].data.animationSetInHand != null)//Если есть анимации с предметом в руках.
                                {
                                    model.SetModel(modelData, startAnimationName);//Устанавливаем стандартную анимацию.
                                }
                            }
                        }
                        inventory.groups[start_groupId].items.RemoveAt(start_itemPos);//Удаляем предмет из стартовой группы.
                        if (end_itemPos != -1)//Если есть предмет который нужно удалить.
                        {
                            inventory.groups[end_groupId].items.RemoveAt(end_itemPos);//Удаляем предмет из конечной группы.
                        }

                        if(end_itemPos != -1)//Если есть предмет, который нужно поместить в стартовую группу.
                        {
                            inventory.groups[start_groupId].items.Add(tempEndItem);//Добавляем предмет из конечной группы к стартовой.
                        }
                        inventory.groups[end_groupId].items.Add(tempStartItem);//Добавляем предмет из стартовой группы к конечной.
                        if(inventory.groups[end_groupId].hotkeyName.Length > 0)//Если у группы есть горячая клавиша.
                        {
                            if(inventory.groups[end_groupId].items[0].data.childPrefab != null)//Если есть активный префаб у предмета.
                            {
                                inventory.groups[end_groupId].items[0].childrenGO = Instantiate(inventory.groups[end_groupId].items[0].data.childPrefab, transform).gameObject;//Создаем объект для предмета.
                                inventory.groups[end_groupId].items[0].childrenGO.name = inventory.groups[end_groupId].items[0].data.name;//Устанавливаем имя.
                                inventory.groups[end_groupId].items[0].childrenGO.GetComponent<Object_ItemChildren>().parentGroup = end_groupId;//Присваиваем группу.
                                inventory.groups[end_groupId].items[0].childrenGO.GetComponent<Object_ItemChildren>().parentPos = 0;//Присваиваем позицию.
                                inventory.groups[end_groupId].items[0].childrenGO.GetComponent<Object_ItemChildren>().unit = this;//Присваиваем позицию.
                            }
                            if (inventory.groups[end_groupId].items[0].data.animationSetInHand != null)//Если есть анимации с предметом в руках.
                            {
                                model.SetModel(inventory.groups[end_groupId].items[0].data.animationSetInHand, startAnimationName);//Устанавливаем новую анимацию.
                            }
                            else
                            {
                                model.SetModel(modelData, startAnimationName);//Устанавливаем стандартную анимацию.
                            }
                        }
                        if(inventory.groups[start_groupId].hotkeyName.Length > 0)//Если у стартовой группы есть горячая клавиша.
                        {
                        if (inventory.groups[start_groupId].items.Count > 0)//Если в группе есть предметы.
                        {
                            if (inventory.groups[start_groupId].items[0].data.childPrefab != null)//Если есть активный префаб у предмета.
                            {
                                inventory.groups[start_groupId].items[0].childrenGO = Instantiate(inventory.groups[start_groupId].items[0].data.childPrefab, transform).gameObject;//Создаем объект для предмета.
                                inventory.groups[start_groupId].items[0].childrenGO.name = inventory.groups[start_groupId].items[0].data.name;//Устанавливаем имя.
                                inventory.groups[start_groupId].items[0].childrenGO.GetComponent<Object_ItemChildren>().parentGroup = end_groupId;//Присваиваем группу.
                                inventory.groups[start_groupId].items[0].childrenGO.GetComponent<Object_ItemChildren>().parentPos = 0;//Присваиваем позицию.
                                inventory.groups[start_groupId].items[0].childrenGO.GetComponent<Object_ItemChildren>().unit = this;//Присваиваем позицию.
                                if (inventory.groups[start_groupId].items[0].data.animationSetInHand != null)//Если есть анимации с предметом в руках.
                                {
                                    model.SetModel(inventory.groups[start_groupId].items[0].data.animationSetInHand, startAnimationName);//Устанавливаем новую анимацию.
                                }
                                else
                                {
                                    model.SetModel(modelData, startAnimationName);//Устанавливаем стандартную анимацию.
                                }
                            }
                        }
                    }
                        return true;
                    }
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    public bool DropItem(int groupId, int itemPos)//Выкидывание предмета из инвентаря.
    {
        if (groupId >= 0 && groupId < inventory.groups.Count)
        {
            if (itemPos >= 0 && itemPos < inventory.groups[groupId].items.Count)
            {
                if (inventory.groups[groupId].items[itemPos].data.prefab != null)//Если есть префаб предмета.
                {
                    if (inventory.groups[groupId].hotkeyName.Length > 0)//Если у конечной группы есть горячая клавиша.
                    {
                        if (inventory.groups[groupId].items.Count > 0)//Если в конечной группе есть предмет.
                        {
                            if (inventory.groups[groupId].items[itemPos].childrenGO != null)//Если у предмета есть дочерний объект.
                            {
                                Destroy(inventory.groups[groupId].items[itemPos].childrenGO);//Уничтожаем дочерний объект.
                            }
                            if (inventory.groups[groupId].items[itemPos].data.animationSetInHand != null)//Если есть анимации с предметом в руках.
                            {
                                model.SetModel(modelData, startAnimationName);//Устанавливаем стандартную анимацию.
                            }
                        }
                    }
                    Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - transform.localScale.y/4, transform.position.z);//Текущая позиция.
                    Transform itemObject = Instantiate(inventory.groups[groupId].items[itemPos].data.prefab, spawnPosition, inventory.groups[groupId].items[itemPos].data.prefab.rotation);//Создаем объект предмета.
                    if (itemObject != null)//Если предмет создан.
                    {
                        itemObject.GetComponent<Object_Item>().Data = inventory.groups[groupId].items[itemPos];
                        RemoveItem(groupId, itemPos);//Удаляем предмет из инвентаря.
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool RemoveItem(int groupId, int itemPos)//Удаление предмета из инвентаря.
    {
        if (groupId >= 0 && groupId < inventory.groups.Count)
        {
            if(itemPos >= 0 && itemPos < inventory.groups[groupId].items.Count)
            {
                inventory.groups[groupId].items.RemoveAt(itemPos);//Удаляем предмет из инвентаря.
                return true;
            }
        }
        return false;
    }

    public bool AddItem(Source_InventoryItem itemData)//Добавление предмета в инвентарь(Консоль либо подбор, либо загрузка).
    {
        if (itemData != null)
        {
            foreach (Source_InventoryGroup group in inventory.groups)
            {
                if (group.LeftWeight >= itemData.data.weight)//Если в группе достаточно места.
                {
                    Source_InventoryItem itemToAdd = new Source_InventoryItem(itemData);//Создание копии предмета.
                    group.items.Add(itemToAdd);//Добавляем предмет к группе.
                    if (group.hotkeyName.Length > 0)//Если у группы есть горячая клавиша.
                    {
                       if (group.items[0].data.childPrefab != null)//Если есть активный префаб у предмета.
                       {
                           group.items[0].childrenGO = Instantiate(group.items[0].data.childPrefab, transform).gameObject;//Создаем объект для предмета.
                         group.items[0].childrenGO.name = group.items[0].data.name;//Устанавливаем имя.
                       }
                        if (group.items[0].data.animationSetInHand != null)//Если есть анимации с предметом в руках.
                        {
                            //model.SetModel(group.items[0].data.animationSetInHand, startAnimationName);//Устанавливаем новую анимацию.
                        }
                       else
                        {
                           model.SetModel(modelData, startAnimationName);//Устанавливаем стандартную анимацию.
                       }
                    }
                    return true;//Возвращаем успешность.
                }
                continue;
            }
        }
        return false;
    }
}

public enum Property_UnitState { Idle, Walking, UseItem_Test, Dead, Eat, Attack, OnGetDamage, OnInWindow, OnOutWindow}
public enum Propety_UnitTeam {Friendly, Enemy, Neutral, Animal, Unknown}