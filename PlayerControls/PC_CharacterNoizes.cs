using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_CharacterNoizes : MonoBehaviour
{
    private PI_IgInventory inventoryInterface;
    private Object_Player playerData;//Данные игрока.
    private PC_SoundsController soundsController;

    public AudioSource inventoryTabClick;//Кликанье по вкладкам инвентаря.

    public AudioSource breathSource;//Объект дыхания.
    public AudioSource heartSource;//Объект сердцебиения.

    public List<AudioClip> breathSounds;//Звуки спокойного дыхания.
    public List<AudioClip> heartSounds;//Звуки сердца.

    public float breathDelayNormal = 1.5f;//Нормальная задержка дыхания.
    public float heartDelayNormal = 1.5f;//Нормальная задержка сердца.
    public float currentBreathDelay
    {
        get
        {
            if(playerData.unit != null)
            {
                return breathDelayNormal - ((1 - (float)playerData.unit.currentHealth / (float)playerData.unit.MaxHealth)*1);//Изменение частоты дыхания.
            }
            return breathDelayNormal;
        }
    }//Текущая задержка, в зависимости от хп.
    public float currentHeartDelay
    {
        get
        {
            if(playerData.unit != null)
            {
                return heartDelayNormal - ((1 - (float)playerData.unit.currentHealth / (float)playerData.unit.MaxHealth) * 1);//Изменение частоты биения.
            }
            return heartDelayNormal;
        }
    }//Текущая задержка взависимости от хп.

    public float breathTimer = 0;//Таймер для дыхания.
    public float heartTimer = 0;//Таймер для сердца.

    private void Start()
    {
        playerData = Camera.main.GetComponent<Object_Player>();//Объект игрока.
        inventoryInterface = Camera.main.GetComponent<PI_IgInventory>();//Ссылка на инвентарь юнита.
        soundsController = Camera.main.GetComponent<PC_SoundsController>();//Ссылки на контроллер звуков.
    }

    private void Update()
    {
        if (soundsController.avaibleCharacterSound)//Если можно проигрывать звуки юнита.
        {
            if (playerData.unit != null)//Если есть юнит игрока.
            {
                if (playerData.unit.state != Property_UnitState.Dead)//Если юнит жив.
                {
                    if (!inventoryInterface.Active)
                    {
                        if (breathSounds.Count > 0)//Если есть звуки дыхания.
                        {
                            if (breathSource != null)
                            {
                                if (!breathSource.isPlaying)//Если звук не проигрывается.
                                {
                                    if (breathTimer > 0)
                                    {
                                        breathTimer -= Time.deltaTime;
                                    }
                                    else
                                    {
                                        breathSource.clip = breathSounds[Random.Range(0, breathSounds.Count - 1)];//Выбираем случайное дыхание.
                                        breathSource.Play();
                                        breathTimer = currentBreathDelay;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (heartSounds.Count > 0)//Если есть звуки сердца.
                        {
                            if (heartSource != null)
                            {
                                if (!heartSource.isPlaying)
                                {
                                    if (heartTimer > 0)
                                    {
                                        heartTimer -= Time.deltaTime;
                                    }
                                    else
                                    {
                                        heartSource.clip = heartSounds[Random.Range(0, heartSounds.Count - 1)];
                                        heartSource.Play();
                                        heartTimer = currentHeartDelay;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Play_InventorySoundEffect(AudioClip audioClip, float volume)
    {
        inventoryTabClick.clip = audioClip;
        inventoryTabClick.volume = volume;
        inventoryTabClick.Play();
    }
}
