using Editarrr.LevelEditor;
using Editarrr.Managers;
using UnityEngine;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "Overlay Manager", menuName = "Managers/Level/new Overlay Manager")]
    public class OverlayManager : ManagerComponent
    {
        [field: SerializeField] private ChannelOverlay ChannelOverlay { get; set; }

        public bool Create(TileConfig tileConfig, out IOverlay overlay)
        {
            if (tileConfig is IChannelUser channelUser)
            {
                ChannelOverlay channelOverlay = GameObject.Instantiate(this.ChannelOverlay);
                channelOverlay.SetTarget(channelUser);
                overlay = channelOverlay;
                return true;
            }

            overlay = null;
            return false;
        }
    }
}
