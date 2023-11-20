using UnityEngine;

namespace Editarrr.Level
{
    [CreateAssetMenu(fileName = "ChannelOverlayData", menuName = "Data/Gameplay/new ChannelOverlay Data")]
    public class ChannelOverlayData : ScriptableObject
    {
        [field: SerializeField] public ChannelInfo[] ChannelInfos { get; private set; }
    }
}
