using System.Collections.Generic;
using UnityEngine;

namespace Editarrr.Audio
{


    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("SFX")]
        [SerializeField] AudioSfxPool sfxPool;

        [Header("Music")]
        [SerializeField] AudioSource bgmSource;
        [SerializeField] bool playBGM;

        AudioSource[] sfxSources;
        int currentSFXSource = 0;
        Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

        [SerializeField] Transform sfxSourceParent;
        void Awake()
        {

            if (instance != null && instance != this)
                Destroy(this.gameObject);
            else
            {
                instance = this;

            }

            InitSfxPoolAndSources();

            if (playBGM && !bgmSource.isPlaying) bgmSource.Play();
        }

        void InitSfxPoolAndSources()
        {
            if (sfxSourceParent.childCount > 0)
            {
                sfxSources = sfxSourceParent.GetComponentsInChildren<AudioSource>();
            }
            else Debug.Log("no sfx sources found");

            var clips = sfxPool.GetClips();

            foreach (AudioClip clip in clips)
            {
                audioDict.Add(clip.name, clip);
            }
        }

        public void PlayAudioClip(string clipName)
        {
            if (audioDict.ContainsKey(clipName))
            {
                sfxSources[currentSFXSource].PlayOneShot(audioDict[clipName]);
                currentSFXSource = (currentSFXSource + 1) % sfxSources.Length;
            }
            else
            {
                Debug.Log("AudioManager: AudioClip " + clipName + " not found in dictionary.");
            }
        }

        public void PlayRandomizedAudioClip(string clipName, float pitchVariance, float volumeVariance)
        {
            if (audioDict.ContainsKey(clipName))
            {
                float pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
                float volume = 1f + Random.Range(-volumeVariance, volumeVariance);
                sfxSources[currentSFXSource].pitch = pitch;
                sfxSources[currentSFXSource].volume = volume;
                sfxSources[currentSFXSource].PlayOneShot(audioDict[clipName]);
                currentSFXSource = (currentSFXSource + 1) % sfxSources.Length;
            }
            else
            {
                Debug.Log("AudioManager: AudioClip " + clipName + " not found in dictionary.");
            }
        }
    }
}
