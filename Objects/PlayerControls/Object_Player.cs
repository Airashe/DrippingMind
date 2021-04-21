using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Player : MonoBehaviour
{
    public Camera_State cameraState = Camera_State.Await;//Состояние камеры.
    public bool useCoordinateAxis = false;//Использовать координатную систему.
    public Object_Unit unit;//Юнит который контролирует игрок.
    public LayerMask detectiveMask;//Какие слои отображать, если в режиме детективав.
    public LayerMask worldMask;//Какие слои отображать, если в игре.
    public LayerMask interfaceMask;//Какие слои отображать, если открыт интерфейс.
    public GUISkin interfaceSkin;//Стиль интерфейса для этого игрока.
    public Source_DialogueTree dialogueTree;//Ветки диалогов для этого игрока.
    public Source_AnimatedGUI healthAnimation;//Анимация для жизней игрока.
    public Source_AnimatedGUI keyButtonBck;//Задник для кнопки с клавишей.
    public Source_AnimatedGUI detectiveCooldown;//Иконка часов.
    public Texture2D mapTexture;//Текстура карты.

    private AudioSource ingameToolTipSource;//Ссылка для воспроизведения звуков.
    private PC_CharacterController playerController;//Ссылка на управление игрока.
    public bool showCursor = true;//Нужно ли отрисовывать курсор.
    public bool showCursorWithoutCheck = false;//Отрисовывать курсор независимо от активного меню игрока.

    public float ingameTooltipAlpha = 0;//Прозрачность подсказки.
    public string ingameToolTipText = "";//Текст подсказки.
    public float ingameToolTipTimer = 0;//Сколько будет подсказка видна.

    public LayerMask activeWorldLayerMask;//Активная маска мира.

    public void ShowIngameToolTip(string text, float time)
    {
        ingameTooltipAlpha = 1;
        ingameToolTipText = text;
        ingameToolTipTimer = time;
        if (ingameToolTipSource != null)
        {
            ingameToolTipSource.Play();
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);//Не уничтожать между сценами.
    }

    private void Start()
    {
        playerController = Camera.main.GetComponent<PC_CharacterController>();//Ссылка на контроллер управления.
        ingameToolTipSource = transform.Find("SoundMaker_IngameToolTip").GetComponent<AudioSource>();//Ссылка на вопроизведение звуков подсказок.
        activeWorldLayerMask = worldMask;//Устанавливаем нормальный список слоев.
    }

    private void Update()
    {
        Camera.main.cullingMask = activeWorldLayerMask;
    }

    private void OnGUI()
    {
        if (showCursor)//Если нужно отрисовывать курсор.
        {
            if (!playerController.enabled || showCursorWithoutCheck)//Нужно ли отображать курсор в зависимости от этапа игры.
            {
                GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 8, 12), interfaceSkin.customStyles[20].normal.background);
            }
        }
        if (ingameToolTipTimer > 0)//Если есть время для показа подсказкию
        {
            ingameToolTipTimer -= Time.deltaTime;
        }
        else
        {
            if (ingameTooltipAlpha > 0)
            {
                ingameTooltipAlpha -= Time.deltaTime;
            }
        }

        GUI.color = new Vector4(1, 1, 1, ingameTooltipAlpha);
        GUI.DrawTexture(new Rect(10, 10, 250, 50), interfaceSkin.customStyles[21].hover.background);
        GUI.DrawTexture(new Rect(20, 20, 30, 30), interfaceSkin.customStyles[21].normal.background);
        GUI.Label(new Rect(60, 20, 190, 40), ingameToolTipText, interfaceSkin.customStyles[22]);
        GUI.color = Color.white;
    }
}
