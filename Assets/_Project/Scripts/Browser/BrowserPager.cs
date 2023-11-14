using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrowserPager : MonoBehaviour
{
    public static event Action<int, PagerRequestResultHasMore> OnBrowserPagerUpdated;
    public delegate void PagerRequestResultHasMore(bool hasMore);


    [SerializeField] private TMP_Text _currentText;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    private int _current = 1;

    private void Start()
    {
        UpdateGui();
        OnBrowserPagerUpdated?.Invoke(0, NextRequestResult);
    }

    private void UpdateGui()
    {
        _currentText.text = _current.ToString();

        _previousButton.gameObject.SetActive(true);
        if (_current < 2)
        {
            _previousButton.gameObject.SetActive(false);
        }
    }

    public void ButtonPressedPrevious()
    {
        _current--;
        UpdateGui();
        OnBrowserPagerUpdated?.Invoke(-1, PreviousRequestResult);
        Debug.Log("Previous");
    }

    public void ButtonPressedNext()
    {
        _current++;
        UpdateGui();
        OnBrowserPagerUpdated?.Invoke(1, NextRequestResult);
        Debug.Log("Next");
    }

    private void NextRequestResult(bool hasMore)
    {
        _nextButton.gameObject.SetActive(true);
        if (!hasMore)
        {
            _nextButton.gameObject.SetActive(false);
        }
    }

    private void PreviousRequestResult(bool _)
    {
        // Show the next button.
        _nextButton.gameObject.SetActive(true);
    }
}
