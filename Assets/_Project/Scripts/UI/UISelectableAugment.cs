using Editarrr.Audio;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class UISelectableAugment : MonoBehaviour
{
    [SerializeField, HideInInspector] private Selectable selectableComponent;
    [SerializeField] AudioClip hoverClip;

    private void Reset()
    {
        selectableComponent = GetComponent<Selectable>();
    }

    public void PlayHoverSound()
    {
        if(hoverClip != null)
            AudioManager.Instance.PlayAudioClip(hoverClip);
        else
            AudioManager.Instance.PlayAudioClip(AudioManager.MINOR_CLICK_CLIP_NAME);
    }
}
