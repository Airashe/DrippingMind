using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Source_SoundCollection
{
    public LayerMask layer;//Слой которому принадлежит коллекция.
    public List<AudioClip> clips;//Список аудиодорожек, для этого слоя.
}
