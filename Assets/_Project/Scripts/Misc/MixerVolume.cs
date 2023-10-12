using Editarrr.Audio;
using Singletons;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Misc
{
    public class MixerVolume : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private string _volumeParameter = "MasterVolume";

        private Slider _slider;
        private AudioSource _audio;
        private bool _playSound = false;
        private float _playSoundDelay = 0.5f;
        private float _playSoundTimer = 0;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _audio = GetComponent<AudioSource>();
        }

        private void Start()
        {
            float value = PreferencesManager.Instance.GetSlider(this._volumeParameter);
            _slider.onValueChanged.AddListener(SetLevel);
            _slider.value = value;
            SetLevel(value);
        }


        private void Update()
        {
            if (_playSound)
            {
                _playSoundTimer += Time.deltaTime;
                if (_playSoundTimer >= _playSoundDelay)
                {
                    _playSound = false;
                    _playSoundTimer = 0;
                    AudioManager.Instance.PlayAudioClip(_audio.clip);
                }
            }
        }

        public void SetLevel(float sliderValue)
        {
            PreferencesManager.Instance.SetSlider(this._volumeParameter, sliderValue);


            Debug.Log("test");
            float value = Mathf.Log10(sliderValue) * 20;
            _mixer.SetFloat(_volumeParameter, value);

            Debug.Log(value);


            if (_audio != null)
            {
                _playSound = true;
            }
        }
    }
}