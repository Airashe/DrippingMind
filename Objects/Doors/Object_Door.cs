using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Door : MonoBehaviour
{
    public Door_TargetState currentTargetState = Door_TargetState.close;
    public bool open;

    public Vector3 closedRotation;//Закрытый поворот.
    public Vector3 openRotation;//Открытый поворот.
    public Vector3 altOpenRotation;//Альтернативный открытый поворот.
    public Transform controll;

    private bool openerLeftSide;//С какой стороны открыватель двери.

    public void Update()
    {
        ChangeDoorState();//Постоянное изменение.
    }

    public void SetTargetState(Door_TargetState newState)
    {
        currentTargetState = newState;//Устанавливаем новое целевое состояние.
        Vector3 dir = (controll.position - Camera.main.GetComponent<Object_Player>().unit.position).normalized;//Направление к открывающему юниту.
        openerLeftSide = Vector3.Dot(dir, controll.right) > 0;//Находиться ли открыватель слева.

    }

    public void ChangeDoorState()
    {
        Vector3 targetOpenRotation = openerLeftSide ? openRotation : altOpenRotation;
        if (!open && currentTargetState == Door_TargetState.open)//Если дверь не открыта, но должна быть.
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetOpenRotation), 3 * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, Quaternion.Euler(targetOpenRotation)) <= 0.01f)
            {
                open = true;
            }
        }
        if(open && currentTargetState == Door_TargetState.close)//Если дверь открыта, но должна быть закрыта.
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(closedRotation), 3 * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, Quaternion.Euler(closedRotation)) <= 0.01f)
            {
                open = false;
            }
        }
    }
}

public enum Door_TargetState { open, close}
