using Editarrr.LevelEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Editarrr.UI.LevelEditor.LevelEditorScreen.TileSelectionComponent;

namespace Editarrr.UI.LevelEditor
{
    public class Tiletip : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private Sprite _checkmark;
        [SerializeField] private Sprite _cross;

        [SerializeField] private GameObject _container;

        [SerializeField] private TextMeshProUGUI _tileName;
        [SerializeField] private TextMeshProUGUI _tileDescription;

        [SerializeField] private Image _rotatable;
        [SerializeField] private Image _qonfigurable;

        private RectTransform _rectTransform;
        private readonly float _setInvisibleTimerMax = 0.2f;
        private float _setInvisibleTimer = 0;
        private bool _toBeSetInvisible;
        private Vector3 _yCorrection = new(0, 50, 0);

        #endregion

        #region Methods

        private void OnEnable()
        {
            TileButton.OnTileButtonHover += SetUIElements;
            TileButton.OnTileButtonExit += SetInvisible;
        }

        private void OnDisable()
        {
            TileButton.OnTileButtonHover -= SetUIElements;
            TileButton.OnTileButtonExit -= SetInvisible;
        }

        private void Awake()
        {
            _container.SetActive(false);
            _rectTransform = GetComponent<RectTransform>();
            _setInvisibleTimer = _setInvisibleTimerMax;
        }

        private void SetUIElements(EditorTileData tileData)
        {
            if (tileData != null)
            {
                _container.SetActive(true);
                _toBeSetInvisible = false;
                _setInvisibleTimer = _setInvisibleTimerMax;

                _tileName.text = tileData.Name;
                _tileDescription.text = tileData.Description;

                _rotatable.sprite = tileData.Tile.CanRotate ? _checkmark : _cross;
                _qonfigurable.sprite = tileData.Config != null ? _checkmark : _cross;
            }
        }

        private void SetInvisible()
        {
            _toBeSetInvisible = true;
        }

        private void Update()
        {
            var mousePosition = UnityEngine.Input.mousePosition;
            var correctedPosition = mousePosition - _yCorrection;
            _rectTransform.position = correctedPosition;

            if (_toBeSetInvisible)
                _setInvisibleTimer -= Time.deltaTime;

            if (_setInvisibleTimer <= 0)
                _container.SetActive(false);
        }

        #endregion
    }
}
