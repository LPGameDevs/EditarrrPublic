using Editarrr.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortingSelector : MonoBehaviour
{
    public enum SortingState
    {
        Inactive,
        Ascending,
        Descending
    }

    public SortingState CurrentState {  get; private set; }
    public string SortingCriterionName { get => _sortingCriterionName; }


    [SerializeField] UISelectableAugment uiSelectable;
    [SerializeField] Image _symbolDisplay;
    [SerializeField] Sprite _inactiveSymbol, _ascendingSymbol, _descendingSymbol;
    [SerializeField] Color _inactiveColor, _activeColor;
    [SerializeField] string _sortingCriterionName;

    public static Action<SortingState, SortingSelector> OnStateChanged;

    private void OnEnable()
    {
        CurrentState = SortingState.Inactive;
        OnStateChanged += SetSortingState;
    }

    private void OnDisable()
    {
        CurrentState = SortingState.Inactive;
        OnStateChanged -= SetSortingState;
    }

    public void CycleSortingState()
    {
        AudioManager.Instance.PlayAudioClip(AudioManager.MEDIUM_CLICK_CLIP_NAME);
        int newState = (int)(CurrentState + 1) % 3;
        SetSortingState((SortingState)newState, null);
        OnStateChanged?.Invoke(SortingState.Inactive, this);
    }

    public void SetSortingState(SortingState incomingState, SortingSelector notifier)
    {
        if (this == notifier)
            return;

        CurrentState = incomingState;
        switch (incomingState)
        {
            case SortingState.Inactive:
                {
                    _symbolDisplay.color = _inactiveColor;
                    _symbolDisplay.sprite = _inactiveSymbol;
                    uiSelectable.LockIn(false);
                    break;
                }
            case SortingState.Ascending:
                {
                    _symbolDisplay.color = _activeColor;
                    _symbolDisplay.sprite = _ascendingSymbol;
                    uiSelectable.LockIn(true);
                    break;
                }
            case SortingState.Descending:
                {
                    _symbolDisplay.color = _activeColor;
                    _symbolDisplay.sprite = _descendingSymbol;
                    uiSelectable.LockIn(true);
                    break;
                }
        }
    }
}
