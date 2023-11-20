using Editarrr.LevelEditor;
using System;
using UnityEngine;

namespace Editarrr.Level
{
    public class ChannelOverlay : MonoBehaviour, IOverlay
    {
        [field: SerializeField] private ChannelOverlayData Data { get; set; }


        [field: SerializeField] private SpriteRenderer SpriteRenderer { get; set; }

        Transform IOverlay.Transform => this.transform;

        IChannelUser ChannelUser { get; set; }

        public void SetTarget(IChannelUser channelUser)
        {
            this.ChannelUser = channelUser;
            this.ChannelUser.OnChannelChanged += this.ChannelUser_OnChannelChanged;
            this.UpdateSprite(this.ChannelUser.Channel);
            // Set Channel Sprite
            // Subscribe to Channel event
        }

        private void ChannelUser_OnChannelChanged(int channel)
        {
            Debug.Log($"Channel changed {channel}");
            this.UpdateSprite(channel);
        }

        private void UpdateSprite(int channel)
        {
            var channelInfo = this.Data.ChannelInfos[channel];
            this.SpriteRenderer.sprite = channelInfo.Sprite;
            // this.SpriteRenderer.color = channelInfo.Color;
        }
    }
}
