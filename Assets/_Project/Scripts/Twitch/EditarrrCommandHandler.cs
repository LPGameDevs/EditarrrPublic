using System;
using UnityEngine;
using Yanniboi.Twitch;

namespace Twitch
{
    public class EditarrrCommandHandler: CommandHandler
    {

        private void Start()
        {
            _enabledCommands.Add(new GameCommand());
            _enabledCommands.Add(new JumpCommand());
            _enabledCommands.Add(new BouncyCommand());
            _enabledCommands.Add(new BarkCommand());
        }

        private void SendMessages(string messageName, string message) {
            SendTwitchMessage(message);
            Debug.Log(String.Format("Twitch command message sent: {0}", messageName));

        }

        private void LogErrors(string errorName, string error) {
            SendTwitchMessage(error);
            Debug.Log(String.Format("Twitch command error occured: {0}", errorName));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            CommandBase.OnCommandError += LogErrors;
            CommandBase.OnCommandMessage += SendMessages;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            CommandBase.OnCommandError -= LogErrors;
            CommandBase.OnCommandMessage -= SendMessages;
        }
    }
}
