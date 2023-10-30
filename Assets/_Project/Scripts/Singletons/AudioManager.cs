using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Editarrr.Audio
{
    public class AudioManager : UnityPersistentSingleton<AudioManager>
    {
        public AudioSource BgmSourceOne { get => bgmSourceOne; }
        public AudioSource BgmSourceTwo { get => bgmSourceTwo; }

        [Header("SFX")]
        [SerializeField] AudioSfxPool sfxPool;
        [SerializeField] Transform sfxSourceParent;

        [Header("Music")]
        [SerializeField] AudioSource bgmSourceOne, bgmSourceTwo;
        [SerializeField] bool playBGM;

        AudioSource[] sfxSources;
        int currentSFXSource = 0;
        [SerializeField] MultiSound[] multiSounds;
        private int currentSource = 0;

        Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();
        Dictionary<string, MultiSound> multiSoundDict = new Dictionary<string, MultiSound>();

        #region Clip names
        public const string AFFIRMATIVE_CLIP_NAME = "affirmative";
        public const string NEGATIVE_CLIP_NAME = "negative";
        public const string BUTTONCLICK_CLIP_NAME = "buttonClick";
        public const string LEVEL_DESTROYED_CLIP_NAME = "levelDestroyed";
        public const string LEVEL_UPLOADED_CLIP_NAME = "levelUploaded";
        public const string ATTENTION_CLIP_NAME = "attention";
        public const string DENIED_CLIP_NAME = "denied";
        public const string WARNING_CLIP_NAME = "warning";
        public const string YOHOHO_CLIP_NAME = "YoHoHo1";
        public const string BOOTY_CLIP_NAME = "Booty01";
        #endregion

        private void Start()
        {
            InitSfxPoolAndSources();

            if (playBGM && !bgmSourceOne.isPlaying) bgmSourceOne.Play();
            if (playBGM && !bgmSourceTwo.isPlaying) bgmSourceTwo.Play();
            BgmSourceTwo.volume = 0f;
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

            foreach (MultiSound sound in multiSounds)
            {
                multiSoundDict.Add(sound.name, sound);
            }
        }

        public void FadeVolume(AudioSource source, float fadeLength, float targetVolume)
        {
            StartCoroutine(CoroutineFade(source, fadeLength, targetVolume));
        }

        private IEnumerator CoroutineFade(AudioSource source, float fadeLength, float targetVolume)
        {
            float timeLimit = Time.time + fadeLength;
            float startingVolume = source.volume;
            while (Time.time < timeLimit)
            {
                float t = 1 - ((timeLimit - Time.time) / fadeLength);
                source.volume = Mathf.Lerp(startingVolume, targetVolume, t);
                yield return new WaitForSecondsRealtime(0f);
            }

            source.volume = targetVolume;
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

        public void PlayAudioClip(AudioClip clip)
        {
            sfxSources[currentSFXSource].PlayOneShot(clip);
            currentSFXSource = (currentSFXSource + 1) % sfxSources.Length;
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

        public void PlayShuffledSound(string soundName, float pitchVariance, float volumeVariance)
        {
            if (multiSoundDict.ContainsKey(soundName))
            {
                float pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
                float volume = 1f + Random.Range(-volumeVariance, volumeVariance);
                sfxSources[currentSource].pitch = pitch;
                sfxSources[currentSource].volume = volume;
                var temp = multiSoundDict[soundName].GetShuffledClip();
                if (temp != null)
                {
                    sfxSources[currentSource].PlayOneShot(temp);
                    currentSource = (currentSource + 1) % sfxSources.Length;
                }

            }
            else
            {
                Debug.Log("AudioManager: AudioClip " + soundName + " not found in multi sound dictionary.");
            }
        }
    }
}
