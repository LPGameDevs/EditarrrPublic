using System;
using UnityEngine;

namespace Yanniboi.Twitch
{
    [RequireComponent(typeof(TwitchIRC))]
    public class TwitchConnect : MonoBehaviour
    {
        /**
         * When we receive a new message notify any listeners.
         *
         * @param string user
         * @param string message
         */
        public static event Action<string, string> OnMessageReceived;

        private bool _advancedMessages = false;
        private TwitchIRC _irc;

        void OnChatMsgRecieved(string rawMessage)
        {
            _irc.DebugLog(rawMessage);

            // Message format:
            // :v0lt13!v0lt13@v0lt13.tmi.twitch.tv PRIVMSG #tarrenam :wassup?
            // user: v0lt13 channel: tarrenam message: wassup?

            // Advanced message format:
            // @badge-info=;badges=broadcaster/1;client-nonce=7266187d840d253af37b3700e7372fb0;color=;display-name=yanni_boi;emotes=;flags=;id=2bbbba39-ddaf-4bbb-a358-c02a9ff1d835;mod=0;room-id=131656348;subscriber=0;tmi-sent-ts=1630663707799;turbo=0;user-id=131656348;user-type= :yanni_boi!yanni_boi@yanni_boi.tmi.twitch.tv PRIVMSG #tarrenam :test
            // user: yanniboi channel: tarrenam message: test?

            // Parse from buffer.
            // @todo Research string cultures.
            int messageStart = rawMessage.IndexOf("PRIVMSG #");
            string messageText = rawMessage.Substring(messageStart + _irc.channelName.Length + 11);
            string user = rawMessage.Substring(1, rawMessage.IndexOf('!') - 1);

            // Invoke new message event.
            OnMessageReceived?.Invoke(user, messageText);
        }

        public void SendMessage(string message)
        {
            _irc.SendMsg(message);
        }

        void Start()
        {
            _irc = GetComponent<TwitchIRC>();

            if (_advancedMessages)
            {
                EnableAdvancedMessages();
            }

            _irc.messageRecievedEvent.AddListener(OnChatMsgRecieved);
        }

        private void EnableAdvancedMessages()
        {
            // register for additional data such as emote-ids, name color etc.
            _irc.SendCommand("CAP REQ :twitch.tv/tags");
        }

        private void OnEnable()
        {
            CommandHandler.OnSendMessage += SendMessage;
        }

        private void OnDisable()
        {
            CommandHandler.OnSendMessage -= SendMessage;
        }
    }
}
