namespace CorgiExtension
{
    public class EditorLevelSelector : LevelSelector
    {

        public virtual void GoToLevel()
        {
            LevelManager.Instance.GotoLevel(LevelName);
        }


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
