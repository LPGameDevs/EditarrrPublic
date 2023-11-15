using Editarrr.Audio;
using Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelFilterHandler : MonoBehaviour
{
    public static Action<UserTagType> OnLabelFilterChanged;

    [SerializeField] UISelectableAugment selectableAugment;
    [SerializeField] UserTagType tagType;
    [SerializeField] Image symbol;

    private void OnEnable()
    {
        OnFilterStateChanged(PreferencesManager.Instance.GetUserTypeTag());
        OnLabelFilterChanged += OnFilterStateChanged;
    }

    private void OnDisable()
    {
        OnLabelFilterChanged -= OnFilterStateChanged;
    }

    public void OnHandlerClicked()
    {
        OnLabelFilterChanged?.Invoke(tagType);
        PreferencesManager.Instance.SetUserTypeTag(tagType);
        AudioManager.Instance.PlayAudioClip(AudioManager.MEDIUM_CLICK_CLIP_NAME);
    }

    private void OnFilterStateChanged(UserTagType activeTagType)
    {
        bool isActive = (tagType == activeTagType);
        symbol.enabled = isActive;
        selectableAugment.LockIn(isActive);
    }
}
