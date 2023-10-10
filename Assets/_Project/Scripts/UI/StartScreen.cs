using Singletons;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.LevelSelectionSceneName);
    }
}
