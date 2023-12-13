using Editarrr.LevelEditor;
using Player;
using System;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    public class CrackedPlank : MonoBehaviour, IConfigurable
    {
        [field: SerializeField] private SpriteRenderer SpriteRenderer { get; set; }
        [field: SerializeField] private Sprite[] Stages { get; set; }

        [field: SerializeField] private Transform CollisionPivot { get; set; }
        [field: SerializeField] private Collider2D Collider { get; set; }


        [field: SerializeField] private float Duration { get; set; }
        [field: SerializeField] private float RespawnDuration { get; set; } = 2f;

        [field: SerializeField] private ContactFilter2D RespawnFilter { get; set; }

        bool IsRespawning { get; set; }
        float RespawnTime { get; set; }

        bool IsCrumbling { get; set; }
        float CrumbleTime { get; set; }
        int SpriteIndex { get; set; }
        bool CanRespawn { get; set; }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Only triggers for player, We can add other conditions though (i.e. Enemies etc.)
            if (!collision.transform.TryGetComponent<PlayerController>(out PlayerController playerController))
                return;

            this.IsCrumbling = true;
        }

        private void Start()
        {
            this.SetState(true);
            this.UpdateSprite();
        }

        private void Update()
        {
            if (this.Respawn())
                return;
            
            if (!this.IsCrumbling)
                return;

            this.CrumbleTime += Time.deltaTime;
            float stageDuration = this.Duration / this.Stages.Length;
            float threshold = (this.SpriteIndex + 1) * stageDuration;

            if (this.CrumbleTime >= threshold)
            {
                this.SpriteIndex++;

                if (this.SpriteIndex >= this.Stages.Length)
                {
                    if (this.CanRespawn)
                    {
                        this.SetState(false);
                        this.IsCrumbling = false;
                        this.IsRespawning = true;
                        this.RespawnTime = 0;
                        this.CrumbleTime = 0;
                        this.SpriteIndex = 0;
                        this.UpdateSprite();
                    }
                    else
                    {
                        GameObject.Destroy(this.gameObject);
                    }
                    return;
                }

                this.UpdateSprite();
            }
        }

        /// <returns>True, if Respawn is active.</returns>
        private bool Respawn()
        {
            if (!this.IsRespawning)
                return false;

            this.RespawnTime = (this.RespawnTime + Time.deltaTime).Clamp(0, this.RespawnDuration);

            if (this.RespawnTime >= this.RespawnDuration)
            {
                // We need to activate the Collider in order to cast it?!?!..
                this.SetState(true);

                Collider2D[] result = new Collider2D[1];
                if (this.Collider.OverlapCollider(this.RespawnFilter, result) > 0)
                {
                    this.SetState(false);
                    return true;
                }

                this.IsRespawning = false;
                
            }

            this.UpdateSprite();

            return true;
        }

        private void SetState(bool state)
        {
            this.CollisionPivot.gameObject.SetActive(state);
        }

        private void UpdateSprite()
        {
            this.SpriteRenderer.sprite = this.Stages[this.SpriteIndex];
            this.SpriteRenderer.color = Color.white * (this.IsRespawning ? .5f : 1);
            if (this.IsRespawning)
            {
                this.SpriteRenderer.transform.localScale = Vector3.one *
                    (.2f + (this.RespawnTime / this.RespawnDuration) * .8f);
            }
            else
            {
                this.SpriteRenderer.transform.localScale = Vector3.one;
            }
        }

        public void Configure(TileConfig config)
        {
            this.Configure(config as CrackedPlankConfig);
        }

        private void Configure(CrackedPlankConfig config)
        {
            if (config == null)
                return;

            this.CanRespawn = config.CanRespawn;
        }
    }
}
