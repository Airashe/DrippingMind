using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_SoundMaker : MonoBehaviour
{
    public int uniqueId;//Уникальный id источника.
    public List<AudioClip> audios;
    public AudioSource source;//Источник звука.
    public AudioHighPassFilter highPassFilter;//Обрезание верхних частот.
    public bool distanceVolume = true;
    private bool playing = false;
    public bool repeat = false;//Нужно ли повторять набор звуков.
    public float volumeBorder = 1;
    public float maxVolume = 1;
    private int currSound = 0;
    public bool playWithoutInit;//Если нужно сразу проигрывать звук.
    public bool smoothChange;//Плавное изменение звука.
    public bool dontPlayOnOtherSide = false;

    private float currentClipLength;//Длинна звука.
    private float currentClipTime;
    private float currentClipTimeLeft
    {
        get
        {
            return currentClipLength - currentClipTime;
        }
    }//Сколько осталось от клипа.
    public float currentVolume = 1;

    private void Start()
    {
        source = GetComponent<AudioSource>();//Получаем ссылку на источник звука.
        highPassFilter = GetComponent<AudioHighPassFilter>();//Ссылка на обрезание верхних частот.
        if(playWithoutInit)
        {
            Play();
        }
    }

    private void Update()
    {
       if(playing)
       {
            //Debug.Log(gameObject.name + ": Проигрываем звук");
            if(smoothChange)//Если нужно плавно менять звук.
            {
                if(source.isPlaying)
                {
                    currentClipTime += Time.deltaTime;
                    if(currentClipTimeLeft < 1)
                    {
                        currentVolume = currentClipTimeLeft;
                    }
                    else
                    {
                        if (currentVolume < maxVolume)
                        {
                            currentVolume += Time.deltaTime;
                        }
                    }
                }
            }
            if(currSound < audios.Count)
            {
                //Debug.Log(gameObject.name + ": В пределах проигрывания");
                if (!source.isPlaying)//Если не играет музычка.
                {
                    //Debug.Log(gameObject.name + ": Звук не проигырвается.");
                    source.clip = audios[currSound];//Устанавливаем клип.
                    if (smoothChange)
                    {
                        currentClipLength = audios[currSound].length;//Длинна звука.\
                        currentClipTime = 0;//Сколько проигралось.
                    }
                    else
                    {
                        currentVolume = maxVolume;
                    }
                    source.Play();
                    currSound += 1;//Переход на следующий звук.
                }
            }
            else
            {
                if(!repeat)
                {
                    Stop();
                    playing = false;
                }
                else
                {
                    currSound = 0;
                }
            }
       }
    }

    public void Play()
    {
        playing = true;
    }

    public void Stop()
    {
        playing = false;
        source.Stop();
        currSound = 0;
    }
}
