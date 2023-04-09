using System.Collections;
using System.Collections.Generic;
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
                Debug.LogWarning("No player object found");
            }
        }
    }
}
