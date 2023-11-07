using System;

namespace Yanniboi.Twitch
{
    public abstract class CommandBase : ICommand
    {
        public static event Action<string, string> OnCommandError;
        public static event Action<string, string> OnCommandMessage;

        public abstract string CommandName { get; }

        // @todo Add Aliases.
    
        public virtual void Execute(string user, string message)
        {
            if (IsValid(user, message, out TwitchMessage error))
            {
                DoExecute(user, message);
            }
            else {
                SendError(error);
            }

            Cleanup();
        }

        protected virtual void Cleanup() { }

        protected abstract void DoExecute(string user, string message);

        protected virtual bool IsValid(string user, string message, out TwitchMessage error)
        {
            error = TwitchMessage.NoError;
            return true;
        }

        protected void SendError(TwitchMessage error)
        {
            string message = "";

            if (error.IsError)
            {
                message = error.Text;
            }

            if (message.Length > 0)
            {
                OnCommandError?.Invoke(error.Name, message);
            }
        }

        protected void SendMessage(TwitchMessage message, string var = "")
        {
            string messageText = message.Text;
            if (messageText.Length > 0)
            {
                OnCommandMessage?.Invoke(message.Name, messageText);
            }
        }
    }
    
    public class TwitchMessage {
        public bool IsError {get; private set;}
        public string Name {get; private set;}
        public string Text {get; private set;}

        public TwitchMessage(string name, string description) {
            Name = name;
            Text = description;
        }
    
        public TwitchMessage(string name, string description, bool isError) {
            Name = name;
            Text = description;
            IsError = isError;
        }

        public static TwitchMessage NoMessage = new TwitchMessage("", "");
        public static TwitchMessage NoError = new TwitchMessage("", "", true);
    }
}
