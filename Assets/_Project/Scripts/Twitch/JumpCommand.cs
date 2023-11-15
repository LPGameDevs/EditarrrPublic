using System;
using Yanniboi.Twitch;

namespace Twitch
{
    public class JumpCommand : CommandBase
    {
        public static event Action OnTwitchJump;

        public override string CommandName => "!jump";

        protected override void DoExecute(string user, string message)
        {
            OnTwitchJump?.Invoke();
        }

    }
}
