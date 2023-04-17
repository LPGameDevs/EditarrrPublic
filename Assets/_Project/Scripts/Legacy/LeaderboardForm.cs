using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CorgiExtension;
using LevelEditor;
using TMPro;
using UnityEngine;

namespace Legacy
{
    public class LeaderboardForm : MonoBehaviour
    {
        public TMP_Text Title;
        public TMP_Text Leaders;

        private Popup _popup;
        private string _code;

        private void Awake()
        {
            _popup = GetComponent<Popup>();
        }

        private void SetCode(string code)
        {
            _code = code;
        }

        public void OpenPopup()
        {
            Title.text = $"{_code.ToUpper()} LEADERBOARD";
            Leaders.gameObject.SetActive(true);
            StringBuilder sb = new StringBuilder();

            //@todo add scores.

            Leaders.text = sb.ToString();
            _popup.Open();
        }

        public void ClosePopup()
        {
            Leaders.gameObject.SetActive(false);
            _popup.Close();
        }


        private void OnEnable()
        {
            EditorLevel.OnLeaderboardRequest += SetCode;
        }

        private void OnDisable()
        {
            EditorLevel.OnLeaderboardRequest -= SetCode;
        }
    }
}
