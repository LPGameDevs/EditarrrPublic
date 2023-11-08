namespace Yanniboi.Twitch.Example
{
    public class ExampleJoinCommand : CommandBase
    {
        public override string CommandName => "!join";
    
        public static TwitchMessage JoinMessage = new TwitchMessage("Join Success Message", "{0} has joined the game!");

        protected override void DoExecute(string user, string message)
        {
            SendMessage(JoinMessage, user);
        }

    }
}
