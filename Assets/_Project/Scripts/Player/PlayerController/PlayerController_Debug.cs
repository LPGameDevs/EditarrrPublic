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

            Debug.Log(this.transform.position);
        }

        private void UpdateDebug()
        {
            //this.DeltaTime += Time.deltaTime;

            //if (this.DeltaTime > 1f / this.FPS)
            //{
            //    this.UpdateController();

            //    if (this.BreakOnFrame)
            //        Debug.Break();

            //    this.DeltaTime = 0;
            //}
        }

        //IEnumerator DebugUpdate()
        //{
        //    while (true)
        //    {
        //        yield return new WaitForEndOfFrame();
        //        this.DeltaTime += 1.0f / this.FPS;
        //        // this.DeltaTime += Time.deltaTime;

        //        if (this.DeltaTime > 1 / this.FPS)
        //        {
        //            this.UpdateController();
        //            this.DeltaTime = 0;
        //        }

        //        //var t = Time.realtimeSinceStartup;
        //        //var sleepTime = currentFrameTime - t - 0.01f;
        //        //if (sleepTime > 0)
        //        //    Thread.Sleep((int)(sleepTime * 1000));
        //        //while (t < currentFrameTime)
        //        //    t = Time.realtimeSinceStartup;
        //    }
        //}
    }
}
