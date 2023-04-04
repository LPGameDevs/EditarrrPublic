using Editarrr.Input;
using UnityEngine;

namespace Legacy
{
    /**
 * Allows showing hidden things for developers if required.
 */
    public class SecretMenu : MonoBehaviour
    {
        public GameObject hiddenObject;

        private int _keyCount;
        private bool _isHiddenObjectActive;


        [field: SerializeField] private InputValue SecretKey { get; set; }

        private void Start()
        {
            _keyCount = 0;

            if (hiddenObject.activeSelf)
            {
                _isHiddenObjectActive = true;
            }
        }

        void Update()
        {
            if (_isHiddenObjectActive)
            {
                return;
            }

            // Secret key to press.
            if (SecretKey.WasPressed)
            {
                _keyCount++;
            }

            // Press the secret key 5 times to show.
            if (_keyCount >= 5)
            {
                _isHiddenObjectActive = true;
                if (hiddenObject)
                {
                    hiddenObject.SetActive(true);
                }
            }
        }
    }
}
