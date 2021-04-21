using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source_CameraManager : MonoBehaviour
{
    //Ссылки на данные.
    private Object_Player playerData;//Данные игрока.
    private Source_GameManager gameManager;//Гейм менеджер.

    //Автоматическое проигрывание секторов.
    private bool playingAutoSector = false;//Проигрываетсял ли сейчас автоматический сектор.
    private int playingId = -1;//id проигрываемого сектора.
    private float autoPercent = 0;//Сколько от сектора проигралось.

    public Vector2 worldStart2D;//Левая граница мира, для 2д.
    public Vector2 worldEnd2D;//Правая граница мира, для 2д.

    public LayerMask CamOcclusion;
    private Vector3 cameraStandartPosition;//Позиция камеры относительно юнита.

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Получаем ссылку на контроллер игры.
        playerData = Camera.main.GetComponent<Object_Player>();//Получаем ссылку на данные игрока.
    }

    private void Update()
    {
        if(playerData.cameraState == Camera_State.Await && playerData.unit != null)//Если камера ждет команд.
        {
            if (!playingAutoSector)
            {
                bool inSector = false;//Устанавливаем, что игрок не в секторе.
                foreach (Object_CameraSector cameraSector in gameManager.cameraSectors)//Для каждого сектора в списке.
                {
                    if (cameraSector.PlayerInSector(playerData.unit.transform.position))//Если юнит игрока в секторе.
                    {
                        inSector = true;//Устанавливаем, что игрок находиться в одном из секторов.
                        playerData.useCoordinateAxis = true;//Использование координатной сетки если мы в секторе.
                        //Высчитывание позиции игрока на секторе.
                        float sectorPercent = 0;//Процент пройденной части сектора.
                        float cameraSectorScale = cameraSector.PrimaryAxis == Axis.X ? cameraSector.Scale.x : cameraSector.Scale.z;//Размер сектора.
                        float cameraSectorPosition = cameraSector.PrimaryAxis == Axis.X ? cameraSector.Position.x : cameraSector.Position.z;//Положение сектора.
                        float unitPosition = cameraSector.PrimaryAxis == Axis.X ? playerData.unit.transform.position.x : playerData.unit.transform.position.z;//Положение юнита.
                        float unitZonSectorAxis = cameraSectorScale - (((cameraSectorScale / 2) + cameraSectorPosition) - unitPosition);
                        //Размер сектора - (Расстояние до конца сектора) = положение юнита на координатной прямой сектора ( или сколько он прошел)
                        sectorPercent = unitZonSectorAxis / cameraSectorScale;//Получаем позицию игрока на

                        Camera.main.transform.position = cameraSector.GetCameraRailPosition(sectorPercent);//Устанавливаем камеру на нужную позицию.

                        if (cameraSector.lookAtPlayer)//Если нужно направлять камеру на игрока.
                        {
                            Camera.main.transform.LookAt(playerData.unit.transform);//Поворачиваем камеру на игрока.
                            playerData.gameObject.GetComponent<PC_CharacterController>().movementCoordinateAxisAlternate = cameraSector.GetComponent<Object_CameraSector>().cameraPath[0].transform;
                            //Устанавливаем основную ось движения - точка камеры.
                        }
                        else//Если не нужно направлять камеру на игрока.
                        {
                            playerData.gameObject.GetComponent<PC_CharacterController>().movementCoordinateAxisAlternate = null;
                            Camera.main.transform.eulerAngles = cameraSector.GetCameraRailEuler(sectorPercent);//Устанавливаем поворот камеры как у точки.
                        }
                    }
                }
                if (!inSector)
                {
                    CameraMode_BehindUnit();//Ставим камеру за юнитом.
                }
            }
            else//Если проигрываем анимацию движения камеры.
            {
                if(playingId != -1)//Если есть id для проигрывания.
                {
                    foreach (Object_CameraSector cameraSector in gameManager.cameraSectors)//Для каждого сектора в списке.
                    {
                        if(cameraSector.uniqueId == playingId)//Если это нужный нам сектор.
                        {
                            if (autoPercent < 1)//Если сектор еще не завершен.
                            {
                                autoPercent += Time.deltaTime*cameraSector.autoSpeed;//Прибавляем процент пройденного растояния, чтобы двигать камеру.
                                Camera.main.transform.position = cameraSector.GetCameraRailPosition(autoPercent);//Устанавливаем камеру на нужную позицию.
                                Camera.main.transform.eulerAngles = cameraSector.GetCameraRailEuler(autoPercent);//Устанавливаем поворот камеры как у точки.
                            }
                            else//Если сектор завершен.
                            {
                                playingId = -1;//Ставим, что никакой сектор не проигрывается.
                                playingAutoSector = false;//Ставим, что проигрышь анимации не идет.
                                autoPercent = 0;//Обнуляем счетчик пройденного расстояния.
                                if(cameraSector.endEvents.Length > 0)
                                {
                                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEvent(cameraSector.endEvents);//Выполняем эвенты после заверешния проигрывания.
                                }
                                if (cameraSector.endEventsObjs.Length > 0)
                                {
                                    GameObject.FindGameObjectWithTag("EventsManager").GetComponent<Source_EventsManager>().EngageEventObjects(cameraSector.endEventsObjs);//Выполняем эвенты после заверешния проигрывания.
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void CameraMode_BehindUnit()//Логика камеры - следовать за спиной юнита.
    {
        if (gameManager.gameMode == GameMode.gm3d)
        {
            playerData.useCoordinateAxis = false;//Отключаем использование координатной оси.

            Vector3 cameraDirectionPosition = playerData.unit.transform.forward;//Вектор по которому будет высчитываться положение камеры вперед/назад от юнита.
            float cameraDirectionDestiny = -0.7f;//На сколько далеко от юнита он будет перемещена отрицательное - из-за спины, положительное - спереди.
            Vector3 cameraLeftRightVector = Camera.main.transform.right;//Вектор по которому будет перемещаться камера влево и вправо относительно юнита.
            float cameraLeftRightDestiny = 0.3f;//На сколько далеко будет смещение влево или право - вправо положительное, в лево - отрицательное.
            float upDownDistiny = 0.2f;//На сколько будет смещение вверх/низ - вверх положительное, вниз отрицательное.

            //Высчитываем стандартную позицию камеры относительно юнита.
            cameraStandartPosition = playerData.unit.transform.position + cameraDirectionPosition * cameraDirectionDestiny;//Положение камеры относительно юнита перед/зад.
            cameraStandartPosition = cameraStandartPosition + cameraLeftRightVector * cameraLeftRightDestiny;//Положение камеры относительно юнита лево/право.
            cameraStandartPosition = cameraStandartPosition + Vector3.up * upDownDistiny;//Положение камеры относительно юнита вверх/вниз.

            Vector3 cameraCheckPosition = cameraStandartPosition + cameraDirectionPosition *0.1f;


            bool somethingBetweeen = false;//Есть ли что-то между камерой.
            RaycastHit cameraCheckHit;//Во что попадает луч, проверяющий есть ли что-то между камерой и юнитом.
            Debug.DrawLine(cameraCheckPosition, playerData.unit.position, Color.yellow);
            if (Physics.Linecast(cameraCheckPosition, playerData.unit.position, out cameraCheckHit, CamOcclusion))//Если есть какой-то объект.
            {
                    Debug.Log(cameraCheckHit.collider.name + " between camera and unit");//Проверка на хит сделана.
                    somethingBetweeen = true;//Между камерой и юнитом что-то есть.
                    Camera.main.transform.position = cameraCheckHit.point + Vector3.forward * 0.1f;
            }
            if(!somethingBetweeen)
            {
                Camera.main.transform.position = cameraStandartPosition;
            }
            Camera.main.transform.LookAt(cameraStandartPosition + cameraDirectionPosition * 5);//Направление куда смотрит камера.
        }
        else if(gameManager.gameMode == GameMode.gm2d)
        {
            playerData.useCoordinateAxis = true;
            if(playerData.unit.position.x <= worldStart2D.x && playerData.unit.position.x >= worldEnd2D.x)//Если в пределах координат уровня.
            {
                //if(Camera.main.transform.position.y <= worldStart2D.y)
                Camera.main.transform.position = playerData.unit.transform.position + Vector3.forward * 1.5f;
                Camera.main.transform.position = Camera.main.transform.position + Vector3.up * 0.2f;
                Camera.main.transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    public void PlaySector(int sectorId)//Приказывать проиграть анимацию сектора.
    {
        playingId = sectorId;//Устанавливаем id для проигрыша.
        playingAutoSector = true;//Устанавливаем, что анимация проигрывается.
    }

    public void PlaceCameraAt(Vector3 position, Vector3 rotation)//Перемещает камеру в указанные координаты.
    {
        Camera.main.transform.position = position;//Устанавливаем позицию.
        Camera.main.transform.eulerAngles = rotation;//Устанавливаем ротацию.
    }

}
public enum Camera_State { Await, Busy }
