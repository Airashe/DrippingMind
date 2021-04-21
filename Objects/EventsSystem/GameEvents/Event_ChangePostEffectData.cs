using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Rendering;

public class Event_ChangePostEffectData : Object_Event
{
    public PostProcessingProfile newProfile;
    public override void Initialize()
    {
        base.Initialize();
        Camera.main.GetComponent<PostProcessingBehaviour>().profile = newProfile;
    }
}
