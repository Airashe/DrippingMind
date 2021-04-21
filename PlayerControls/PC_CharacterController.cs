using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_CharacterController : MonoBehaviour
{
    //Ссылки на объекты.
    private Object_Player playerData;//Данные игрока.
    private Source_GameManager gameManager;//Менеджер игры.
    public Transform movementCoordinateAxis;//Координатная ось передвижения.
    public bool showCoordinateAxis = false;//Показывать координатную ось, управляемого юнита.
    public Transform movementCoordinateAxisAlternate;
    private Quaternion behind = Quaternion.Euler(0, 0, 0);
    public Vector3 moveDirection;//Вектор движения - нулевой.
    private bool back = false;
    private bool forward = false;
    private bool right = false;
    private bool left = false;

    private void Start()
    {
        playerData = Camera.main.gameObject.GetComponent<Object_Player>();//Ссылка на данные игрока.
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Source_GameManager>();//Получаем ссылку на контроллер игры.
    }

    private void Update()
    {
        if(playerData.unit != null)//Если есть управляемый юнит.
        {
            UnitMovement();//Передвижение юнита.
            UnitItemUsage();//Использование активных слотов юнита.
        }
    }

    private void OnDisable()
    {
        if(playerData.unit != null)//Если есть управляемый юнит.
        {
            playerData.unit.SetState(Property_UnitState.Idle);//Изменяем состояние юнита на ходьбу.
        }
    }

    private void UnitItemUsage()
    {
        for(int i = 0; i < playerData.unit.inventory.groups.Count;i++)//Для каждой группы инвентаря.
        {
            if(playerData.unit.inventory.groups[i].hotkeyName.Length > 0)//Если есть название горячей клавиши.
            {
                if(Input.GetKeyDown(Source_Constants.userInputs[playerData.unit.inventory.groups[i].hotkeyName]))//Если нажата кнопка этой группы.
                {
                    if(playerData.unit.inventory.groups[i].items.Count > 0)//Если есть предмет для использования.
                    {
                        playerData.unit.UseItem_Start(i, 0);//Сообщаем что использование начато.
                    }
                }
                if (Input.GetKeyUp(Source_Constants.userInputs[playerData.unit.inventory.groups[i].hotkeyName]))//Если нажата кнопка этой группы.
                {
                    if (playerData.unit.inventory.groups[i].items.Count > 0)//Если есть предмет для использования.
                    {
                        playerData.unit.UseItem_Cancel(i, 0);//Сообщаем что использование прервано.
                    }
                }
            }
        }
    }

    private void UnitMovement()
    {
        //Контроль передвижения.
        Transform movementCoordinateCenter = movementCoordinateAxisAlternate == null ? movementCoordinateAxis : movementCoordinateAxisAlternate;
        
        if (movementCoordinateAxisAlternate == null)
        {
            movementCoordinateCenter.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);//Поворачиваем систему координат передвижения под камеру.
        }

        if (showCoordinateAxis)//Если нужно показывать оси.
        {
            Debug.DrawRay(playerData.unit.transform.position, playerData.unit.transform.forward, Color.green);//Прямо относительно юнита.
            Debug.DrawRay(playerData.unit.transform.position, playerData.unit.transform.forward * -1, Color.blue);//Назад относительно юнита.
            Debug.DrawRay(playerData.unit.transform.position, playerData.unit.transform.right * -1, Color.red);//Лево относительно юнита.
            Debug.DrawRay(playerData.unit.transform.position, playerData.unit.transform.right, Color.yellow);//Право относительно юнита.

            Debug.DrawRay(playerData.unit.transform.position + Vector3.up, movementCoordinateCenter.transform.forward, Color.green);//Прямо относительно оси.
            Debug.DrawRay(playerData.unit.transform.position + Vector3.up, movementCoordinateCenter.transform.forward * -1, Color.blue);//Назад относительно оси.
            Debug.DrawRay(playerData.unit.transform.position + Vector3.up, movementCoordinateCenter.transform.right * -1, Color.red);//Лево относительно оси.
            Debug.DrawRay(playerData.unit.transform.position + Vector3.up, movementCoordinateCenter.transform.right, Color.yellow);//Право относительно оси.
        }

        CharacterController charController = playerData.unit.GetComponent<CharacterController>();//Получаем ссылку на контроллер юнита.

        if (charController.isGrounded)//Если персонаж находиться на земле.
        {
            if (Input.GetKeyDown(Source_Constants.userInputs["MOVEMENT_MOVELEFT"]))//Если кнопка движение влево.
            {
                if (playerData.useCoordinateAxis)//Если используем координатную ось либо мы в 2д пространстве.
                {
                    moveDirection = movementCoordinateCenter.transform.right * -1;//Устанавливаем движение влево от поворота объекта юнита.
                    moveDirection *= playerData.unit.movementSpeed;//Умножаем на скорость юнита.
                    playerData.unit.transform.eulerAngles = new Vector3(0, movementCoordinateCenter.eulerAngles.y + 270, 0);//Прямо
                    left = true;
                }
            }
            if (Input.GetKeyDown(Source_Constants.userInputs["MOVEMENT_MOVERIGHT"]))//Если кнопка движение вправо.
            {
                if (playerData.useCoordinateAxis)//Если используем координатную ось либо мы в 2д пространстве.
                {
                    moveDirection = movementCoordinateCenter.transform.right;//Устанавливаем движение вправо от поворота объекта юнита.
                    moveDirection *= playerData.unit.movementSpeed;//Умножаем на скорость юнита.
                    playerData.unit.transform.eulerAngles = new Vector3(0, movementCoordinateCenter.eulerAngles.y + 90, 0);//Прямо
                    right = true;
                }
            }
            if (Input.GetKeyDown(Source_Constants.userInputs["MOVEMENT_MOVEFORWARD"]))//Если кнопка движение вперед.
            {
                if (playerData.useCoordinateAxis)//Если используем координатную ось и не в 2д.
                {
                    if (gameManager.gameMode != GameMode.gm2d)
                    {
                        moveDirection = movementCoordinateCenter.transform.forward;//Устанавливаем движение прямо от поворота объекта юнита.
                        moveDirection *= playerData.unit.movementSpeed;//Умножаем на скорость юнита.
                        playerData.unit.transform.eulerAngles = new Vector3(0, movementCoordinateCenter.eulerAngles.y + 0, 0);//Прямо
                        forward = true;
                    }
                }
            }
            if (Input.GetKeyDown(Source_Constants.userInputs["MOVEMENT_MOVEBACKWARD"]))//Если кнопка движение назад.
            {
                if (playerData.useCoordinateAxis)//Если используем координатную ось и не в 2д.
                {
                    if (gameManager.gameMode != GameMode.gm2d)
                    {
                        moveDirection = movementCoordinateCenter.transform.forward * -1;//Устанавливаем движение назад от поворота объекта юнита.
                        moveDirection *= playerData.unit.movementSpeed;//Умножаем на скорость юнита.
                        playerData.unit.transform.eulerAngles = new Vector3(0, movementCoordinateCenter.eulerAngles.y + 180, 0);//Прямо
                        back = true;
                    }
                }
                else
                {
                    behind = Quaternion.Euler(0, 180, 0) * playerData.unit.transform.rotation;//Больший поворот.
                }
            }

            //Изменение состояния юнита.
            if (Input.GetKeyUp(Source_Constants.userInputs["MOVEMENT_MOVEBACKWARD"]) ||
               Input.GetKeyUp(Source_Constants.userInputs["MOVEMENT_MOVEFORWARD"]) ||
               Input.GetKeyUp(Source_Constants.userInputs["MOVEMENT_MOVERIGHT"]) ||
               Input.GetKeyUp(Source_Constants.userInputs["MOVEMENT_MOVELEFT"]))
                {
                    playerData.unit.SetState(Property_UnitState.Idle);//Изменяем состояние юнита на ходьбу.
                }
            if(Input.GetKey(Source_Constants.userInputs["MOVEMENT_MOVEBACKWARD"]))
            {
                if(gameManager.gameMode != GameMode.gm2d)//Если не в 2д.
                {
                    playerData.unit.SetState(Property_UnitState.Walking);//Изменяем состояние юнита на ходьбу.
                }
            }
            if(Input.GetKey(Source_Constants.userInputs["MOVEMENT_MOVEFORWARD"]))
            {
                if (gameManager.gameMode != GameMode.gm2d)//Если не в 2д.
                {
                    playerData.unit.SetState(Property_UnitState.Walking);//Изменяем состояние юнита на ходьбу.
                }
            }
            if(Input.GetKey(Source_Constants.userInputs["MOVEMENT_MOVERIGHT"]))
            {
                playerData.unit.SetState(Property_UnitState.Walking);//Изменяем состояние юнита на ходьбу.
            }
            if(Input.GetKey(Source_Constants.userInputs["MOVEMENT_MOVELEFT"]))
            {
                playerData.unit.SetState(Property_UnitState.Walking);//Изменяем состояние юнита на ходьбу.
            }

            if (Input.GetKeyUp(Source_Constants.userInputs["MOVEMENT_MOVELEFT"]))
            {
                left = false;
                if(!forward && !back && !right)
                {
                    moveDirection = Vector3.zero;
                }
            }
            else if (Input.GetKeyUp(Source_Constants.userInputs["MOVEMENT_MOVERIGHT"]))
            {
                right = false;
                if (!forward && !back && !left)
                {
                    moveDirection = Vector3.zero;
                }
            }
            else if (Input.GetKeyUp(Source_Constants.userInputs["MOVEMENT_MOVEFORWARD"]))
            {
                forward = false;
                if (!left && !back && !right)
                {
                    moveDirection = Vector3.zero;
                }
            }
            else if (Input.GetKeyUp(Source_Constants.userInputs["MOVEMENT_MOVEBACKWARD"]))
            {
                back = false;
                if (!forward && !left && !right)
                {
                    moveDirection = Vector3.zero;
                }
            }



            if (Input.GetKey(Source_Constants.userInputs["MOVEMENT_MOVELEFT"]))
            {
                if (!playerData.useCoordinateAxis)//Если не используем координатную ось.
                {
                    playerData.unit.transform.Rotate(0, -1, 0);//Поворот влево.
                }
            }
            if (Input.GetKey(Source_Constants.userInputs["MOVEMENT_MOVERIGHT"]))
            {
                if (!playerData.useCoordinateAxis)//Если не используем координатную ось.
                {
                    playerData.unit.transform.Rotate(0, 1, 0);//Поворот вправо.
                }
            }
            if (Input.GetKey(Source_Constants.userInputs["MOVEMENT_MOVEFORWARD"]))
            {
                if (!playerData.useCoordinateAxis)//Если не используем координатную ось.
                {
                    moveDirection = playerData.unit.transform.forward;//Движение вперед.
                    moveDirection *= playerData.unit.movementSpeed;//Умножаем на скорость юнита.
                }
            }
            if (Input.GetKey(Source_Constants.userInputs["MOVEMENT_MOVEBACKWARD"]))
            {
                if (!playerData.useCoordinateAxis)//Если не используем координатную ось.
                {
                    playerData.unit.transform.rotation = Quaternion.Lerp(playerData.unit.transform.rotation, behind, 7 * Time.deltaTime);
                }
            }

            moveDirection.y -= 500 * Time.deltaTime;//Действие гравитации.

            charController.Move(moveDirection * Time.deltaTime);//Передаем движение компоненту контроллера.
        }
        else
        {
            moveDirection = Vector3.zero;
            moveDirection.y -= 500 * Time.deltaTime;//Действие гравитации.

            charController.Move(moveDirection * Time.deltaTime);//Передаем движение компоненту контроллера.
        }

        if (moveDirection.y < -100)//Если гравитация ушла слишком далеко.
        {
            moveDirection.y = -100;//Устанавливаем предел гравитации.
        }

        //Постоянное передвижение юнита, по одной из осей.
        //moveDirection = playerData.unit.transform.TransformDirection(moveDirection);//Расчитываем направление движение относительно юнита.
    }
}
