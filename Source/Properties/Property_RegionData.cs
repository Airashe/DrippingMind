using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Property_RegionData
{
    public Object_SoundRegion region;
    public AudioSource source;
    public float deadTime = 2;

    public Property_RegionData(Object_SoundRegion region, AudioSource source)
    {
        this.region = region;
        this.source = source;
        this.deadTime = 1;
    }
}
