using Editarrr.LevelEditor;
using Player;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    public class MovingPlatform : MonoBehaviour, IConfigurable
    {
        [field: SerializeField] private float Speed { get; set; } = 100f;
        [field: SerializeField] private float Distance { get; set; } = 6f;

        PlayerController Player { get; set; }
        Transform EnterTransform { get; set; }

        Vector3 Origin { get; set; }
        Vector3 Target { get; set; }
        bool Direction { get; set; }

        private void Start()
        {
            this.Origin = this.transform.position;
            this.Target = this.transform.position + (this.Direction ? this.transform.right : -this.transform.right) * this.Distance;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.transform.TryGetComponent<PlayerController>(out PlayerController playerController))
                return;

            this.Player = playerController;
            this.EnterTransform = this.Player.transform.parent;
            this.Player.transform.SetParent(this.transform, true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.transform.TryGetComponent<PlayerController>(out PlayerController playerController))
                return;

            this.Player.transform.SetParent(this.EnterTransform, true);
            this.Player = null;
            this.EnterTransform = null;
        }

        private void LateUpdate()
        {
            Vector3 target = this.Target;

            if (this.Direction)
            {
                target = this.Origin;
            }

            this.transform.position = Vector3.MoveTowards(this.transform.position, target, this.Speed * Time.deltaTime);

            if ((target - this.transform.position).sqrMagnitude <= float.Epsilon)
                this.Direction = !this.Direction;
        }

        public void Configure(TileConfig config)
        {
            this.Configure(config as MovingPlatformConfig);
        }

        private void Configure(MovingPlatformConfig config)
        {
            if (config == null)
                return;

            this.Distance = config.Distance;
            this.Direction = config.Direction;
        }
    }
}
