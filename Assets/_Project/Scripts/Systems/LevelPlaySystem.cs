using Editarrr.Level;
using Editarrr.Misc;
using Editarrr.Systems;
using Gameplay.GUI;
using LevelEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Systems
{
    public class LevelPlaySystem : SystemComponent<LevelPlayManager>
    {
        private const string Documentation = "This System is for setting up a playable level.\r\n" +
                                             "It will add the relevant manager and connect it with scene dependencies.\r\n";

        [field: SerializeField, Info(Documentation)] private Tilemap Foreground { get; set; }
        [field: SerializeField] private Tilemap Background { get; set; }
        [field: SerializeField] private GameplayGuiManager GameplayGuiManager { get; set; }
        [field: SerializeField] private GhostRecorder Recorder { get; set; }


        protected override void PreAwake()
        {
            this.Manager.SetForeground(this.Foreground);
            this.Manager.SetBackground(this.Background);

            this.Manager.SetGuiManager(this.GameplayGuiManager);
            this.Manager.SetRecorder(this.Recorder);
        }
    }
}
