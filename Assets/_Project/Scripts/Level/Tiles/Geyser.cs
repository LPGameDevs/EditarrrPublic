using System;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    public class Geyser : MonoBehaviour
    {
        [field: SerializeField] private float Force { get; set; } = 100f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent<PlayerForceReceiver>(out PlayerForceReceiver forceReceiver))
                return;

            forceReceiver.ReceiveImpulse(this.Force, this.transform.up);
        }
    }
}
