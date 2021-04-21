using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Свойство анимации в модели.
[System.Serializable]
public class Property_ModelsAnimation
{
    public string name;//Имя анимации.
    public bool repeat;//Нужно ли повторять анимацию.
    public RenderPart main;//Базовый набор анимации.
    public RenderPart second;//Дополнительный набор анимаций(для ног например).
    public int[] endAnimationEvents;//Конечные анимации.

    [System.Serializable]
    public class RenderPart
    {
        public MaterialsList materials;
        public FramesList frames;
    }

    [System.Serializable]
    public class MaterialsList
    {
        public Material front;//Кадры для отображения спереди.
        public Material frontLeft;//Кадры для отображения спереди-слева.
        public Material frontRight;//Кадры для отображения спереди-справа.
        public Material right;//Кадры для отображения справа.
        public Material left;//Кадры для отображение слева.
        public Material back;//Кадры для отображения сзади.
        public Material backLeft;//Кадры для отображения слева.
        public Material backRight;//Кадры для отображения справа.
    }

    //Список кадров анимации для каждого угла камеры.
    [System.Serializable]
    public class FramesList
    {
        public List<Sprite> front;//Кадры для отображения спереди.
        public List<Sprite> frontLeft;//Кадры для отображения спереди-слева.
        public List<Sprite> frontRight;//Кадры для отображения спереди-справа.
        public List<Sprite> right;//Кадры для отображения справа.
        public List<Sprite> left;//Кадры для отображение слева.
        public List<Sprite> back;//Кадры для отображения сзади.
        public List<Sprite> backLeft;//Кадры для отображения слева.
        public List<Sprite> backRight;//Кадры для отображения справа.
    }
}
