using Singletons;
using UnityEngine;

public class BindCameraToPlayer : MonoBehaviour
{
    bool _playerFound = false;

    void Update()
    {
        if (!_playerFound)
        {
            // find the player object
            Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player != null)
            {
                Camera.main.transform.position = player.position;
                _playerFound = true;
                // set the parent of this (the camera follow target object) as a child of the player object
                Transform cameraFollowTarget = transform.parent;
                cameraFollowTarget.transform.SetParent(player);
                cameraFollowTarget.localPosition = Vector3.zero;
                // clean up this object/script
                Destroy(gameObject);
            }
            else
            {
                // The level cannot be played if there is no player.
                Debug.LogWarning("No player object found");
                SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.CreateLevelSceneName);
            }
        }
    }
}
