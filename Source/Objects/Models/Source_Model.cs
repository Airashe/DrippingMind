using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Объект моделей, содержит все данные необходимые для ренедера.

[CreateAssetMenu(fileName = "New model", menuName = "Dripping Mind/Данные модели")]
public class Source_Model : ScriptableObject
{
    public new string name;//Имя данных модели.
    public Source_Model_RenderType renderType;//Тип рендера модели.
    public List<Property_ModelsAnimation> animations;//Настройки анимаций модели.

    public Material GetAnimationMaterial(int animIndex, Source_Model_Part part, Source_Model_Rotation rotation)//Получаем материал для анимаций.
    {
        Material currentMaterial = null;//Текущий материал.

        if (animIndex >= 0 && animIndex < animations.Count)//Если номер анимации валиден.
        {
            //Проверка валидности.
            if (part == Source_Model_Part.Main)//Для основного списка кадров.
            {
                switch (rotation)
                {
                    case Source_Model_Rotation.Front:
                        currentMaterial = animations[animIndex].main.materials.front;
                        break;
                    case Source_Model_Rotation.FrontLeft:
                        currentMaterial = animations[animIndex].main.materials.frontLeft;
                        break;
                    case Source_Model_Rotation.FrontRight:
                        currentMaterial = animations[animIndex].main.materials.frontRight;
                        break;
                    case Source_Model_Rotation.Left:
                        currentMaterial = animations[animIndex].main.materials.left;
                        break;
                    case Source_Model_Rotation.Right:
                        currentMaterial = animations[animIndex].main.materials.right;
                        break;
                    case Source_Model_Rotation.Back:
                        currentMaterial = animations[animIndex].main.materials.back;
                        break;
                    case Source_Model_Rotation.BackLeft:
                        currentMaterial = animations[animIndex].main.materials.backLeft;
                        break;
                    case Source_Model_Rotation.BackRight:
                        currentMaterial = animations[animIndex].main.materials.backRight;
                        break;
                }
            }
            else//Для дополнительного списка кадров.
            {
                switch (rotation)
                {
                    case Source_Model_Rotation.Front:
                        currentMaterial = animations[animIndex].second.materials.front;
                        break;
                    case Source_Model_Rotation.FrontLeft:
                        currentMaterial = animations[animIndex].second.materials.frontLeft;
                        break;
                    case Source_Model_Rotation.FrontRight:
                        currentMaterial = animations[animIndex].second.materials.frontRight;
                        break;
                    case Source_Model_Rotation.Left:
                        currentMaterial = animations[animIndex].second.materials.left;
                        break;
                    case Source_Model_Rotation.Right:
                        currentMaterial = animations[animIndex].second.materials.right;
                        break;
                    case Source_Model_Rotation.Back:
                        currentMaterial = animations[animIndex].second.materials.back;
                        break;
                    case Source_Model_Rotation.BackLeft:
                        currentMaterial = animations[animIndex].second.materials.backLeft;
                        break;
                    case Source_Model_Rotation.BackRight:
                        currentMaterial = animations[animIndex].second.materials.backRight;
                        break;
                }
            }
        }

        return currentMaterial;
    }

    public List<Sprite> GetAnimationFrames(int animIndex, Source_Model_Part part, Source_Model_Rotation rotation)//Получаем конкретный кадр анимации.
    {
        List<Sprite> currentFramesList = new List<Sprite>();//Текущий список кадров.
        if (animIndex >= 0 && animIndex < animations.Count)//Если номер анимации валиден.
        {
            //Проверка валидности.
            if(part == Source_Model_Part.Main)//Для основного списка кадров.
            {
                switch(rotation)
                {
                    case Source_Model_Rotation.Front:
                        currentFramesList = animations[animIndex].main.frames.front;
                        break;
                    case Source_Model_Rotation.FrontLeft:
                        currentFramesList = animations[animIndex].main.frames.frontLeft;
                        break;
                    case Source_Model_Rotation.FrontRight:
                        currentFramesList = animations[animIndex].main.frames.frontRight;
                        break;
                    case Source_Model_Rotation.Left:
                        currentFramesList = animations[animIndex].main.frames.left;
                        break;
                    case Source_Model_Rotation.Right:
                        currentFramesList = animations[animIndex].main.frames.right;
                        break;
                    case Source_Model_Rotation.Back:
                        currentFramesList = animations[animIndex].main.frames.back;
                        break;
                    case Source_Model_Rotation.BackLeft:
                        currentFramesList = animations[animIndex].main.frames.backLeft;
                        break;
                    case Source_Model_Rotation.BackRight:
                        currentFramesList = animations[animIndex].main.frames.backRight;
                        break;
                }
            }
            else//Для дополнительного списка кадров.
            {
                switch (rotation)
                {
                    case Source_Model_Rotation.Front:
                        currentFramesList = animations[animIndex].second.frames.front;
                        break;
                    case Source_Model_Rotation.FrontLeft:
                        currentFramesList = animations[animIndex].second.frames.frontLeft;
                        break;
                    case Source_Model_Rotation.FrontRight:
                        currentFramesList = animations[animIndex].second.frames.frontRight;
                        break;
                    case Source_Model_Rotation.Left:
                        currentFramesList = animations[animIndex].second.frames.left;
                        break;
                    case Source_Model_Rotation.Right:
                        currentFramesList = animations[animIndex].second.frames.right;
                        break;
                    case Source_Model_Rotation.Back:
                        currentFramesList = animations[animIndex].second.frames.back;
                        break;
                    case Source_Model_Rotation.BackLeft:
                        currentFramesList = animations[animIndex].second.frames.backLeft;
                        break;
                    case Source_Model_Rotation.BackRight:
                        currentFramesList = animations[animIndex].second.frames.backRight;
                        break;
                }
            }
        }
        return currentFramesList;//Возвращаем нужный список кадров.
    }

    public int GetAnimationPosByName(string animationName)//Получаем позицию анимации в списке всех анимаций.
    {
        for(int index = 0; index < animations.Count; index++)//Для каждой анимации в списке анимаций.
        {
            if(animations[index].name == animationName)//Если имя анимации равно искомой анимации.
            {
                return index;//Возвращаем позицию анимации в списке.
            }
        }
        return -1;//Если анимация не была найдена, возвращаем отрицательную позицию.
    }
}

public enum Source_Model_Part { Main, Second }//Виды частей модели.
public enum Source_Model_RenderType {Multiple, Single }//Виды рендера моделей.
public enum Source_Model_Rotation { Front, FrontLeft, FrontRight, Left, Right, Back, BackLeft, BackRight };//Возможный угол отрисовки.
