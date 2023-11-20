using UnityEngine;

namespace Editarrr.Level
{
    [System.Serializable]
    public class ChannelInfo
    {
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}
