using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CameraPosition : MonoBehaviour
{
    public Transform[] positions;
    public float Timer = 5;
    public int position = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(positions.Length > 0)
        {
            if(Timer <= 0)
            {
                position += 1;
                if(position >= positions.Length)
                {
                    position = 0;
                }
                Timer = 5;

                Camera.main.transform.position = positions[position].position;
                Camera.main.transform.rotation = positions[position].rotation;

            }
            if(Timer > 0)
            {
                Timer -= Time.deltaTime;
            }
        }
	}
}
