using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source_AnimationController
{
    private Source_AnimatedGUI animation;//Анимация которая будет проигрываться.
    private int currentAnimationFrame;//Текущий фрейм.
    private float animationTimer = 0;//Таймер анимации.
    private float animationTimer_nextTick = 0;//Следущий тик таймера анимации.

    public float animationSpeed = 1;//Скорость анимации.

    public Texture2D CurrentFrame
    {
        get
        {
            if(currentAnimationFrame < animation.frames.Count && animation.frames.Count > 0)
            {
                return animation.frames[currentAnimationFrame];//Возвращаем конкретный фрейм.
            }
            return null;//Возвращаем конкретный фрейм.
        }
    }

    public void Start()//Установка первого кадра.
    {
        if(animation != null)//Если есть анимация
        {
            if (animation.frames.Count > 0)//Если есть кадры.
            {
                currentAnimationFrame = 0;//Устанавливаем первый кадр.
            }
        }
    }

    public void Play()//Апдейт вызываемый из MonoBehaviour.
    {
        if (animation != null)//Если анимация есть.
        {
            animationTimer += Time.deltaTime;//Ведем счетчик таймера.

            if (animation.frames.Count > 1)//Если есть кадры анимации.
            {
                if (animationTimer >= animationTimer_nextTick)//Если таймер основной анимации прошел.
                {
                    animationTimer_nextTick = animationTimer + (1 / animationSpeed);//Устанавливаем следующий тик таймера.
                    currentAnimationFrame += 1;//Прибавляем счетчик кадров.
                    if (currentAnimationFrame >= animation.frames.Count)//Если счетчик кадров вышел за пределы возможных кадров.
                    {
                        currentAnimationFrame = 0;//Устанавливаем первый кадр.
                    }
                }
            }
        }
    }

    public Source_AnimationController(Source_AnimatedGUI animation, float animationSpeed, bool startAfterInit)
    {
        this.animation = animation;
        this.animationSpeed = animationSpeed;
        if(startAfterInit)
        {
            Start();
        }
    }
}
