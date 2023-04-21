using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Editarrr.Audio
{
    public class PlaySound : MonoBehaviour
    {
        [SerializeField] string clipName;
        public void Play()
        {
            AudioManager.instance.PlayAudioClip(clipName);
        }
    }
}


