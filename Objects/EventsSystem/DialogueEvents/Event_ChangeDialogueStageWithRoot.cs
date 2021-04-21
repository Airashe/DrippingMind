using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ChangeDialogueStageWithRoot : Object_Event
{
    public int uniqueUnitId = -1;//Для какого юнита.
    public int needRoot = 0;//Сколько очков нужно.
    public int newUnitStageTrue = -1;//Какой этап диалого поставить.
    public int newUnitStageFalse = -1;//Какой этап диалого поставить.
    public bool requestNewDialogue;//Запрашивать ли новый диалог (если этап меняется для того же юнита).
    public bool finishDialogue;//Завершить текущий диалог.

    public override void Initialize()
    {
        base.Initialize();
        Object_Player playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Ссылка на данные игрока.
        Source_GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Ссылка на гейм менеджер.
        PC_DialogueController dialogueController = Camera.main.GetComponent<PC_DialogueController>();//Ссылка на контроллер диалогов.
        int newUnitStage = gameManager.EnoughRoot(uniqueUnitId, needRoot) ? newUnitStageTrue : newUnitStageFalse;
        Debug.Log(gameManager.EnoughRoot(uniqueUnitId, needRoot));
        gameManager.ChangeStageForUnitTo(uniqueUnitId, newUnitStage);//Изменяем stage для юнита.
        if(requestNewDialogue)//Если нужно запросить новый диалог.
        {
            PI_Dialogue dialogueInterface = Camera.main.GetComponent<PI_Dialogue>();//Ссылка на контроллер интерфейса диалогов.
            dialogueController.currentLineId = -1;//Обнуляем строчку фразы нпс, чтобы фраза началась с начала.
            dialogueController.currentDialogueId = playerData.dialogueTree.GetDialogueId(dialogueController.currentBranchID, newUnitStage);//Получаем диалог из ветки, по определенному этапу.
            dialogueInterface.dialogueLine = "";//Опусташаем текущую фразу, чтобы произошел запрос новой.
            dialogueInterface.dialogueAnswers = new List<string>();//Обнуляем ответы(Чтобы произошел запрос новых ответов).
            dialogueInterface.dialogueGUIStage = GUI_DialogueStage.ShowInterfaceNPCPart;//Показываем фразы нпс.
        }
        if(finishDialogue)//Если нужно завершить диалог.
        {
            dialogueController.FinishDialogue();//Завершаем текущий диалог.
        }
    }
}
