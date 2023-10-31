using UnityEngine;
using System;

namespace Editarrr.Audio
{
    [Serializable]
    //AudioManager.Instance.PlayRandomizedAudioClip("clipName");
    [CreateAssetMenu(fileName = "audioClips", menuName = "Pool/Audio/new audio pool")]
    public class AudioSfxPool : ScriptableObject
    {

        [Header("set of SFX clips that can be added to the audio manager")]

        // [SerializeField] AudioClip audioClips;
        [SerializeField] AudioClip[] clips;

        public AudioClip[] GetClips()
        {
            return clips;
        }
    }
}


