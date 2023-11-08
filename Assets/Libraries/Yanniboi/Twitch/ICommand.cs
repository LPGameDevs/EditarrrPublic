namespace Yanniboi.Twitch
{
    public interface ICommand
    {
        public string CommandName { get; }
        public void Execute(string user, string message);
    }
}
