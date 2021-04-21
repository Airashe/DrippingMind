using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PI_Dialogue : MonoBehaviour
{
    //Ссылки на объекты.
    private Object_Player playerData;//Данные игрока.
    private PC_DialogueController dialogueController;//Контроллер диалога.

    //Прозрачность черной маски.
    public float fadeAlpha = 1;//Основной цвет маски затемнения.

    //Переменные текущего диалога.
    public GUI_DialogueStage dialogueGUIStage = GUI_DialogueStage.NotInDialogue;//Какой интерфейс сейчас отображать.
    public bool showBlackStrips = false;//Показывать ли ограничители экрана.
    public string dialogueLine = "";//То, что говорит нам собеседник.

    public List<string> dialogueAnswers = new List<string>();//Возможные варианты ответа.

    private void Start()
    {
        dialogueController = gameObject.GetComponent<PC_DialogueController>();//Получаем ссылку на контроллер диалогов.
        playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Ссылка на данные игрока.
    }

    private void Update()
    {
        switch(dialogueGUIStage)//Для каждой стадии диалогов.
        {
            case GUI_DialogueStage.Begining:
                FadeOn();//Затемняем если диалог начался.
                break;
            case GUI_DialogueStage.ReadyToStart:
                FadeOff();//Убираем маску.
                showBlackStrips = true;//Отрисовываем ограничительные полоски.
                if (fadeAlpha == 0)//Если маска убрана.
                {
                    dialogueGUIStage = GUI_DialogueStage.ShowInterfaceNPCPart;//Показываем интерфейс того, что говорит собеседник.
                }
                break;
            case GUI_DialogueStage.AwaitInputsToNextLine:
                if(Input.GetKeyDown(Source_Constants.userInputs["DIALOGUE_NEXTLINE"]))//Если игрок нажал клавишу перехода этапа.
                {
                    dialogueGUIStage = GUI_DialogueStage.ShowInterfaceNPCPart;//Показываем интерфейс того, что говорит собеседник.
                    dialogueLine = "";
                }
                break;
            case GUI_DialogueStage.AwaitInputsToAnswer:
                if (Input.GetKeyDown(Source_Constants.userInputs["DIALOGUE_NEXTLINE"]))//Если игрок нажал клавишу перехода этапа.
                {
                    dialogueGUIStage = GUI_DialogueStage.ShowInterfaceAnswerPart;//Показываем интерфейс того, что говорит собеседник.
                }
                break;
        }
    }

    private void OnGUI()//Интерфейс диалогов.
    {
        GUI.color = new Vector4(0, 0, 0, fadeAlpha);//Устанавливаем цвет гуи.
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), playerData.interfaceSkin.customStyles[8].normal.background);//Рисуем маску.
        GUI.color = Color.white;//Устанавливаем стандартный цвет гуи.
        if (showBlackStrips)//Если нужно показывать ограничители экрана.
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, 75), playerData.interfaceSkin.customStyles[8].normal.background);//Рисуем верхнюю полоску.
            GUI.DrawTexture(new Rect(0, Screen.height - 75, Screen.width, 75), playerData.interfaceSkin.customStyles[8].normal.background);//Рисуем верхнюю полоску.
        }
        if(dialogueGUIStage == GUI_DialogueStage.ShowInterfaceNPCPart || dialogueGUIStage == GUI_DialogueStage.AwaitInputsToNextLine || dialogueGUIStage == GUI_DialogueStage.AwaitInputsToAnswer)//Если сейчас очередь собеседника
        {
            if (dialogueGUIStage != GUI_DialogueStage.AwaitInputsToAnswer)//Если не нужно запрашивать следующую часть фразы собеседника.
            {
                if (dialogueLine.Length == 0)//Если текст собеседника пуст.
                {
                    if (dialogueController != null)//Если диалог контроллер был найден.
                    {
                        string tempLine = dialogueController.GetNextLine();//Получаем текст фразы собеседника.
                        if (tempLine.EndsWith("@endline"))//Если текст вернулся пустым, значит текст собеседника закончился и нужно ему ответить.
                        {
                            dialogueGUIStage = GUI_DialogueStage.AwaitInputsToAnswer;//Ставим ожидание команды.
                            dialogueLine = tempLine.Substring(0, tempLine.Length - 8);//Обрезаем особые символы и выводим в интерфейс.
                        }
                        else
                        {
                            dialogueGUIStage = GUI_DialogueStage.AwaitInputsToNextLine;//Ставим ожидание команды.
                            dialogueLine = tempLine;
                        }
                    }
                }
            }
            GUI.Label(new Rect(10, Screen.height - 75, Screen.width-20, 75), dialogueLine, playerData.interfaceSkin.customStyles[9]);//Отображаем текст собеседника.
        }
        if(dialogueGUIStage == GUI_DialogueStage.ShowInterfaceAnswerPart)//Если сейчас очередь ответа.
        {
            if(dialogueAnswers.Count == 0)//Если строчек ответов нет.
            {
                dialogueAnswers = dialogueController.GetAnswers();//Запрашиваем ответы.
                if(dialogueAnswers.Count == 0)//Если количество полученных ответов = 0, завершаем диалог.
                {
                    dialogueController.FinishDialogue();//Вызываем завершение диалога.
                }
            }
            else
            {
                float buttonWidth = 500;//Длинна кнопки по умолчанию.
                for(int i = 0; i < dialogueAnswers.Count; i++)//Для каждого ответа.
                {
                    if(dialogueAnswers[i].Length > 0 && dialogueAnswers[i].Length <= 100)//Если длинна ответа до 100 символов.
                    {
                        if (buttonWidth < 350)//Если это не самый большой ответ.
                        {
                            buttonWidth = 350;//Устанавливаем длинну кнопки под этот ответ.
                        }
                    }
                    if (dialogueAnswers[i].Length > 100 && dialogueAnswers[i].Length < 200)//Если длина ответа от 100 до 200 символов.
                    {
                        if(buttonWidth < 500)//Если это не самый большой ответ.
                        {
                            buttonWidth = 500;//Устанавливаем длинну кнопки под этот ответ.
                        }
                    }
                }
                Rect lastButton = new Rect(new Rect(Screen.width - (buttonWidth+100), Screen.height / 2 - (dialogueAnswers.Count/2*100), buttonWidth, 100));
                for (int i = 0; i < dialogueAnswers.Count; i++)
                {
                        if (GUI.Button(new Rect(lastButton.x - 10, lastButton.y, lastButton.width, lastButton.height), "", playerData.interfaceSkin.customStyles[10]))//Кнопка ответа.
                        {
                            dialogueController.OnUserAnswer(i);//Отправляем ответ контроллеру диалогов.
                        }
                    if (dialogueAnswers.Count > i)
                    {
                        GUI.Label(lastButton, dialogueAnswers[i], playerData.interfaceSkin.customStyles[12]);
                    }
                        lastButton.y += lastButton.height + 20;
                }
            }
        }
    }

    public void FadeOn()//Затемнение экрана.
    {
        if (fadeAlpha <= 1 && fadeAlpha >= 0)//Если затемнение не завершено.
        {
            fadeAlpha += Time.deltaTime*3;//Начинаем показывать затемняющую маску.
        }
        fadeAlpha = fadeAlpha >= 1 ? 1 : fadeAlpha;//Если параметр за пределами допустимых значений.
    }

    public void FadeOff()//Просветеление экрана.
    {
        if (fadeAlpha <= 2 && fadeAlpha > 0)//Если просветление не завершено
        {
            fadeAlpha -= Time.deltaTime * 1;//Начинаем убирать затемняющую маску.
        }
        fadeAlpha = fadeAlpha < 0 ? 0 : fadeAlpha;//Если параметр за пределами допустимых значений.
    }
}

public enum GUI_DialogueStage { Begining, ReadyToStart, ShowInterfaceNPCPart, ShowInterfaceAnswerPart, NotInDialogue, AwaitInputsToNextLine, AwaitInputsToAnswer, FinishDialogue, ReadyToNext }
