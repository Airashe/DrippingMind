using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PI_IngameGUI : MonoBehaviour
{
    //Ссылки на различные объекты.
    private Object_Player playerData;//Ссылка на данные игрока.
    private PC_MainUseLogic mainUseLogic;//Ссылка на контролле взаимодействия.
    private PI_IgInventory inventoryInterface;//Ссылка на инвентарь.
    private PI_Dialogue dialogueInterface;//Ссылка на диалог.

    private List<Object_Unit> speakingUnits = new List<Object_Unit>();//Список говорящих юнитов.

    //Прозрачность черной маски.
    public bool fadeState = false;
    public float fadeAlpha = 1;//Основной цвет маски затемнения.

    //Прозрачность светлой маски.
    public bool whiteState = false;
    public float whiteAlpha = 0;//Основной цвет маски осветления.

    //Прозрачность накладываемой текстуры.
    public bool textureState = false;
    public float textureAlpha = 0;//Основной цвет маски текстуры.
    public Texture2D textureSplash;//Отображаемая текстура.

    public Source_AnimationController pressBtnController;
    public Source_AnimationController detectiveCdController;//Контроллер анимации сердца.

    private void Start()
    {
        playerData = Camera.main.GetComponent<Object_Player>();//Получаем ссылку на данные игрока.
        mainUseLogic = Camera.main.GetComponent<PC_MainUseLogic>();//Ссылка на основную логику.
        dialogueInterface = Camera.main.GetComponent<PI_Dialogue>();//Ссылка на контроллер интерфейса диалогов.
        inventoryInterface = Camera.main.GetComponent<PI_IgInventory>();//Ссылка на инвертарь.
        pressBtnController = new Source_AnimationController(playerData.keyButtonBck, 25, true);
        detectiveCdController = new Source_AnimationController(playerData.detectiveCooldown, 1, true);
        Cursor.visible = false;//Скрываем курсор мыши.
    }

    private void Update()
    {
            if (textureState)
            {
                TextureOn();
            }
            else
            {
                TextureOff();
            }
        if (whiteState)
        {
            WhiteOn();
        }
        else
        {
            WhiteOff();
        }
        if(fadeState)
        {
            FadeOn();
        }
        else
        {
            FadeOff();
        }
        if(detectiveCdController != null)
        {
            detectiveCdController.Play();
        }
        if (pressBtnController != null)//Если есть контроллер анимации.
        {
            pressBtnController.Play();//Проигрываем анимацию сердца.
        }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                mainUseLogic.selectedItem = (mainUseLogic.selectedItem + 1);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                mainUseLogic.selectedItem = (mainUseLogic.selectedItem - 1);
            }
        mainUseLogic.selectedItem = mainUseLogic.selectedItem < 0 ? 0 : mainUseLogic.selectedItem >= mainUseLogic.useList.Count ? mainUseLogic.useList.Count - 1 : mainUseLogic.selectedItem;
    }

    private void OnGUI()
    {
        GUI.color = new Vector4(1, 1, 1, whiteAlpha);//Цвет белой маски.
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), playerData.interfaceSkin.customStyles[8].hover.background);//Рисуем осветление.
        GUI.color = new Vector4(0, 0, 0, fadeAlpha);//Устанавливаем цвет гуи.
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), playerData.interfaceSkin.customStyles[8].normal.background);//Рисуем маску.
        GUI.color = new Vector4(1, 1, 1, textureAlpha);//Цвет текстуры.
        if(textureSplash != null)//Если есть текстура.
        {
            Rect textureRect = new Rect((Screen.width/2)-((Screen.width * 1200) / 1920)/2, 0, (Screen.width * 1200) / 1920, Screen.height);
            GUI.DrawTexture(textureRect, textureSplash);//Рисуем текстуру.
        }
        GUI.color = Color.white;//Устанавливаем стандартный цвет гуи.
        if (speakingUnits.Count > 0)//Если есть говорящие юниты.
        {
            DrawUnitQuotes();//Выполняем отрисовку фраз.
        }
        if(detectiveCdController != null)
        {
            if(Camera.main.GetComponent<PC_DetectiveMode>().enabled)
            {
                if (Camera.main.GetComponent<PC_DetectiveMode>().cooldownTimer > 0)//Если идет перезарядка.
                {
                    GUI.DrawTexture(new Rect(Screen.width - 42, Screen.height - 46, 37, 41), detectiveCdController.CurrentFrame);
                }
            }
        }
        else
        {
            detectiveCdController = new Source_AnimationController(playerData.detectiveCooldown, 1, true);
        }
        if(pressBtnController != null)
        {
            if(mainUseLogic.useList.Count > 0)
            {
                Rect toolTipPosition = new Rect(Screen.width / 2 - 90, Screen.height / 2 + Screen.height / 4 - 60, 50, 20);
                if (mainUseLogic.selectedItem -1 >= 0 && mainUseLogic.selectedItem - 1 < mainUseLogic.useList.Count)
                {
                    GUI.color = new Vector4(1, 1, 1, 0.5f);
                    GUI.Label(toolTipPosition, "Press ", playerData.interfaceSkin.customStyles[19]);//Начало предложения.
                    toolTipPosition.x += 50;//Смещаем положение.
                    toolTipPosition.x += 25;
                    toolTipPosition.width = 20;
                    GUI.Label(toolTipPosition, " " + mainUseLogic.ToolTipForPosition(mainUseLogic.selectedItem - 1), playerData.interfaceSkin.customStyles[19]);//Для.
                    GUI.color = Color.white;
                }
                if(mainUseLogic.selectedItem >= 0 && mainUseLogic.selectedItem < mainUseLogic.useList.Count)
                {
                    toolTipPosition = new Rect(Screen.width / 2 - 100, Screen.height / 2 + Screen.height / 4 - 30, 50, 30);
                    GUI.Label(toolTipPosition, "Press ", playerData.interfaceSkin.customStyles[17]);//Начало предложения.
                    toolTipPosition.x += 50;//Смещаем положение.
                    toolTipPosition.width = 30;
                    toolTipPosition.height = 30;
                    GUI.DrawTexture(toolTipPosition, pressBtnController.CurrentFrame);
                    GUI.Label(toolTipPosition, Source_Constants.userInputs[mainUseLogic.useKeyCodeName].ToString(), playerData.interfaceSkin.customStyles[17]);
                    toolTipPosition.x += 35;
                    toolTipPosition.width = 20;
                    GUI.Label(toolTipPosition, " " + mainUseLogic.ToolTip , playerData.interfaceSkin.customStyles[18]);//Для.
                }
                toolTipPosition = new Rect(Screen.width / 2 - 90, Screen.height / 2 + Screen.height / 4 + 20, 50, 20);
                if (mainUseLogic.selectedItem + 1 >= 0 && mainUseLogic.selectedItem + 1 < mainUseLogic.useList.Count)
                {
                    GUI.color = new Vector4(1, 1, 1, 0.5f);
                    GUI.Label(toolTipPosition, "Press ", playerData.interfaceSkin.customStyles[19]);//Начало предложения.
                    toolTipPosition.x += 50;//Смещаем положение.
                    toolTipPosition.x += 25;
                    toolTipPosition.width = 20;
                    GUI.Label(toolTipPosition, " " + mainUseLogic.ToolTipForPosition(mainUseLogic.selectedItem + 1), playerData.interfaceSkin.customStyles[19]);//Для.
                    GUI.color = Color.white;
                }
            }
        }
        else
        {
            pressBtnController = new Source_AnimationController(playerData.keyButtonBck, 25, true);
        }
    }

    private void DrawUnitQuotes()//Отрисовка фраз юнитов.
    {
        foreach (Object_Unit unit in speakingUnits)//Для каждого юнита в списке отслеживаемых говорящих юнитов.
        {
            if(unit.say_quote != null)//Если есть фраза для юнита.
            {
                if (unit.say_lineTimer > 0)//Если таймер еще не истек.
                {
                    Vector3 quoteWorldPosition = unit.transform.position;//Позиция фразы в игровом мире.
                    quoteWorldPosition.y += 0.5f;

                    Vector3 unitQuotePosition = Camera.main.WorldToScreenPoint(quoteWorldPosition);//Определяем позицию фразы, для юнита.
                    
                    
                    GUI.Label(new Rect(unitQuotePosition.x - 90, Screen.height - (unitQuotePosition.y - 15), 180, 30), unit.say_quote.line, playerData.interfaceSkin.customStyles[11]);//Рисуем текст юнита.
                }
            }
        }
    }

    public void AddSayTrackForUnit(Object_Unit trackUnit)
    {
        speakingUnits.Add(trackUnit);//Добавляем юнит к списку отслеживаемых.
    }

    public void SetTexture(Texture2D newTexture)
    {
        textureSplash = newTexture;
    }

    public void TextureOn()
    {
        if (textureAlpha <= 1 && textureAlpha >= 0)//Если затемнение не завершено.
        {
            textureAlpha += Time.deltaTime * 1;//Начинаем показывать затемняющую маску.
        }
        textureAlpha = textureAlpha >= 1 ? 1 : textureAlpha;//Если параметр за пределами допустимых значений.
    }

    public void TextureOff()//Просветеление экрана.
    {
        if (textureAlpha <= 2 && textureAlpha > 0)//Если просветление не завершено
        {
            textureAlpha -= Time.deltaTime * 1;//Начинаем убирать затемняющую маску.
        }
        textureAlpha = textureAlpha < 0 ? 0 : textureAlpha;//Если параметр за пределами допустимых значений.
    }

    public void WhiteOn()
    {
        if (whiteAlpha <= 1 && whiteAlpha >= 0)//Если затемнение не завершено.
        {
            whiteAlpha += Time.deltaTime * 3;//Начинаем показывать затемняющую маску.
        }
        whiteAlpha = whiteAlpha >= 1 ? 1 : whiteAlpha;//Если параметр за пределами допустимых значений.
    }

    public void WhiteOff()//Просветеление экрана.
    {
        if (whiteAlpha <= 2 && whiteAlpha > 0)//Если просветление не завершено
        {
            whiteAlpha -= Time.deltaTime * 1;//Начинаем убирать затемняющую маску.
        }
        whiteAlpha = whiteAlpha < 0 ? 0 : whiteAlpha;//Если параметр за пределами допустимых значений.
    }

    public void FadeOn()//Затемнение экрана.
    {
        if (fadeAlpha <= 1 && fadeAlpha >= 0)//Если затемнение не завершено.
        {
            fadeAlpha += Time.deltaTime * 3;//Начинаем показывать затемняющую маску.
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
