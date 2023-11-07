using Yanniboi.Twitch;

namespace Twitch
{
    public class GameCommand : CommandBase
    {
        public override string CommandName => "!game";

        public static TwitchMessage GameMessage = new TwitchMessage("What is this game?", "This game is called Editarrr - Find it here: https://store.steampowered.com/app/2609410/Editarrr/");

        protected override void DoExecute(string user, string message)
        {
            SendMessage(GameMessage);
        }

    }
}
