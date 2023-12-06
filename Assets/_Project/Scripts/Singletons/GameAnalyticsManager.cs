using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;
using UnityEngine;

namespace Singletons
{
    /**
     * This class is used connect to the game analytics service.
     */
    public class GameAnalyticsManager : UnitySingleton<GameAnalyticsManager>, IUnitySinglton
    {
        private const string GameKey = "6b59e8499a4eed03b050419b044811de";
        private const string GameSecret = "d73750b0cb0ebf8824c81782b9ecda549189e7c3";


        public void Initialize()
        {
            Configure();
            GameAnalytics.Initialize();

            UnityEngine.Debug.Log("analytics are live!");
            UnityEngine.Debug.Log(GameAnalytics.GetUserId());
        }

        private void Configure()
        {
            // GameAnalytics.SetEnabledInfoLog(true);
            // GameAnalytics.SetEnabledVerboseLog(true);

            // string build = Application.version;
            // GameAnalytics.Set(build);

            string userId = PreferencesManager.Instance.GetUserId();
            GameAnalytics.SetCustomId(userId);
        }

        public void PlayerProgression()
        {
            // GameAnalytics.NewProgressionEvent (GA_Progression.GAProgressionStatus progressionStatus, string progression01, string progression02);
        }

        public void CustomEvent()
        {
            // GameAnalytics.NewDesignEvent (string eventName, float eventValue);
        }

        public void LogError(string message)
        {
            GameAnalytics.NewErrorEvent (GAErrorSeverity.Critical, message);
        }

        public void CustomDimension()
        {
            // GameAnalytics.SetCustomDimension01(string customDimension);
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        public void Debug()
        {
            UnityEngine.Debug.Log("everything is awesome!");
        }
    }
}
