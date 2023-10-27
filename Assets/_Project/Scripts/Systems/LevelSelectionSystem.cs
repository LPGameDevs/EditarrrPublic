using Browser;
using Editarrr.Misc;
using Editarrr.Systems;
using UI;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    public class LevelSelectionSystem : SystemComponent<LevelSelectionManager>
    {
        private const string Documentation = "This System is for managing level selection.\r\n" +
                                             "It will add the relevant manager and connect it with scene dependencies.\r\n";

        [field: SerializeField, Info(Documentation)] public LevelSelectionLoader LevelLoader { get; private set; }
        [field: SerializeField] public LeaderboardForm Leaderboard { get; private set; }
        [field: SerializeField] public Canvas ModalCanvas { get; private set; }
        [field: SerializeField] public ModalPopupConfirmation UploadModal { get; private set; }
        [field: SerializeField] public ModalPopupConfirmation DeleteModal { get; private set; }
        [field: SerializeField] public ModalPopup InvalidModal { get; private set; }
        [field: SerializeField] public ModalPopup IncompleteModal { get; private set; }
        [field: SerializeField] public AchievementPopupBlock Achievement { get; private set; }

        protected override void PreAwake()
        {
            this.Manager.SetLevelLoader(this.LevelLoader);
            this.Manager.SetLeaderboard(this.Leaderboard);
            this.Manager.SetCanvas(this.ModalCanvas);
            this.Manager.SetUploadModal(this.UploadModal);
            this.Manager.SetDeleteModal(this.DeleteModal);
            this.Manager.SetInvalidModal(this.InvalidModal);
            this.Manager.SetIncompleteModal(this.IncompleteModal);
            this.Manager.SetAchievementBlock(this.Achievement);
        }
    }
}
