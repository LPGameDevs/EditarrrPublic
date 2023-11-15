using System;
using Yanniboi.Twitch;

namespace Twitch
{
    public class BouncyCommand : CommandBase
    {
        public static event Action OnTwitchJumpForever;

        public override string CommandName => "!bouncy";

        protected override void DoExecute(string user, string message)
        {
            OnTwitchJumpForever?.Invoke();
        }

    }
}
