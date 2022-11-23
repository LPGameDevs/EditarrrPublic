namespace _Project.Scripts.CorgiExtension
{
    public class TOLevelSelector : LevelSelector
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
