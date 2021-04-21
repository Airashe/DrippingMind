using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_SoundsController : MonoBehaviour
{
    //Ссылки на объекты.
    private Source_GameManager gameManager;//Ссылка на гейм менеджер.
    private Object_Player playerData;//Данные игрока.
    private PI_IgInventory inventoryInterface;
    //private PC_CharacterNoizes characterNoizes;//Звуки игрока.

    public AudioSource backgroundMusicSource;//Источник для фоновой музыки.
    public GameObject soundRegions;//Объект окружающих звуков.
    public List<Property_RegionData> regionsDatas = new List<Property_RegionData>();//Источники звука для регионов.
    public LayerMask checkingLayers;
    public bool avaibleIngameSounds;//Доступность внутриигроых звуков.
    public bool avaibleCharacterSound;//Доступность звуков персонажа.

    private bool stopBackgroundMusic;//Остановка фоновой музыки.

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Получаем ссылку на объект гейм менеджера.
        playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Ссылка на данные игрока.
        inventoryInterface = Camera.main.gameObject.GetComponent<PI_IgInventory>();//Ссылка на инвентарь
        //characterNoizes = Camera.main.gameObject.GetComponent<PC_CharacterNoizes>();//Ссылка на скрипт звуков игрока.
    }

    private void Update()
    {
        if (avaibleIngameSounds)
        {
            if (playerData.unit != null)//Если есть юнит.
            {
                foreach (Object_SoundMaker soundMaker in gameManager.soundMakers)//Для каждого источника звука.
                {
                    if (soundMaker != null)
                    {
                        if (soundMaker.source != null)
                        {
                            if (soundMaker.source.isPlaying && soundMaker.distanceVolume)//Проигрывается ли звук.
                            {
                                if (!inventoryInterface.Active)
                                {
                                    bool filterEnabled = false;
                                    RaycastHit soundCheckerHit;//Объект попадания.
                                    Debug.DrawLine(playerData.unit.position, soundMaker.transform.position, Color.blue);
                                    if (Physics.Linecast(playerData.unit.position, soundMaker.transform.position, out soundCheckerHit))
                                    {
                                        if (checkingLayers == (checkingLayers | (1 << soundCheckerHit.collider.gameObject.layer)))//Если слои объекта и коллекции совпадают.
                                        {
                                            Debug.DrawLine(playerData.unit.position, soundMaker.transform.position, Color.red);
                                            filterEnabled = true;
                                        }
                                        else
                                        {
                                            Debug.DrawLine(playerData.unit.position, soundMaker.transform.position, Color.green);
                                        }
                                    }
                                    soundMaker.highPassFilter.enabled = filterEnabled;
                                    float distanceTorwardsPlayerUnit = Vector3.Distance(soundMaker.transform.position, playerData.unit.position) / Source_Constants.CONST_METRIC_WORLD_DISNTACE_IN_UNITS;//Дистанция от игрока до юнита.
                                    soundMaker.source.volume = (soundMaker.currentVolume / Mathf.Pow(distanceTorwardsPlayerUnit, 2));
                                    if (soundMaker.source.volume > soundMaker.volumeBorder)
                                    {
                                        soundMaker.source.volume = soundMaker.volumeBorder;
                                    }
                                    if (soundMaker.dontPlayOnOtherSide)
                                    {
                                        if (filterEnabled)
                                        {
                                            soundMaker.source.volume = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    soundMaker.source.volume = 0;
                                }
                            }
                        }
                    }
                }//Для каждого источника звука.

                foreach (Object_SoundRegion soundRegion in gameManager.soundRegions)//Для каждого региона со звуком.
                {
                    if (!inventoryInterface.Active)//Если не в интерфейсе.
                    {
                        if (soundRegion.regionSound != null)//Если есть звук.
                        {
                            //Debug.Log(soundRegion.name + ": Region with sound");
                            if (soundRegion.PlayerInRegion(playerData.unit.position))//Если игрок дотягивается до предмета.
                            {
                                //Debug.Log(soundRegion.name + "Unit in range");
                                if (!soundRegion.playing)//Если звук не играет.
                                {

                                    AudioSource regionSource = soundRegions.AddComponent<AudioSource>();//Добавляем копонент звука.
                                    regionSource.volume = soundRegion.volume;//Устанавливаем громкость звука.
                                    regionSource.clip = soundRegion.regionSound;//Устанавливаем звук.
                                    regionSource.loop = soundRegion.loop;
                                    regionSource.Play();
                                    regionsDatas.Add(new Property_RegionData(soundRegion, regionSource));//Добавляем данные.
                                    soundRegion.playing = true;
                                }
                                else
                                {
                                    foreach (Property_RegionData regionData in regionsDatas)
                                    {
                                        if (regionData != null)
                                        {
                                            if (regionData.region == soundRegion)
                                            {
                                                regionData.source.volume = soundRegion.volume;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Debug.Log(soundRegion.name + " :Outside of sound region.");
                                if (soundRegion.playing)
                                {
                                    //Debug.Log(soundRegion.name + " :Sound region is playing.");
                                    foreach (Property_RegionData regionData in regionsDatas)
                                    {
                                        if (regionData != null)
                                        {
                                            if (regionData.region == soundRegion)
                                            {
                                                if (regionData.deadTime > 0)
                                                {
                                                    regionData.deadTime -= Time.deltaTime;
                                                    regionData.source.volume = regionData.deadTime;
                                                }
                                                else
                                                {
                                                    Debug.Log(soundRegion.name + " :Remove region source.");
                                                    Destroy(regionData.source);
                                                    regionsDatas.Remove(regionData);
                                                    soundRegion.playing = false;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            regionsDatas.Remove(regionData);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Property_RegionData regionData in regionsDatas)
                        {
                            if (regionData != null)
                            {
                                if (regionData.region == soundRegion)
                                {
                                    regionData.source.volume = 0;
                                }
                            }
                        }
                    }
                }

            }
        }
        else
        {
            foreach (Object_SoundMaker soundMaker in gameManager.soundMakers)//Для каждого источника звука.
            {
                if(soundMaker.source.isPlaying)
                {
                    soundMaker.source.volume = 0;
                }
            }
        }

        if(stopBackgroundMusic)//Если идет остановка фоновой музыки.
        {
            if(backgroundMusicSource.volume > 0)
            {
                backgroundMusicSource.volume -= Time.deltaTime;
            }
            else
            {
                backgroundMusicSource.volume = 0;
                backgroundMusicSource.Stop();
                backgroundMusicSource.clip = null;
                backgroundMusicSource.loop = false;
                stopBackgroundMusic = false;
            }
        }
    }

    public void StopBackgroundMusic()
    {
        stopBackgroundMusic = true;
    }

    public void PlayBackgroundMusic(AudioClip audioClip, float Volume, bool loop)
    {
        stopBackgroundMusic = false;
        backgroundMusicSource.clip = audioClip;
        backgroundMusicSource.volume = Volume;
        backgroundMusicSource.loop = loop;
        backgroundMusicSource.Play();
    }
}
