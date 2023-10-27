using Editarrr.UI;
using Singletons;
using UnityEngine;

namespace UI
{

    [CreateAssetMenu(fileName = "NewAchievementPopup", menuName = "Achievements/new Achievement Popup")]
    public class PopupAchievement : ScriptableObject, IModalPopup
    {
        [SerializeField] protected GameAchievement Type;
        [SerializeField] protected string TitleText;
        [SerializeField] protected string ContentText;
        [SerializeField] protected string CloseText;
        [SerializeField] protected AchievementPopupBlock Prefab;

        public virtual void Open(Transform parent = null, bool alwaysShow = false)
        {
            if (!alwaysShow && PreferencesManager.Instance.IsModalEventTracked(this.name, ModalPopupAction.Close))
            {
                this.Close();
                return;
            }

           if (parent == null)
            {
                parent = FindObjectOfType<Canvas>().transform;
            }
            var popup = Instantiate(Prefab, parent);
            popup.Setup(this);

            PreferencesManager.Instance.SetModalEventTracked(this.name, ModalPopupAction.Open);
        }

        public virtual void Close()
        {
        }

        public virtual string GetTitleText()
        {
            return TitleText;
        }

        public virtual string GetContentText()
        {
            return ContentText;
        }

        public virtual string GetCloseText()
        {
            return CloseText;
        }
    }
}
