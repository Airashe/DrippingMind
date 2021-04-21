using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Проверка рендера моделей.
public class Test_ModelsRender : MonoBehaviour
{
    private void OnGUI()
    {
        if(gameObject.tag == "Object_Model")
        {
            GUI.Label(new Rect(0, 0, 180, 20), "Main animation speed: ");
            gameObject.GetComponent<Object_Model>().mainAnimationSpeed = int.Parse(GUI.TextField(new Rect(180, 0, 180, 20), gameObject.GetComponent<Object_Model>().mainAnimationSpeed.ToString()));

            GUI.Label(new Rect(0, 20, 180, 20), "Second animation speed: ");
            gameObject.GetComponent<Object_Model>().secondAnimationSpeed = int.Parse(GUI.TextField(new Rect(180, 20, 180, 20), gameObject.GetComponent<Object_Model>().secondAnimationSpeed.ToString()));
            
            for(int i = 0; i < gameObject.GetComponent<Object_Model>().modelData.animations.Count; i++)
            {
                if(GUI.Button(new Rect(0, 40+(30*i), 180, 30), gameObject.GetComponent<Object_Model>().modelData.animations[i].name))
                {
                    gameObject.GetComponent<Object_Model>().SetAnimation(gameObject.GetComponent<Object_Model>().modelData.animations[i].name);
                }
            }
        }
    }
}
