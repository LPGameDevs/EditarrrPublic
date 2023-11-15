using Editarrr.Audio;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class UISelectableAugment : MonoBehaviour
{
    public string NameTag {  get => _nameTag; }

    [SerializeField, HideInInspector] Selectable _selectableComponent;
    [SerializeField] AudioClip _hoverClip;
    [SerializeField] Shadow _shadow1, _shadow2;
    [SerializeField] Outline _outline1, _outline2;
    [SerializeField] string _nameTag = string.Empty;

    private void Reset()
    {
        _selectableComponent = GetComponent<Selectable>();
        
    }

    public void PlayHoverSound()
    {
        if(_hoverClip != null)
            AudioManager.Instance.PlayAudioClip(_hoverClip);
        else
            AudioManager.Instance.PlayAudioClip(AudioManager.MINOR_CLICK_CLIP_NAME);
    }

    public void LockIn(bool activateLock)
    {
        _outline1.enabled = activateLock;
        _outline2.enabled = activateLock;
        _shadow1.enabled = !activateLock;
        _shadow2.enabled = !activateLock;     
    }
}
