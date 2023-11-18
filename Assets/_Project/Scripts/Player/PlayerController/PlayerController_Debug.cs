using Editarrr.Input;
using System.Collections;
using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        [field: SerializeField, Header("DEBUG")] private int FPS { get; set; }
        // [field: SerializeField] private float DeltaTime { get; set; }

        [field: SerializeField] private bool IsDebugMode { get; set; }
        [field: SerializeField] private bool BreakOnFrame { get; set; }
        [field: SerializeField] private bool BreakOnStartFalling { get; set; }

        [field: SerializeField] private bool SetFPSOnStart { get; set; }

        void StartDebug()
        {
            if (this.SetFPSOnStart)
                Application.targetFrameRate = this.FPS;

        }

        private void OnEnableDebug()
        {
            OnPlayerStartedFalling += this.Debug_OnPlayerStartedFalling;
        }

        private void OnDisableDebug()
        {
            OnPlayerStartedFalling -= this.Debug_OnPlayerStartedFalling;
        }

        private void Debug_OnPlayerStartedFalling()
        {
            if (this.IsDebugMode && this.BreakOnStartFalling)
                Debug.Break();

            // Debug.Log(this.transform.position);
        }
    }
}
