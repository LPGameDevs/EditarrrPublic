using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Editarrr.Audio
{
    public class PlaySound : MonoBehaviour
    {
        [SerializeField] string clipName;
        [SerializeField] MultiSound sound;
        public void Play()
        {
            AudioManager.Instance.PlayAudioClip(clipName);
        }

        public void TestMultiSound()
        {
         
            AudioManager.Instance.PlayShuffledSound(sound.name,0,0);
        }
    }
}


