using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_SoundRegion : MonoBehaviour
{
    public AudioClip regionSound;
    public float volume;//громкость звука.
    public bool playing;
    public bool loop;

    public bool PlayerInRegion(Vector3 unitPosition)//Находиться ли игрок в данном секторе.
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
        return false;//Возвращаем ложь.
    }
}
