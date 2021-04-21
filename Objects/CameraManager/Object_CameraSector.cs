using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_CameraSector : MonoBehaviour
{
    //public bool lookAtPlayer;//Нужно ли следить за игроком.
    public int uniqueId = 0;//Уникальный id сектора.
    public List<GameObject> cameraPath;//Путь камеры.
    public Axis primaryAxis = Axis.None;//Основная ось.
    public bool lookAtPlayer = false;//Смотреть на игрока.
    public bool autoSector = false;//Является автоматическим сектором.
    public float autoSpeed = 1;//На сколько быстро проигрывается анимация.
    public int[] endEvents;//id ивентов, которые произойдут после завершения анимации.
    public Object_Event[] endEventsObjs;//

    public Axis PrimaryAxis
    {
        get
        {
            Axis result = Axis.None;//Что возвращать.
            result = primaryAxis != Axis.None ? primaryAxis : Scale.x > Scale.z ? Axis.X : Axis.Z;//Возвращаем выбранную ось.
            return result;
        }
    }

    public Vector3 Position//Позиция трансформа.
    {
        get { return transform.position; }
    }

    public Vector3 Scale//Размеры трансформа.
    {
        get { return transform.localScale; }
    }

    public void Start()
    {
        for(int cPI = 0; cPI < cameraPath.Count; cPI++)//Для каждой точки на пути.
        {
            if(cameraPath[cPI] == null)//Если точка пуста.
            {
                cameraPath.RemoveAt(cPI);//Удаляем точку с пути.
            }
        }
    }

    public Vector3 GetCameraRailPosition(float sectorPercent)
    {
        List<float> cameraPointsPathPosition = new List<float>();//Позиция точек камеры, на пути.

        float summaryDistance = 0;//Длинна пути камеры.
        for (int i = 0; i < cameraPath.Count; i++)//Для каждой точки камеры в пути.
        {
            if (i < cameraPath.Count - 1)//Если это не последняя точка пути.
            {
                if (cameraPath[i] != null && cameraPath[i + 1] != null)//Если точки существуют.
                {
                    cameraPointsPathPosition.Add(summaryDistance);//Добавляем позицию точки на пути.
                    summaryDistance += Vector3.Distance(cameraPath[i].transform.position, cameraPath[i + 1].transform.position);//Добавляем расстояние до следующей точки.
                }
            }
            else//Если это последняя точка пути.
            {
                cameraPointsPathPosition.Add(summaryDistance);//Добавляем позицию последней точки на пути.
            }
        }

        float unitPathPosition = summaryDistance * sectorPercent;//Позиция юнита на пути.
        int sectionStartPathPositionID = 0;//Позиция начала отрезка, между двумя камерами, на пути, в котором расположен юнит.
        for (int z = 0; z < cameraPointsPathPosition.Count; z++)//Для каждой позиции камеры на пути.
        {
            if (unitPathPosition >= cameraPointsPathPosition[z])//Если позиция юнита на пути больше, чем позиция камеры на пути, то он ее уже прошел, либо на ее отрезке.
            {
                sectionStartPathPositionID = z;//Записываем id позиции камеры, которая является началом отрезка, на котором находиться юнит.
                continue;//Продолжаем цикл.
            }
            else//Если позиция юнита меньше, чем позиция камеры на пути, то он не дошел до нее.
            {
                break;//Останавливаем цикл.
            }
        }

        if (cameraPath.Count > 0)
        {
            Vector3 sectionStartPoint = cameraPath[sectionStartPathPositionID].transform.position;
            Vector3 sectionEndPoint = (sectionStartPathPositionID == cameraPath.Count - 1) ? Vector3.zero : cameraPath[sectionStartPathPositionID + 1].transform.position;

            if (sectionEndPoint != Vector3.zero)//Если конец пути не равен нулевому вектору.
            {
                float sectionLength = cameraPointsPathPosition[sectionStartPathPositionID + 1] - cameraPointsPathPosition[sectionStartPathPositionID];//Длинна участка между двумя камерами.
                float unitAtSection = unitPathPosition - cameraPointsPathPosition[sectionStartPathPositionID];//Позиция юнита на отрезке.
                float subpathPercent = unitAtSection / sectionLength;

                return (1 - subpathPercent) * sectionStartPoint + subpathPercent * sectionEndPoint;
            }
            return sectionStartPoint;
        }
        return Vector3.zero;
    }

    public Vector3 GetCameraRailEuler(float sectorPercent)
    {
        List<float> cameraPointsPathPosition = new List<float>();//Позиция точек камеры, на пути.

        float summaryDistance = 0;//Длинна пути камеры.
        for (int i = 0; i < cameraPath.Count; i++)//Для каждой точки камеры в пути.
        {
            if (i < cameraPath.Count - 1)//Если это не последняя точка пути.
            {
                if (cameraPath[i] != null && cameraPath[i + 1] != null)//Если точки существуют.
                {
                    cameraPointsPathPosition.Add(summaryDistance);//Добавляем позицию точки на пути.
                    summaryDistance += Vector3.Distance(cameraPath[i].transform.position, cameraPath[i + 1].transform.position);//Добавляем расстояние до следующей точки.
                }
            }
            else//Если это последняя точка пути.
            {
                cameraPointsPathPosition.Add(summaryDistance);//Добавляем позицию последней точки на пути.
            }
        }

        float unitPathPosition = summaryDistance * sectorPercent;//Позиция юнита на пути.
        int sectionStartPathPositionID = 0;//Позиция начала отрезка, между двумя камерами, на пути, в котором расположен юнит.
        for (int z = 0; z < cameraPointsPathPosition.Count; z++)//Для каждой позиции камеры на пути.
        {
            if (unitPathPosition >= cameraPointsPathPosition[z])//Если позиция юнита на пути больше, чем позиция камеры на пути, то он ее уже прошел, либо на ее отрезке.
            {
                sectionStartPathPositionID = z;//Записываем id позиции камеры, которая является началом отрезка, на котором находиться юнит.
                continue;//Продолжаем цикл.
            }
            else//Если позиция юнита меньше, чем позиция камеры на пути, то он не дошел до нее.
            {
                break;//Останавливаем цикл.
            }
        }
        if (cameraPath.Count > 0)
        {
            Vector3 sectionStartEuler = cameraPath[sectionStartPathPositionID].transform.GetComponent<Object_CameraPoint>().main;
            Vector3 sectionEndEuler = (sectionStartPathPositionID == cameraPath.Count - 1) ? new Vector3(-500, -500, -500) : cameraPath[sectionStartPathPositionID + 1].transform.GetComponent<Object_CameraPoint>().main;

            if (sectionEndEuler != new Vector3(-500, -500, -500))//Если конец пути не равен нулевому вектору.
            {
                float sectionLength = cameraPointsPathPosition[sectionStartPathPositionID + 1] - cameraPointsPathPosition[sectionStartPathPositionID];//Длинна участка между двумя камерами.
                float unitAtSection = unitPathPosition - cameraPointsPathPosition[sectionStartPathPositionID];//Позиция юнита на отрезке.
                float subpathPercent = unitAtSection / sectionLength;

                return (1 - subpathPercent) * sectionStartEuler + subpathPercent * sectionEndEuler;
            }
            return sectionStartEuler;
        }
        return Vector3.zero;
    }

    public bool PlayerInSector(Vector3 unitPosition)//Находиться ли игрок в данном секторе.
    {
        if (!autoSector)//Если сектор не автоматический
        {
            if (unitPosition.x >= (transform.position.x - (transform.localScale.x / 2)) && unitPosition.x <= (transform.position.x + (transform.localScale.x / 2)))//Если игрок попадает под скейлы по х.
            {
                if (unitPosition.z >= (transform.position.z - (transform.localScale.z / 2)) && unitPosition.z <= (transform.position.z + (transform.localScale.z / 2)))//Если игрок попадает под скейлы z.
                {
                    if (unitPosition.y >= (transform.position.y - (transform.localScale.y / 2)) && unitPosition.y <= (transform.position.y + (transform.localScale.y / 2)))
                    {
                        return true;//Возвращаем истину.
                    }
                }
            }
        }
        return false;//Возвращаем ложь.
    }
}

public enum Axis {None, X ,Z}
