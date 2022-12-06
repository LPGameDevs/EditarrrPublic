namespace CorgiExtension
{
    public class EditorLevelSelector : LevelSelector
    {


        public void GoToLevelSelection()
        {
            LevelManager.Instance.GotoLevel("EditorSelection");
        }

        protected void OnEnable()
        {
            WinMenu.OnScoreSubmitted += GoToLevelSelection;
        }

        protected void OnDisable()
        {
            WinMenu.OnScoreSubmitted -= GoToLevelSelection;
        }
    }
}
