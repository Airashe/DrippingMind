using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Рендерер моделей.

public class Object_Model : MonoBehaviour
{
    public Source_Model modelData;//Данные о модели, которая будет рендериться.

    public Vector3 lookDirection = Vector3.back;//Куда смотрит модель.
    public Vector3 parentRight = Vector3.right;//Правая сторона от родителя.

    [HideInInspector]
    public SpriteRenderer mainRenderer;//Рендерер основного спрайта.
    [HideInInspector]
    public SpriteRenderer secondRenderer;//Рендерер дополнительного спрайта.

    private int animationDataIndex = -1;//Позиция текущей анимации в списке данных модели.
    private float animationTimer = 0;//Таймер смены кадра текущей анимации.

    private float mainAnimationTimer_NextTick = 0;//Когда нужно менять кадр текущей анимации.
    private int mainAnimationFrameIndex = 0;//Отображаемый кадр текущей анимации.
    public float mainAnimationSpeed = 10;//Скорость проигрывания текущей анимации.

    private float secondAnimationTimer_NextTick = 0;//Когда нужно менять кадр текущей дополнительной анимации.
    private int secondAnimationFrameIndex = 0;//Отображаемый кадр текущей дополнительной анимации.
    public float secondAnimationSpeed = 10;//Скорость проигрывания текущей дополнительной анимации.

    public bool rotateToCamera = true;//Нужно ли поворачивать модель к камере.


    public bool isCurrentMainAnimEnded = false;//Закончена ли анимация для основного рендера.

    private void Awake()//Перед началом сцены.
    {
        mainRenderer = transform.Find("MainRenderer").GetComponent<SpriteRenderer>();//Получаем ссылку на основной рендер.
        secondRenderer = transform.Find("SecondRenderer").GetComponent<SpriteRenderer>();//Получаем ссылку на дополнительный рендер.
        if(modelData != null)//Если модель поставлена заранее.
        {
            secondRenderer.gameObject.SetActive(modelData.renderType == Source_Model_RenderType.Multiple);//Если нужно обрабатывать несколько анимаций, то включаем дополнительный рендерер.
        }
    }

    private void Update()//Выполнять каждый кадр.
    {
        if (Camera.main != null)//Если есть основная камера.
        {
            RenderModelAnimation();//Рендерим анимации.
        }
       // Debug.DrawRay(transform.position, lookDirection, Color.red);
    }

    private void LateUpdate()//Последние действия в кадре.
    {
        if (Camera.main != null)//Если есть основная камера.
        {
            if (rotateToCamera)
            {
                transform.LookAt(Camera.main.transform);//Поворачиваем модель к камере.
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);//Лочим повороты по оси x.
            }
        }
    }

    public void ChangeRotationMode(bool newRotationMode, Vector3 fixedLoodDirection)
    {
        if(!newRotationMode && rotateToCamera)//Если нужно выключить поворот к камере и поворот к камере включен.
        {
            transform.LookAt(transform.position + (lookDirection*5));//Поворачиваем модель в сторону куда она смотрит.
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);//Лочим повороты по оси x.
        }
        rotateToCamera = newRotationMode;//Нужно ли поворачивать модель к камере.
        lookDirection = fixedLoodDirection;//Поворачиваем юнита к камере.
    }

    public void SetLookDirection(Vector3 newLookDirection)//Изменение направления взгляда модели.
    {
        lookDirection = newLookDirection;//Изменяем параметр.
    }

    public void SetModel(Source_Model newModelData, string animationName)//Изменение активной модели и изменение анимации.
    {
        if(modelData != newModelData)//Если это не та же самая моделька.
        {
            modelData = newModelData;//Изменяем данные модели.
            if (modelData != null)//Если модель поставлена заранее.
            {
                secondRenderer.gameObject.SetActive(modelData.renderType == Source_Model_RenderType.Multiple);//Если нужно обрабатывать несколько анимаций, то включаем дополнительный рендерер.
            }
            SetAnimation(animationName);//Установка новой анимации.
        }
        
    }

    public void SetAnimation(string animationName)//Смена проигрываемой анимации.
    {
        if(modelData != null)//Если есть данные модели.
        {
            if (animationDataIndex != -1)
            {
                if (modelData.animations[animationDataIndex].name != animationName)//Если анимация уже не проигрывается.
                {
                    animationDataIndex = modelData.GetAnimationPosByName(animationName);//Находим позицию необходимой анимации.
                    mainAnimationFrameIndex = 0;//Обнуляем счетчик кадров основной анимации.
                    secondAnimationFrameIndex = 0;//Обнуляем счетчик кадров дополнительной анимации.
                }
            }
            else
            {
                animationDataIndex = modelData.GetAnimationPosByName(animationName);//Находим позицию необходимой анимации.
                mainAnimationFrameIndex = 0;//Обнуляем счетчик кадров основной анимации.
                secondAnimationFrameIndex = 0;//Обнуляем счетчик кадров дополнительной анимации.
            }
        }
    }

    public void RenderModelAnimation()//Рендер конкретного кадра, конкретной анимации.
    {
        if (modelData != null)//Если есть модель, которую нужно отрисовывать.
        {
            animationTimer += Time.deltaTime;//Ведем счетчик таймера.
            Source_Model_Rotation currentRotation = GetCameraAngle();//Какую сторону нужно отрисовывать.
            
            List<Sprite> framesList = modelData.GetAnimationFrames(animationDataIndex, Source_Model_Part.Main, currentRotation);//Получаем список фреймов для текущего поворота.
            Material framesMaterial = modelData.GetAnimationMaterial(animationDataIndex, Source_Model_Part.Main, currentRotation);//Получаем материал для текущего поворота.

            if (framesList.Count > 0)//Если есть фреймы.
            {
                if (mainAnimationFrameIndex < framesList.Count)//Если не за пределами анимации.
                {
                    mainRenderer.sprite = framesList[mainAnimationFrameIndex];//Устанавливаем фрейм из списка в основной рендерер.
                    mainRenderer.material = framesMaterial;//Устанавливаем материал.
                }
                else
                {
                    mainRenderer.sprite = framesList[framesList.Count - 1];//Устанавливаем фрейм из списка в основной рендерер.
                    mainRenderer.material = framesMaterial;//Устанавливаем материал.
                }
                if(mainAnimationFrameIndex == framesList.Count - 1)
                {
                    isCurrentMainAnimEnded = true;
                    if (modelData.animations[animationDataIndex].endAnimationEvents != null)
                    {
                        if (modelData.animations[animationDataIndex].endAnimationEvents.Length > 0)//Если есть события которые запускаются после окончания анимации.
                        {
                            GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(modelData.animations[animationDataIndex].endAnimationEvents);//Запускаем события.
                        }
                    }
                }
                else
                {
                    isCurrentMainAnimEnded = false;
                }
            }
            if (framesList.Count > 1)//Либо спрайт не установлен, либо кадров больше чем 1.
            {
                if (animationTimer >= mainAnimationTimer_NextTick)//Если таймер основной анимации прошел.
                {
                mainAnimationTimer_NextTick = animationTimer + (1 / mainAnimationSpeed);//Устанавливаем следующий тик таймера.
                mainAnimationFrameIndex += 1;//Прибавляем счетчик кадров.
                    if (mainAnimationFrameIndex >= framesList.Count)//Если счетчик кадров вышел за пределы возможных кадров.
                    {
                        if (modelData.animations[animationDataIndex].repeat)//Если анимацию нужно повторять.
                        {
                            mainAnimationFrameIndex = 0;//Устанавливаем первый кадр.
                        }
                        else//Если повторять анимацию не нужно.
                        {
                            mainAnimationFrameIndex = framesList.Count - 1;//Устанавливаем последний возможный кадр.
                        }
                    }
                }
            }

            if(modelData.renderType == Source_Model_RenderType.Multiple)//Если нужно обрабатывать дополнительный рендер.
            {
                framesMaterial = modelData.GetAnimationMaterial(animationDataIndex, Source_Model_Part.Second, currentRotation);//Получаем материал для текущего поворота.
                framesList = modelData.GetAnimationFrames(animationDataIndex, Source_Model_Part.Second, currentRotation);//Получаем список фреймов для текущего поворота.
                if(framesList.Count == 0)//Если фреймов нет.
                {
                    secondRenderer.sprite = null;//Удаляем спрайт.
                    secondRenderer.material = framesMaterial;//Удаляем материал.
                }
                if (framesList.Count > 0)//Если есть фреймы.
                {
                    secondRenderer.sprite = framesList[secondAnimationFrameIndex];//Устанавливаем фрейм из списка в основной рендерер.
                    secondRenderer.material = framesMaterial;//Устанавливаем материал.
                }
                if (framesList.Count > 1)//Либо спрайт не установлен, либо кадров больше чем 1.
                {
                    if (animationTimer >= secondAnimationTimer_NextTick)//Если таймер основной анимации прошел.
                    {
                        secondAnimationTimer_NextTick = animationTimer + (1 / secondAnimationSpeed);//Устанавливаем следующий тик таймера.
                        secondAnimationFrameIndex += 1;//Прибавляем счетчик кадров.
                        if (secondAnimationFrameIndex >= framesList.Count)//Если счетчик кадров вышел за пределы возможных кадров.
                        {
                            if (modelData.animations[animationDataIndex].repeat)//Если анимацию нужно повторять.
                            {
                                secondAnimationFrameIndex = 0;//Устанавливаем первый кадр.
                            }
                            else//Если повторять анимацию не нужно.
                            {
                                secondAnimationFrameIndex = framesList.Count - 1;//Устанавливаем последний возможный кадр.
                            }
                        }
                    }
                }
            }
        }
    }

    public Source_Model_Rotation GetCameraAngle()
    {
        Vector3 cameraVector = Camera.main.transform.position - transform.position;//Вектор камеры.

        float dotProduct = (cameraVector.x * lookDirection.x) + (cameraVector.z * lookDirection.z);//Скалярное произведение векторов.
        float absCamVector = Mathf.Sqrt(Mathf.Pow(cameraVector.x, 2) + Mathf.Pow(cameraVector.z, 2));//Модуль вектора камеры.
        float absBackVector = Mathf.Sqrt(Mathf.Pow(lookDirection.x, 2) + Mathf.Pow(lookDirection.z, 2));//Модуль нулевого вектора.

        float cosAng = dotProduct / (absCamVector * absBackVector);//Косинус угла.
        double angleRadian = Mathf.Acos(cosAng);//Радианы угла.
        double angleDegrees = angleRadian * 180 / Mathf.PI;//Градусы угла.

        Vector3 dir = (transform.position - Camera.main.transform.position).normalized;//Направление к камере.
        bool cameraLeftSide = Vector3.Dot(dir, parentRight) > 0;//Находиться ли камера слева.

        float resultDegree = (float)angleDegrees;//Градусы поворота.

        if(resultDegree > 0 && resultDegree <= 22.5f)//Front.
        {
            return Source_Model_Rotation.Front;
        }
        if(resultDegree > 22.5f && resultDegree <= 67.5f)//FrontLeft
        {
            return cameraLeftSide ? Source_Model_Rotation.FrontLeft : Source_Model_Rotation.FrontRight;
        }
        if (resultDegree > 67.5f && resultDegree <= 112.5f)//Left
        {
            return cameraLeftSide ? Source_Model_Rotation.Left : Source_Model_Rotation.Right;
        }
        if (resultDegree > 112.5f && resultDegree <= 157.5f)//BackLeft
        {
            return cameraLeftSide ? Source_Model_Rotation.BackLeft : Source_Model_Rotation.BackRight;
        }
        if (resultDegree > 157.5f && resultDegree <= 180)//Back
        {
            return Source_Model_Rotation.Back;
        }



        return Source_Model_Rotation.Front;
    }
}
