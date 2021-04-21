using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Source_GameManager : MonoBehaviour
{
    public GameStage stage = GameStage.MainMenu;//По умолчанию находимся в главном меню.
    public MainMenu mainMenu = MainMenu.MainMenu;//Открытая вкладка главного меню.
    public GameMode gameMode = GameMode.gm3d;//Режим игры.
    public SettingsTab settingsTab = SettingsTab.Hotkeys;//Активная вкладка в меню настроек.

    public List<Property_UnitData> units;//Все юниты на сцене.
    public List<Object_Trigger> triggers;//Все триггеры на карте.
    public List<Object_Chest> chests;//Все сундуки на карте.
    public List<Object_CameraSector> cameraSectors;//Все сектора камеры на карте.
    public List<Object_Teleport> teleports;//Все порталы на карте.
    public List<Object_SoundMaker> soundMakers;//Все источники звука на карте.
    public List<Object_DoorConnecter> doorConnecters;//Все двери на карте.
    public List<Object_SoundRegion> soundRegions;//Все регионы звуков на карте.

    //Ссылки на остальные менеджеры.
    private Source_CameraManager cameraManager;//Мендежер камеры.
    private Source_EventsManager eventsManager;//Менеджер событий.

    public string loadingOperationName;//Что в данный момент загружает менеджер.
    private Source_UnitInventory inventoryTransfer;//Переменная для передачи инвентаря юнита между уровнями.

    public float gammaCorrection;//

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);//Не уничтожать между сценами.
    }

    private void Start()//При старте сцены.
    {
        //Ссылки на другие менеджеры.
        cameraManager = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<Source_CameraManager>();//Получаем ссылку на менеджер камеры.
        eventsManager = GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>();//Менеджер событий.
        UpdateMapObjects();//Обновление при запуске первой сцены.
    }

    public void Save_UnitInventory()//Сохранение инвентаря управляемого юнита.
    {
        if (Camera.main.GetComponent<Object_Player>().unit != null)
        {
            inventoryTransfer = new Source_UnitInventory(Camera.main.GetComponent<Object_Player>().unit.inventory);
        }
    }

    public void Load_UnitInventory()
    {
        if(Camera.main.GetComponent<Object_Player>().unit != null)
        {
            Camera.main.GetComponent<Object_Player>().unit.GetComponent<Object_Unit>().inventory = new Source_UnitInventory(inventoryTransfer);
        }
    }

    public void UpdateMapObjects()
    {
        units = new List<Property_UnitData>();//Создаем новый список юнитов на сцене.
        GameObject[] unitsAtStart = GameObject.FindGameObjectsWithTag("Object_Unit");//Ищем всех юнитов на сцене.
        foreach (GameObject unit in unitsAtStart)//Для каждого найденного объекта.
        {
            AddUnitData(unit);//Добавляем юнит в список.
        }

        triggers = new List<Object_Trigger>();//Создаем новый список тригеров на сцене.
        GameObject[] triggersAtStart = GameObject.FindGameObjectsWithTag("Object_Trigger");//Ищем все триггеры на сцене.
        foreach (GameObject trigger in triggersAtStart)//Для каждого найденного объекта.
        {
            AddTriggerData(trigger);//Добавляем триггер в список.
        }

        chests = new List<Object_Chest>();//Создаем новый список сундуков на сцене.
        GameObject[] chestsAtStart = GameObject.FindGameObjectsWithTag("Object_Chest");//Ищем все сундуки на сцене.
        foreach (GameObject chest in chestsAtStart)//Для каждого найденного объекта.
        {
            AddChestData(chest);//Добавляем сундук в список.
        }

        cameraSectors = new List<Object_CameraSector>();//Создаем новый список секторов камер на сцене.
        GameObject[] cameraSectorsAtStart = GameObject.FindGameObjectsWithTag("Object_CameraSector");//Ищем все сектора на сцене.
        foreach (GameObject cs in cameraSectorsAtStart)//Для каждого найденного объекта.
        {
            AddCameraSectorData(cs);//Добавляем сектор в список.
        }
        teleports = new List<Object_Teleport>();//Создаем новый список порталов на сцене.
        GameObject[] teleportsAtStart = GameObject.FindGameObjectsWithTag("Object_Teleport");//Ищем все телепорты на сцене.
        foreach(GameObject tp in teleportsAtStart)//Для каждого найденного портала.
        {
            AddTeleportData(tp);//Добавляем портал в список.
        }
        soundMakers = new List<Object_SoundMaker>();//Создаем новый список звуков на сцене.
        GameObject[] smAtStart = GameObject.FindGameObjectsWithTag("Object_SoundMaker");//Ищем все телепорты на сцене.
        foreach (GameObject sm in smAtStart)//Для каждого найденного портала.
        {
            AddSoundMakerData(sm);//Добавляем источник звука в список.
        }
        doorConnecters = new List<Object_DoorConnecter>();//Создаем новый список дверей на сцене.
        GameObject[] dcAtStart = GameObject.FindGameObjectsWithTag("Object_DoorConnecter");//Ищем все двери на сцене.
        foreach (GameObject dc in dcAtStart)//Для каждого найденного портала.
        {
            AddDoorConnecterData(dc);//Добавляем дверь в список.
        }
        soundRegions = new List<Object_SoundRegion>();//Создаем новый список регионов со звуками на сцене.
        GameObject[] srAtStart = GameObject.FindGameObjectsWithTag("Object_SoundRegion");//Ищем все саунд регионы на сцене.
        foreach (GameObject sr in srAtStart)//Для каждого найденного портала.
        {
            AddSoundRegionData(sr);//Добавляем дверь в список.
        }
    }

    private void Update()//Выполняется каждый кадр.
    {
        if(stage == GameStage.MainMenu)//Если находимся в главном меню.
        {
            SetCameraToMainMenu();//Устанавливаем камеру в главном меню.
            if(mainMenu == MainMenu.MainMenu)//Если открыто главное меню.
            {

            }
            if(mainMenu == MainMenu.Settings)//Если открыто меню настроек.
            {

            }
        }
        RenderSettings.ambientLight = new Vector4(gammaCorrection, gammaCorrection, gammaCorrection, 1.0f);
    }

    public void GoToNextScene(int SceneId)//Переход на следующую сцену.
    {
        Debug.Log("Переход на следующую сцену.");
        Camera.main.GetComponent<Object_Player>().activeWorldLayerMask = Camera.main.GetComponent<Object_Player>().worldMask;
        Camera.main.GetComponent<Object_Player>().cameraState = Camera_State.Await;
        stage = GameStage.SceneLoading;//Устанавливаем игру на загрузку.
        StartCoroutine(LoadScene(SceneId));//Загружаем сцену.
    }

    public void StartGame(int SceneId)//Начало одиночной игры.
    {
        Debug.Log("Запуск одиночной истории");//Дебаг.
        Camera.main.GetComponent<PC_Notepad>().quests = new List<Source_Quest>();
        Camera.main.GetComponent<PC_Notepad>().unitTips = new List<Source_Tip>();
        stage = GameStage.SceneLoading;//Устанавливаем игру на загрузку.
        StartCoroutine(LoadScene(SceneId));//Загружаем сцену.
    }

    public void InitializeGameManagers()//Инициализация всех менеджеров.
    {
        loadingOperationName = "Initializating managers...";//Сообщаем о текущей операции.
        UpdateMapObjects();//Загружаем все объекты карты.
        loadingOperationName = "Await events system...";//Сообщаем о текущей операции.
        stage = GameStage.AwaitingEventsSystem;//Загрузка.
        eventsManager.awaitingStartEvent = true;//Устанавливаем менеджер в ожидание.
    }

    IEnumerator LoadScene(int sceneId)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneId);//Асинхронно загружаем сцену.
        async.allowSceneActivation = false;
        while (!async.isDone)//Пока загрузка сцены не завершена.
        {
            loadingOperationName = "World loading...";//Сообщаем о текущей операции.
            if (async.progress == 0.9f)
            {
                async.allowSceneActivation = true;
                UpdateMapObjects();//Загружаем все объекты карты.
            }
            yield return null;
        }
        loadingOperationName = "World loading...";//Сообщаем о текущей операции.
        stage = GameStage.Loading;//Загрузка.
        InitializeGameManagers();
    }

    private void SetCameraToMainMenu()
    {
        //Помещаем камеру в главное меню.
        GameObject mainMenuCamPos = GameObject.FindGameObjectWithTag("Scene_MainMenu");//Ищем родительский объект сцены главного меню.
        if (mainMenuCamPos != null)//Если есть позиция камеры, для меню.
        {
            cameraManager.PlaceCameraAt(mainMenuCamPos.transform.position, mainMenuCamPos.transform.eulerAngles);//Помещаем камеру для просмотра сцены меню.
        }
    }

    public bool AddSoundRegionData(GameObject sr)
    {
        Object_SoundRegion data = sr.GetComponent<Object_SoundRegion>();//Данные саунд региона.
        if (data != null)//Если данные существуют.
        {
            this.soundRegions.Add(data);//Добавляем саунд регион в список.
            return true;
        }
        return false;
    }

    public bool AddDoorConnecterData(GameObject dc)
    {
        Object_DoorConnecter data = dc.GetComponent<Object_DoorConnecter>();//Данные двери.
        if (data != null)//Если данные существуют.
        {
            this.doorConnecters.Add(data);//Добавляем дверь в список.
            return true;
        }
        return false;
    }

    public bool AddSoundMakerData(GameObject sm)
    {
        Object_SoundMaker data = sm.GetComponent<Object_SoundMaker>();//Данные портала.
        if (data != null)//Если данные существуют.
        {
            this.soundMakers.Add(data);//Добавляем портал в список.
            return true;
        }
        return false;
    }

    public bool AddTeleportData(GameObject teleport)
    {
        Object_Teleport data = teleport.GetComponent<Object_Teleport>();//Данные портала.
        if (data != null)//Если данные существуют.
        {
            this.teleports.Add(data);//Добавляем портал в список.
            return true;
        }
        return false;
    }

    public bool AddCameraSectorData(GameObject cameraSector)
    {
        Object_CameraSector data = cameraSector.GetComponent<Object_CameraSector>();//Данные сектора.
        if (data != null)//Если данные существуют.
        {
            this.cameraSectors.Add(data);//Добавляем сектор в список.
            return true;
        }
        return false;
    }

    public bool AddChestData(GameObject chest)//Добавление сундука в список.
    {
        Object_Chest data = chest.GetComponent<Object_Chest>();//Данные сундука.
        if (data != null)//Если данные существуют.
        {
            this.chests.Add(data);//Добавляем сундук в список.
            return true;
        }
        return false;
    }

    public bool AddTriggerData(GameObject trigger)//Добавление триггера в список.
    {
        bool added = false;
        foreach(Object_Trigger triggerData in this.triggers)
        {
            if(triggerData.gameObject == trigger)//Если уже есть в списке.
            {
                added = true;
                break;
            }
        }
        if (!added)
        {
            Object_Trigger data = trigger.GetComponent<Object_Trigger>();//Данные триггера.
            if (data != null)//Если данные существуют.
            {
                this.triggers.Add(data);//Добавляем сундук в список.
                return true;
            }
        }
        return false;
    }
    public bool RemoveTriggerData(GameObject trigger)//Удаление триггера из списка.
    {
        Object_Trigger data = trigger.GetComponent<Object_Trigger>();//Данные триггера.
        this.triggers.Remove(data);//Удаляем триггер из списка.
        return false;
    }

    public bool AddUnitData(GameObject unit)//Добавление данных юнита в список.
    {
        Object_Unit data = unit.GetComponent<Object_Unit>();//Данные юнита.
        if (data != null)//Если данные существуют.
        {
            this.units.Add(new Property_UnitData(data.uniqueID, data, 0, 0));//Записываем данные юнита в список.
            return true;
        }
        return false;
    }

    public Object_Unit GetUnitDataByUniqueId(int uniqueId)//Возвращает ссылку на юнит, по уникальному id
    {
        Object_Unit result = null;//Возвращаемый объект.

        foreach(Property_UnitData unit in units)//Для каждого юнита в списке.
        {
            if(unit.unitUniqueID == uniqueId)//Если уникальные id совпадают
            {
                result = unit.data;//Возвращаемый объект - данные юнита.
                break;
            }
            continue;
        }

        return result;//Возвращаем результат.
    }

    public bool ChangeStageForUnitTo(int unitUniqueId, int newStage)//Изменение прогресса с юнитом, по id.
    {
        foreach (Property_UnitData unit in units)//Для каждых юнита.
        {
            if (unit.unitUniqueID == unitUniqueId)//Если id юнита - искомый id.
            {
                unit.stageId = newStage;//Устанавливаем новый этап общения с юнитом.
                return true;
            }
            continue;
        }
        return false;
    }

    public int GetStageForUnit(int unitUniqueID)//Получить на каком этапе находиться игрок для определенного юнита.
    {
        foreach (Property_UnitData unit in units)//Для каждого юнита в списке.
        {
            if (unit.unitUniqueID == unitUniqueID)//Если это нужный нам юнит.
            {
                return unit.stageId;//Возвращаем этап.
            }
            continue;
        }
        return -1;//Возвращаем, что данных нет.
    }

    public bool AddRootForUnit(int unitUniqueId, int rootAdd)//Добавляет рут к персонажу.
    {
        foreach (Property_UnitData unit in units)//Для каждых юнита.
        {
            if (unit.unitUniqueID == unitUniqueId)//Если id юнита - искомый id.
            {
                unit.root += rootAdd;//Добавляем руты.
                return true;
            }
            continue;
        }
        return false;
    }

    public bool EnoughRoot(int unitUniqueId, int rootNeed)//Проверка на хватание рутов.
    {
        foreach (Property_UnitData unit in units)//Для каждых юнита.
        {
            if (unit.unitUniqueID == unitUniqueId)//Если id юнита - искомый id.
            {
                return rootNeed <= unit.root;//
            }
            continue;
        }
        return false;
    }
}

public enum GameMode { gm3d, gm2d}
public enum GameStage { MainMenu, Loading, Game, SceneLoading, AwaitingEventsSystem}//В каком состоянии находиться игра.
public enum MainMenu { MainMenu, Settings}//Вкладки главного меню.
public enum SettingsTab { Hotkeys, Video, Audio}