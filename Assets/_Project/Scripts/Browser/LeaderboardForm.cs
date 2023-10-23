using Level.Storage;
using UnityEngine;

namespace Browser
{
    public class LeaderboardForm : MonoBehaviour
    {
        public Transform Rows;
        public LeaderboardRow RowPrefab;

        private string _code;

        public void SetCode(string code)
        {
            _code = code;
        }

        public void SetScores(ScoreStub[] scoreStubs)
        {
            foreach (Transform child in Rows) {
                Destroy(child.gameObject);
            }

            int i = 0;
            foreach (ScoreStub scoreStub in scoreStubs)
            {
                i++;
                if (i > 5) break;
                LeaderboardRow row = Instantiate(RowPrefab, Rows);
                row.SetRow(_code, i.ToString(), scoreStub.Score, scoreStub.CreatorName);
            }
        }

        public void CloseButtonPressed()
        {
            foreach (Transform child in Rows) {
                Destroy(child.gameObject);
            }
            gameObject.SetActive(false);
        }
    }
}