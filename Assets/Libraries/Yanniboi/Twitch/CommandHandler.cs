using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yanniboi.Twitch
{
    public class CommandHandler: MonoBehaviour
    {
        public static event Action<string> OnSendMessage;

        protected List<ICommand> _enabledCommands = new List<ICommand>();

        private void OnReceived(string user, string message)
        {
            bool isCommand = GetCommandFromMessage(message, out ICommand command);
            // Debug.Log(isCommand);
            // Debug.Log(_enabledCommands);
            if (isCommand)
            {
                Debug.Log(command);
                command.Execute(user, message);
            }
        }

        protected void SendTwitchMessage(string message)
        {
            OnSendMessage?.Invoke(message);
        }

        private bool GetCommandFromMessage(string message, out ICommand foundCommand)
        {
            foundCommand = null;
            foreach (ICommand command in _enabledCommands)
            {
                if (message.ToLower().Contains(command.CommandName))
                {
                    foundCommand = command;
                    return true;
                }
            }

            return false;
        }

        #region Setup

        protected virtual void OnEnable()
        {
            TwitchConnect.OnMessageReceived += OnReceived;
        }


        protected virtual void OnDisable()
        {
            TwitchConnect.OnMessageReceived -= OnReceived;
        }
        #endregion
    }
}
