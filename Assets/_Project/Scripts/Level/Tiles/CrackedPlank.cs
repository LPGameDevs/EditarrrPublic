using Player;
using UnityEngine;

namespace Editarrr.Level.Tiles
{
    public class CrackedPlank : MonoBehaviour
    {
        [field: SerializeField] private SpriteRenderer SpriteRenderer { get; set; }
        [field: SerializeField] private Sprite[] Stages { get; set; }

        [field: SerializeField] private float Duration { get; set; }

        bool IsCrumbling { get; set; }

        float CrumbleTime { get; set; }
        int SpriteIndex { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Only triggers for player, We can add other conditions though (i.e. Enemies etc.)
            if (!collision.transform.TryGetComponent<PlayerController>(out PlayerController playerController))
                return;

            this.IsCrumbling = true;
        }

        private void Start()
        {
            this.UpdateSprite();
        }

        private void Update()
        {
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
                    GameObject.Destroy(this.gameObject);
                    return;
                }

                this.UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            this.SpriteRenderer.sprite = this.Stages[this.SpriteIndex];
        }
    }
}
