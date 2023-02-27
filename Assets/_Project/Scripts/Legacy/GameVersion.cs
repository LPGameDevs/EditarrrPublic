using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Legacy
{
    public class GameVersion : MonoBehaviour
    {

        private TMP_Text _tmpText;
        private Text _text;

        private void Start()
        {
            _tmpText = GetComponent<TMP_Text>();
            _text = GetComponent<Text>();

            if (_tmpText != null)
            {
                _tmpText.text = Application.version;
            }
            else if (_text != null)
            {
                _text.text = Application.version;
            }
        }
    }
}
