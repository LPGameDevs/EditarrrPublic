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

        public void OpenPopup(CommentsResponseData response)
        {
            Title.text = $"{_code.ToUpper()} LEADERBOARD";
            Leaders.gameObject.SetActive(true);
            StringBuilder sb = new StringBuilder();

            List<CommentResponseData> comments = response.comments.ToList();
            comments.Sort((x, y) => String.Compare(x.time, y.time, StringComparison.Ordinal));

            int i = 0;
            foreach (CommentResponseData comment in comments)
            {
                i++;
                if (i > 5) break;
                sb.AppendLine($"{comment.time} by {comment.user}");
            }

            Leaders.text = sb.ToString();
            _popup.Open();
        }

        public void ClosePopup()
        {
            Leaders.gameObject.SetActive(false);
            _popup.Close();
        }

        private void DatabaseRequestComplete(DatabaseRequestType type, CommentsResponseData response)
        {
            if (type != DatabaseRequestType.GetLevelComments)
            {
                return;
            }

            OpenPopup(response);
        }

        private void OnEnable()
        {
            EditorLevelStorage.OnCommentsRequestComplete += DatabaseRequestComplete;
            EditorLevel.OnLeaderboardRequest += SetCode;
        }

        private void OnDisable()
        {
            EditorLevelStorage.OnCommentsRequestComplete -= DatabaseRequestComplete;
            EditorLevel.OnLeaderboardRequest -= SetCode;
        }
    }
}
