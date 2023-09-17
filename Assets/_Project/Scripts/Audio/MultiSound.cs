using UnityEngine;
using System;
//AudioManager.instance.PlayAudioClip("clipName");

[Serializable]
[CreateAssetMenu(fileName = "new multi Sound",  menuName = "Pool/Audio/multi sound")]
public class MultiSound : ScriptableObject
{
    public AudioClip[] clips;
    int lastIndex;

    public AudioClip GetShuffledClip()
    { 
        int randomIndex = UnityEngine.Random.Range(0, clips.Length);  // pick a random index
        if (randomIndex == lastIndex && clips.Length > 1)  // if the same index was picked last time and there are more than 1 clips
        {
            randomIndex = (randomIndex + 1) % clips.Length;  // select the next clip in the array
        }
        
        lastIndex = randomIndex;
        return clips[randomIndex];
    }
}