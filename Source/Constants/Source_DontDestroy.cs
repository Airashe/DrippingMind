using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source_DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);//Не уничтожать между сценами.
    }
}
