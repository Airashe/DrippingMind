using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChildren_Weapon : Object_ItemChildren
{
    public bool aiming = false;//Прицеливается ли игрок.
    public Light gunLight1;//Свет от выстрела.
    public Light gunLight2;//Свет от выстрела.
    public Object_SoundMaker shotSoundMaker;
    public float lightLivetime = 0.1f;//Сколько будет видно свет от выстрела.
    private float lightLiveTimer = 0;

    private void Start()
    {
        Camera.main.GetComponent<Object_Player>().showCursorWithoutCheck = true;
    }
    private void OnDestroy()
    {
        if(Camera.main.GetComponent<Object_Player>() != null)
        {
            Camera.main.GetComponent<Object_Player>().showCursorWithoutCheck = false;
        }
    }
    private void Update()
    {
        if(lightLiveTimer > 0)
        {
            gunLight1.enabled = true;
            gunLight2.enabled = true;
            lightLiveTimer -= Time.deltaTime;
        }
        else
        {
            gunLight1.enabled = false;
            gunLight2.enabled = false;
        }
        Source_ItemBase parentData = unit.inventory.groups[parentGroup].items[parentPos].data;
        if (Input.GetKeyDown(Source_Constants.userInputs["INVENTORY_ITEMMENU_SHOW"]))//Прицеливание.
        {
            aiming = true;//Устанавливаем, что прицеливается.
        }
        if (Input.GetKeyUp(Source_Constants.userInputs["INVENTORY_ITEMMENU_SHOW"]))//Прицеливание.
        {
            aiming = false;//Снимаем прицеливание..
        }

        if (aiming)//Если прицеливается.
        {
            unit.model.SetModel(parentData.alterAnimationList[GetAnimationId()], unit.startAnimationName);
        }
        else
        {
            unit.model.SetModel(parentData.animationSetInHand, unit.startAnimationName);
        }
    }

    private int GetAnimationId()
    {
        Source_ItemBase parentData = unit.inventory.groups[parentGroup].items[parentPos].data;
        int size = Screen.height / parentData.alterAnimationList.Count;
        for (int i = 0; i < parentData.alterAnimationList.Count; i++)
        {
                if (Input.mousePosition.y > i * size && Input.mousePosition.y <= (i + 1) * size)
                {
                return i;
                }
        }
        return 0;
    }

    public void Fire(int damage, bool inverse, AudioClip shotSound)//Выстрел
    {
        if (aiming)//Если игрок целиться
        {
            shotSoundMaker.source.clip = shotSound;
            shotSoundMaker.source.Play();
            lightLiveTimer = lightLivetime;
            RaycastHit hit;//Информация о попадании.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//Выпускаемый луч.

            if (Physics.Raycast(ray, out hit, 1 << 12))//Выпускаем луч.
            {
                Vector3 hitPoint = hit.point;//Точка попадания.

                if (unit != null)
                {
                    hitPoint.z = unit.position.z;//Луч паралелен юниту.\
                    Debug.DrawLine(transform.position, hitPoint, Color.red);
                    Vector3 direction = hitPoint - transform.position;//Устанавливаем направление от юнита до точки курсора.

                    List<GameObject> hittedGOS = new List<GameObject>();//Список юнитов по которым мы попали.

                    direction = inverse ? Quaternion.Euler(0, 180, 0) * direction : direction;//Если нужно инверсировать - то инверсируем.

                    RaycastHit fireHit;
                    for (int i = -50; i < 50;)//Для каждого возможного поворота.
                    {
                        Vector3 newDirection = Quaternion.Euler(0, i, 0) * direction;//Поворот направления.

                        if (Physics.Raycast(transform.position, newDirection, out fireHit))//Выпускаем настоящий луч стрельбы.
                        {
                            bool added = false;
                            foreach (GameObject hitObj in hittedGOS)//Для каждого объекта, который уже зарегестрирован.
                            {
                                if (hitObj == fireHit.transform.gameObject)//Если в списке уже есть этот объект.
                                {
                                    added = true;
                                }
                            }
                            if (!added)//Если объекта еще нет в списке.
                            {
                                hittedGOS.Add(fireHit.transform.gameObject);//Добавляем объект в список.
                            }
                            Debug.DrawLine(transform.position, fireHit.point, Color.green);//Дебаг.
                        }
                        i += 2;//Делаем шаг.
                    }

                    foreach (GameObject obj in hittedGOS)//Для каждого объекта в который было зафиксированно попадание.
                    {
                        if (obj.GetComponent<Object_Unit>() != null)//Если есть компонент юнита.
                        {
                            obj.GetComponent<Object_Unit>().GetDamage(damage);//Наносим урон.
                            Debug.Log("Damagin " + obj.name + " for " + damage + " damage.");
                        }
                    }

                    Debug.Log(hittedGOS.Count);
                }
            }
        }
    }
}
