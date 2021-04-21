using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_DialogueController : MonoBehaviour
{
    //Ссылки на менеджеры данных.
    private Source_GameManager gameManager;//Ссылка на гейм менежер.
    private Object_Player playerData;//Данные игрока.
    private PI_Dialogue dialogueInterface;//Ссылка на интерфейс диалога.
    private PC_CharacterController playerController;//Контроль игрока над юнитом.
    private PI_IgInventory inventoryInterface;//Контроллер инвентаря.
    private PI_IngameGUI ingameGUI;//Контроллер внутриигрового гуи.
    private Source_EventsManager eventsManager;//Меденждер событий.
    private PC_MainUseLogic mainUseLogic;//Скрипт который и вызывает взаимодействие.

    //Данные текущего диалога.
    public bool dialogueActive = false;//Ведеться ли сейчас диалог.
    public int currentBranchID = -1;
    public int currentDialogueId = -1;
    public int currentLineId = -1;//Текущая фраза.
    public Object_Unit dialogueUnit;//Юнит с которым ведеться диалог.

    //Данные перед началом текущего диалога.
    private Vector3 lastPoint;//Последние координаты камеры, перед диалогом.
    private Quaternion lastRotation;//Последний поворот камеры, перед диалогом.
    private Vector3 lastUnitLookDirection;//Куда смотрел юнит, перед диалогом.
    private Vector3 dialoguePoint;//Точка в которую будет перемещена камера.

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Ссылка на гейм манагер.
        playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Ссылка на данные игрока.
        ingameGUI = Camera.main.GetComponent<PI_IngameGUI>();//Ссылка на внутриигровой интерфейс.
        dialogueInterface = gameObject.GetComponent<PI_Dialogue>();//Получаем ссылку.
        playerController = gameObject.GetComponent<PC_CharacterController>();//Получаем ссылку.
        inventoryInterface = gameObject.GetComponent<PI_IgInventory>();//Получаем ссылку на контроллер и интерфейс инвентаря.
        eventsManager = GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>();//Получаем ссылку на менеджер событий.
        mainUseLogic = Camera.main.GetComponent<PC_MainUseLogic>();//Ссылка на использователь объектов.
    }

    private void Update()
    {
        if (playerData != null)//Если есть контроллер игрока.
        {
            if (playerData.unit != null)//Если у игрока есть подконтрольный юнит.
            {
                if (!dialogueActive)//Если нет активных диалогов.
                {
                    GameObject[] sceneUnits = GameObject.FindGameObjectsWithTag("Object_Unit");//Получаем список всех предметов на сцене.
                    foreach (GameObject unit in sceneUnits)//Для каждого предмета.
                    {
                        if (unit != playerData.unit.gameObject)//Если это не юнит игрока.
                        {
                            if (Vector3.Distance(playerData.unit.transform.position, unit.transform.position) < Source_Constants.CONST_GAMEPLAY_PLAYER_DIALOGUE_START_DISTANCE)//Если игрок дотягивается до предмета.
                            {
                                if (unit.GetComponent<Object_Unit>().team == Propety_UnitTeam.Friendly || unit.GetComponent<Object_Unit>().team == Propety_UnitTeam.Neutral)//Если юнит дружественный.
                                {
                                    mainUseLogic.AddUseItem(unit.gameObject, UseData_ObjType.Unit);//Добавляем юнита в список использования.
                                }
                            }
                        }
                    }
                }
            }
            playerController.enabled = !dialogueActive;//Отключаем передвижение юнитов или включаем.
            ingameGUI.enabled = !dialogueActive;//Отключаем внутриигровой интерфейс, ежи пиздим.
            if (inventoryInterface != null)//Если есть 
            {
                inventoryInterface.enabled = !dialogueActive;//Отключаем передвижение юнитов или включаем.
            }
        }
        if(dialogueActive)//Если есть активный диалог.
        {
            MoveCameraToDialoguePoint();//Двигаем камеру к точке диалога, если он идет.
            if(dialogueInterface.dialogueGUIStage == GUI_DialogueStage.FinishDialogue)//Если находимся в завершении диалога.
            {
                if(dialogueInterface.fadeAlpha < 1)//Если затемнения не произошло.
                {
                    dialogueInterface.FadeOn();//Затемняем экран.
                }
                else//Если затемнение произошло.
                {
                    //Возвращение камеры и юнита собеседника на позицию, показывание юнита игрока.
                    Camera.main.transform.position = lastPoint;//Возвращаем последнюю позицию камеры.
                    Camera.main.transform.rotation = lastRotation;//Возвращаем последний поворот камеры.
                    dialogueUnit.transform.eulerAngles = lastUnitLookDirection;//Возвращаем поворот юнита.
                    playerData.unit.Visible = true;//Показываем юнит, под управлением игрока.

                    //Обнуление переменных интерфейса.
                    dialogueInterface.showBlackStrips = false;//Скрываем черные полоски.
                    dialogueInterface.dialogueLine = "";//Обнуляем последнюю фразу собседника.
                    dialogueInterface.dialogueAnswers = new List<string>();//Обнуляем возможные ответы.
                    dialogueInterface.dialogueGUIStage = GUI_DialogueStage.ReadyToNext;//Переходим на этап - вне диалога.

                    //Обнуление переменных контроллера.
                    currentBranchID = -1;//Обнуляем текущую ветку диалога.
                    currentDialogueId = -1;//Обнуляем текущий диалог.
                    currentLineId = -1;//Обнуляем текущую фразу юнита собеседника.
                    dialogueUnit = null;//Обнуляем переменную собеседника.
                    lastPoint = Vector3.zero;//Обнуляем последнюю точку камеры.
                    lastUnitLookDirection = Vector3.zero;//Обнуляем последнее направление юнита.
                }
            }
            if(dialogueInterface.dialogueGUIStage == GUI_DialogueStage.ReadyToNext)//Если диалог закончен.
            {
                if (dialogueInterface.fadeAlpha > 0)//Если прояснение не произошло.
                {
                    dialogueInterface.FadeOff();//Проясняем экран.
                }
                else//Если экран прояснен.
                {
                    dialogueActive = false;//Диалог более не активен.
                    dialogueInterface.dialogueGUIStage = GUI_DialogueStage.NotInDialogue;
                    playerData.cameraState = Camera_State.Await;//Переводим камеру в состояние - ожидает команд.
                }
            }
        }
    }

    public void OnUseInput(Object_Unit unit)
    {
        if (unit.GetComponent<Object_Unit>().team == Propety_UnitTeam.Friendly || unit.GetComponent<Object_Unit>().team == Propety_UnitTeam.Neutral)//Если юнит дружественный.
        {
            StartDialogue(unit.GetComponent<Object_Unit>());//Начать диалог с юнитом.
        }
    }

    public void OnUserAnswer(int answId)//Когда получаем ответ.
    {
        if(eventsManager != null)//Если менеджер событий был найден.
        {
            eventsManager.EngageEvent(playerData.dialogueTree.branches[currentBranchID].dialogues[currentDialogueId].answers[answId].evenetsId);//Запускаем все ивенты связанные с ответом.
        }
    }

    public List<string> GetAnswers()//Запрашиваем список ответов.
    {
        return playerData.dialogueTree.GetAnswers(currentBranchID, currentDialogueId);//Запрашиваем ответы из древа диалогов игрока.
    }

    public string GetNextLine()//Запрашивает следующую фразу диалога.
    {
        currentLineId += 1;//Переходим на следующую строку.
        string[] currentLine = playerData.dialogueTree.GetUnitLine(currentBranchID, currentDialogueId);
        if (currentLineId >= 0 && currentLineId < currentLine.Length)//Если диалог не вышел за пределы.
        {
            if(currentLineId == currentLine.Length - 1)//Если это последняя строка этой фразы.
            {
                return currentLine[currentLineId] + "@endline";//Передаем строку фразы с специальным символом конца фразы.
            }
            else
            {
                return currentLine[currentLineId];//Передаем строку фразы.
            }
        }
        return "@endline";//Если строка не была найдена, возвращаем конец фразы.
    }

    public void StartDialogue(Object_Unit dialogueUnit)//Начало диалога с чуваками.
    {
        if(playerData.dialogueTree != null)//Если есть данные древа диалогов.
        {
            currentBranchID = playerData.dialogueTree.GetBranchIndex(dialogueUnit.uniqueID);//Получаем позицию веток диалого данного юнита.
            int stageId = gameManager.GetStageForUnit(dialogueUnit.uniqueID);//Получаем этап юнита.

            if (currentBranchID != -1)//Если ветка для юнита существует.
            {
                currentDialogueId = playerData.dialogueTree.GetDialogueId(currentBranchID, stageId);//Получаем диалог из ветки, по определенному этапу.
                this.dialogueUnit = dialogueUnit;//Записываем, с кем ведем диалог.
                dialogueActive = true;//Сообщаем, что диалог начат.
                currentLineId = -1;//Обнуляем текущую фразу, чтобы при первом запросе выдалась фраза с id = 0.
            }
        }
    }

    public void FinishDialogue()//Завершение диалога.
    {
        dialogueInterface.dialogueGUIStage = GUI_DialogueStage.FinishDialogue;//Переходим на этап окончания диалога.
    }

    public void MoveCameraToDialoguePoint()//Передвигает камеру из текущего положения в положение диалога.
    {
        if(dialogueInterface.dialogueGUIStage == GUI_DialogueStage.NotInDialogue)
        {
            lastPoint = Camera.main.transform.position;//Текущая позиция камеры.
            lastRotation = Camera.main.transform.rotation;//Текущий поворот камеры.
            lastUnitLookDirection = dialogueUnit.transform.eulerAngles;//Текущий поворот юнита.
            dialoguePoint = dialogueUnit.transform.position + Vector3.right * 0.5f;//Точка в которую будет перемещена камера.
            dialoguePoint += Vector3.forward * 0.20f;
            dialogueUnit.model.ChangeRotationMode(false, Vector3.right);//Отключаем поворот к камере.
            playerData.cameraState = Camera_State.Busy;//Переводим камеру в состояние занятости.
        }
            if (Vector3.Distance(Camera.main.transform.position, dialoguePoint) > 0.01f)
            {
                if(dialogueInterface.dialogueGUIStage == GUI_DialogueStage.Begining)
                {
                Camera.main.transform.position = Vector3.MoveTowards(transform.position, dialoguePoint, 1 * Time.deltaTime);
            }
                if (dialogueInterface.dialogueGUIStage == GUI_DialogueStage.NotInDialogue)//Только если не в диалоге - меняем на начало диалога.
                {
                    dialogueInterface.dialogueGUIStage = GUI_DialogueStage.Begining;//Интерфейс - диалог начался.
                }
            }
            else
            {
                if (dialogueInterface.dialogueGUIStage == GUI_DialogueStage.Begining)//Если в подготовке диалога.
                {
                
                dialogueUnit.transform.LookAt(Camera.main.transform.position + Vector3.left * -5);
                playerData.unit.Visible = false;//Скрываем юнит.
                dialogueInterface.dialogueGUIStage = GUI_DialogueStage.ReadyToStart;//Меняем на то, что диалог готов начаться.
                }
                if(dialogueInterface.dialogueGUIStage == GUI_DialogueStage.ReadyToStart)
                {
                Camera.main.transform.LookAt(Camera.main.transform.position + Vector3.left * 5);//Повернули камеру, для разговора.
                 }
            }
    }
}
