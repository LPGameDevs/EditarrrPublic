using Singletons;
using UnityEngine;
using Yanniboi.Twitch;

namespace Twitch
{
    public class EditarrrTwitchNotifications : MonoBehaviour
    {
        private TwitchConnect _twitch;

        private void Awake()
        {
            _twitch = GetComponent<TwitchConnect>();
        }

        protected void SendTwitchMessage(string message)
        {
            _twitch.SendMessage(message);
        }

        private void OnEnable()
        {
            TwitchManager.OnSendNotification += SendTwitchMessage;
        }

        private void OnDisable()
        {
            TwitchManager.OnSendNotification -= SendTwitchMessage;
        }
    }
}
