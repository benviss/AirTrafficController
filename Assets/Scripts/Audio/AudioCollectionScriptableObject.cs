using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewAudioCollection", menuName = "Audio/New Audio Collection")]
public class AudioCollectionScriptableObject : ScriptableObject
{
    public Sound[] Sounds;
}
