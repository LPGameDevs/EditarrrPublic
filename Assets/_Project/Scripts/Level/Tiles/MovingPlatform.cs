using Editarrr.LevelEditor;
using Player;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    public class MovingPlatform : MonoBehaviour, IConfigurable, IExternalMovementSource
    {
        [field: SerializeField] private float Speed { get; set; } = 100f;
        [field: SerializeField] private float Distance { get; set; } = 6f;

        public Vector3 Delta { get; private set; }

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
            if (!collision.transform.TryGetComponent(out IMoveOnPlatform moveOnPlatform))
                return;

            moveOnPlatform.EnterPlatform(this);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.transform.TryGetComponent(out IMoveOnPlatform moveOnPlatform))
                return;

            moveOnPlatform.ExitPlatform(this);
        }

        
        private void FixedUpdate()
        {
            Vector3 target = this.Target;

            if (this.Direction)
            {
                target = this.Origin;
            }

            var transformPosition = this.transform.position;

            this.transform.position = Vector3.MoveTowards(this.transform.position, target, this.Speed * Time.deltaTime);

            this.Delta = this.transform.position - transformPosition;

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
            this.Direction = config.MoveRight;
        }
    }

    public interface IMoveOnPlatform
    {
        void EnterPlatform(IExternalMovementSource externalMovementSource);
        void ExitPlatform(IExternalMovementSource externalMovementSource);
    }

    public interface IExternalMovementSource
    {
        Vector3 Delta { get; }
    }
}
