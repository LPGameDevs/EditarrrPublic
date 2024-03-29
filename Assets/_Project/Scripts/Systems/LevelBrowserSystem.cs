using Editarrr.Misc;
using Editarrr.Systems;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    public class LevelBrowserSystem : SystemComponent<LevelBrowserManager>
    {
        private const string Documentation = "This System is for managing level selection.\r\n" +
                                             "It will add the relevant manager and connect it with scene dependencies.\r\n";

        [field: SerializeField, Info(Documentation)] public LevelBrowserLoader LevelLoader { get; private set; }

        protected override void PreAwake()
        {
            this.Manager.SetLevelLoader(this.LevelLoader);
        }
    }
}
