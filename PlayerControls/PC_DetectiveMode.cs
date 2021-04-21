using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_DetectiveMode : MonoBehaviour
{
    public bool active = false;//Находимся ли в режиме детектива.

    [Header("Time data:")]
    public float cooldownTime = 60;//Перезарядка детективного режима.
    public float useTime = 10;//Сколько доступен детективный режим.

    public float useTimer = 0;//Таймер детективного режима.
    public float cooldownTimer = 0;//Таймер перезарядки.

    public bool CanActivate
    {
        get
        {
            return cooldownTimer == 0;//Если перезарядка завершилась, возвращаем истину.
        }
    }//Можно ли перейти в режим детектива.

    private UnityStandardAssets.ImageEffects.Grayscale grayScaleEffect;//Цветовой фильтр - черно белый.
    private Object_Player playerData;//Данные игрока.

    private void Start()
    {
        playerData = Camera.main.GetComponent<Object_Player>();//Получаем ссылку на данные игрока.
        grayScaleEffect = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.Grayscale>();//Скрипт маски.
    }

    private void Update()
    {
        if (cooldownTimer > 0)//Если таймер перезарядки не закончился.
        {
            cooldownTimer -= Time.deltaTime;//Отнимаем время от таймера.
        }
        cooldownTimer = cooldownTimer < 0 ? 0 : cooldownTimer;//Границы таймера перезарядки.
        useTimer = useTimer < 0 ? 0 : useTimer;//Границы таймера использования.

        if(Camera.main.GetComponent<PI_Dialogue>().dialogueGUIStage == GUI_DialogueStage.NotInDialogue)
        {
            if (Input.GetKeyDown(Source_Constants.userInputs["DETECTIVE_MODE"]))//Активация режима детектива.
            {
                if (CanActivate)//Если можем войти в режим детектива.
                {
                    active = true;//Режим детектива активирован.
                    playerData.activeWorldLayerMask = playerData.detectiveMask;//Отображаемые слои - слои детектива.
                    grayScaleEffect.enabled = true;//Включаем черно белый фильтр.
                }
            }
        }

        if(active)//Если находимся в режиме детектива.
        {
            if (useTimer > 0)//Если таймер использования не закончился.
            {
                useTimer -= Time.deltaTime;//Отнимаем время таймера.
            }
            if (useTimer <= 0)//Если время использования завершилось.
            {
                cooldownTimer = cooldownTime;//Запускаем перезарядку.
                active = false;//Выходим из режима детектива.
                useTimer = useTime;//Устанавливаем время использования.
                playerData.activeWorldLayerMask = playerData.worldMask;//Устанавливаем обычную маску мира.
                grayScaleEffect.enabled = false;//Отключаем черно/белый фильтр мира.
            }
        }
    }
}
