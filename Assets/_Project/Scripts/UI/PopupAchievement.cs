using Editarrr.UI;
using Singletons;
using UnityEngine;

namespace UI
{

    [CreateAssetMenu(fileName = "NewAchievementPopup", menuName = "Achievements/new Achievement Popup")]
    public class PopupAchievement : ScriptableObject
    {
        [SerializeField] public GameAchievement Type;
        [SerializeField] protected string TitleText;

        public virtual string GetTitleText()
        {
            return TitleText;
        }
    }
}
