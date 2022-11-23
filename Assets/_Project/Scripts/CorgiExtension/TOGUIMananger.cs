using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

public class TOGUIMananger : GUIManager
{
    public static event Action OnMusicRandom;

    [Header("Bindings")]

    /// the win screen game object
    [Tooltip("the win screen game object")]
    public GameObject WinScreen;

    /// the lost screen game object
    [Tooltip("the lost screen game object")]
    public GameObject LoseScreen;

    /// Help overlay for information
    [Tooltip("Help overlay for information")]
    public GameObject HelpOverlay;


    public float WinDelay = 1f;
    public float LoseDelay = 1f;
    public float HelpOverlaySize = 10f;
    public float HelpSlideDuration = 1.5f;

    private bool _isShowingHelp;

    #region MenuHelpers

    private void CloseMenu(GameObject menu)
    {
        if (menu != null)
        {
            menu.SetActive(false);

            // @todo unpause game.
        }
    }

    private void ShowMenu(GameObject menu)
    {
        if (menu)
        {
            menu.SetActive(true);
            MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0f, 0f, false, 0f, true);
            // @todo pause game.
        }
    }

    public void CloseWinMenu()
    {
        CloseMenu(WinScreen);
    }

    public void ShowWinMenu()
    {
        WinScreen.GetComponent<Popup>().Open();
    }

    public void CloseLoseMenu()
    {
        CloseMenu(LoseScreen);
    }

    public void ShowLoseMenu()
    {
        ShowMenu(LoseScreen);
    }

    #endregion

    private void ShowWinMenuDelayed()
    {
        Invoke("ShowWinMenu", WinDelay);
    }

    private void ShowLoseMenuDelayed()
    {
        Invoke("ShowLoseMenu", LoseDelay);
    }

    public void ShuffleMusic()
    {
        OnMusicRandom?.Invoke();
    }

    public void ToggleHelp()
    {
        StartCoroutine(nameof(SlideHelp));
    }

    private IEnumerator SlideHelp()
    {
        Vector3 origin = HelpOverlay.transform.position;
        Vector3 destination = origin;
        float elapsedTime = 0;
        if (_isShowingHelp)
        {
            destination += Vector3.right * HelpOverlaySize;
        }
        else
        {
            destination += Vector3.left * HelpOverlaySize;
        }

        while (elapsedTime < HelpSlideDuration)
        {
            HelpOverlay.transform.position = Vector3.Lerp(origin, destination, elapsedTime / HelpSlideDuration);
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }

        HelpOverlay.transform.position = destination;
        _isShowingHelp = !_isShowingHelp;
    }

    protected  void OnEnable()
    {
        LevelWin.OnLevelWin += ShowWinMenu;
    }

    protected  void OnDisable()
    {
        LevelWin.OnLevelWin -= ShowWinMenu;
    }
}
