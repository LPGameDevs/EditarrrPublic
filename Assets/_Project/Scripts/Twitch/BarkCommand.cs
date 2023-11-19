using System;
using System.Collections.Generic;
using Editarrr.Audio;
using UnityEngine;
using Yanniboi.Twitch;

namespace Twitch
{
    public class BarkCommand : CommandBase
    {
        private Dictionary<string, string> _registeredBarks = new Dictionary<string, string>();

        public BarkCommand()
        {
            _registeredBarks.Add("booty", "Booty01");
            _registeredBarks.Add("arrr", "Arrr01");
            _registeredBarks.Add("yohoho", "YoHoHo01");
            _registeredBarks.Add("yarr", "Yarr01");
            _registeredBarks.Add("stupid", "stupid");
        }

        public override string CommandName => "!bark";



        protected override void DoExecute(string user, string message)
        {
            if (message == "!bark")
            {
                // Send list of barks.
                // Maybe hide "stupid" as secret bark.
                string response = "The list of available barks is: ";
                foreach (var bark in this._registeredBarks)
                {
                    if (bark.Key == "stupid")
                    {
                        continue;
                    }

                    response += bark.Key + " ";
                }

                this.SendMessage(new TwitchMessage("Bark help", response));
                return;
            }

            foreach (var bark in _registeredBarks)
            {
                if (message.Contains(bark.Key))
                {
                    AudioManager.Instance.PlayAudioClip(bark.Value);
                    return;
                }
            }
        }

    }
}
