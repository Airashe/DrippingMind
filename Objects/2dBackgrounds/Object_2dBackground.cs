using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_2dBackground : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Camera.main != null)//Если есть основная камера.
        {
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3;
            transform.position = transform.position + Vector3.up * 2;
        }
    }
}
