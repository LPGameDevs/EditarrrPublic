using System;
using Editarrr.Level;
using Gameplay.GUI;
using Singletons;
using UnityEngine;

public class BindCameraToPlayer : MonoBehaviour
{
    bool _playerFound = false;

    private Camera _camera;
    private Transform _cameraFollowTarget;
    private Transform _player;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraFollowTarget = transform.parent;
    }

    void Update()
    {
        if (_playerFound)
        {
            return;
        }
        // find the player object
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (_player != null)
        {
            _playerFound = true;
        }
        else
        {
            // The level cannot be played if there is no player.
            Debug.LogWarning("No player object found");
            SceneTransitionManager.Instance.GoToScene(SceneTransitionManager.CreateLevelSceneName);
        }
    }

    private void SetStartingPosition(Vector3 position)
    {
        _cameraFollowTarget.position = position;
    }

    private void StartFollowingPlayer()
    {
        // set the parent of this (the camera follow target object) as a child of the player object
        _cameraFollowTarget.transform.SetParent(_player);
        _cameraFollowTarget.localPosition = Vector3.zero;
        // clean up this object/script
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        LevelPlayManager.OnCameraTileSet += SetStartingPosition;
        GameplayGuiManager.OnGameStarted += StartFollowingPlayer;
    }

    private void OnDisable()
    {
        LevelPlayManager.OnCameraTileSet -= SetStartingPosition;
        GameplayGuiManager.OnGameStarted -= StartFollowingPlayer;
    }
}
