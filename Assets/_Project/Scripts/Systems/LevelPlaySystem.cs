using Editarrr.Level;
using Editarrr.Misc;
using Editarrr.Systems;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Systems
{
    public class LevelPlaySystem : SystemComponent<LevelPlayManager>
    {
        private const string Documentation = "This System is for setting up a playable level.\r\n" +
                                             "It will add the relevant manager and connect it with scene dependencies.\r\n";

        [field: SerializeField, Info(Documentation)] private Tilemap Walls { get; set; }
        [field: SerializeField] private Tilemap Damage { get; set; }


        protected override void PreAwake()
        {
            this.Manager.SetTilemapWalls(this.Walls);
            this.Manager.SetTilemapDamage(this.Damage);
        }
    }
}
