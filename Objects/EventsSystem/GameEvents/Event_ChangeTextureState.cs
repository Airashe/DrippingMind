using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ChangeTextureState : Object_Event
{
    public Texture2D newTexture;
    public bool newState = false;
    public override void Initialize()
    {
        base.Initialize();
        if(newTexture != null)
        {
            Camera.main.GetComponent<PI_IngameGUI>().SetTexture(newTexture);
        }
        Camera.main.GetComponent<PI_IngameGUI>().textureState = newState;
    }
}
