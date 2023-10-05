using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [RequireComponent(typeof(AudioSource))]
    public class EditorFeedback : MonoBehaviour
    {
        enum EditorAction 
        {
            SelectedTile,
            PlacedTile,
            RemovedTile
        }

        [SerializeField] AudioSource _audioSource;
        [SerializeField] AudioClip _tileSelect, _tilePlace, _tileRemove;
        [SerializeField] float _pitchMin, _pitchMax;

        float _streakTimer = 0f, _bufferTimer = 0f;
        EditorAction _lastAction;
        bool _isStreaking;

        const float streakPitchIncrement = 0.01f, _streakInterval = 1f, _bufferDuration = 0.05f;

        private void Update()
        {
            if(Time.time > _streakTimer || !_isStreaking)
                _audioSource.pitch = _pitchMin;
        }

        private void OnEnable()
        {
            _bufferTimer = Time.time + 0.5f;

            EditorTileSelectionManager.OnTileSelect += OnTileSelected;
            EditorLevelManager.OnTileSet += OnTilePlaced;
            EditorLevelManager.OnTileUnset += OnTileRemoved;
        }

        private void OnDisable()
        {
            EditorTileSelectionManager.OnTileSelect -= OnTileSelected;
            EditorLevelManager.OnTileSet -= OnTilePlaced;
            EditorLevelManager.OnTileUnset -= OnTileRemoved;
        }

        private void OnTileSelected()
        {
            PlayFeedback(_tileSelect, _pitchMin, EditorAction.SelectedTile);
        }

        private void OnTilePlaced(EditorTileData data, TileType tileType, int inLevel)
        {
            PlayFeedback(_tilePlace, _audioSource.pitch + streakPitchIncrement, EditorAction.PlacedTile);
        }

        private void OnTileRemoved(EditorTileData data, TileType tileType, int inLevel)
        {
            PlayFeedback(_tileRemove, _audioSource.pitch + streakPitchIncrement, EditorAction.RemovedTile);
        }

        private void PlayFeedback(AudioClip clip, float newPitch, EditorAction newAction)
        {
            if (Time.time < _bufferTimer)
                return;

            _bufferTimer = Time.time + _bufferDuration;

            //Track action streaks
            _isStreaking = newAction == _lastAction;
            _lastAction = newAction;
            _streakTimer = Time.time + _streakInterval;

            _audioSource.pitch = newPitch;
            _audioSource.PlayOneShot(clip);
        }
    }
}