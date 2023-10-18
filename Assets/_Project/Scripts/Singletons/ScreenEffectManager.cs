using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffectManager : UnityPersistentSingleton<ScreenEffectManager>
{
    [field: SerializeField, Tooltip("Used for transition effects like fading")] private Image ScreenCover { get; set; }

    [SerializeField] Animator _animator;
    [SerializeField] AudioSource _audioSource;

    public static readonly string EFFECT_FadeToBlack = "FadeToBlack";
    public static readonly string EFFECT_FadeFromBlack = "FadeFromBlack";

    private bool _effectActive;

    /// <summary>
    /// Immediately plays an effect based on premade animation clips
    /// </summary>
    /// <param name="effectName">Get names from the EffectManager itself, starting with EFFECT</param>
    /// <param name="interruptCurrentEffect">If false, the incoming call will be ignored completely (no queueing)</param>
    public void PlayEffect(string effectName, bool interruptCurrentEffect)
    {
        if(_effectActive && !interruptCurrentEffect)
            return;

        _audioSource.Stop();
        _effectActive = true;
        _animator.SetTrigger(effectName);
    }

    public void FadeScreen(Color baseColor, Color targetColor, float fadeDuration)
    {
        ScreenCover.color = baseColor;
        StartCoroutine(FadeCoroutine(targetColor, fadeDuration));
    }

    private IEnumerator FadeCoroutine(Color targetColor, float fadeDuration)
    {
        float timeLimit = fadeDuration + Time.time;
        while ((ScreenCover.color != targetColor) && Time.time < timeLimit)
        {
            ScreenCover.color = Color.Lerp(ScreenCover.color, targetColor, Time.deltaTime);

            yield return null;
        }
    }

    public void ResetActiveFlag() => _effectActive = false;
}