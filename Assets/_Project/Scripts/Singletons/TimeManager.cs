using Editarrr.Input;
using Systems;
using UnityEngine;

namespace Singletons
{
    public class TimeManager : UnityPersistentSingleton<TimeManager>, IEventListener<GameEvent>
    {
        public void OnEvent(GameEvent eventType)
        {
            if (eventType.Type == GameEventType.Pause)
                StopTime();
            else if (eventType.Type == GameEventType.Unpause)
                StartTime();
        }

        public void StopTime()
        {
            Time.timeScale = 0;
        }

        public void StartTime()
        {
            Time.timeScale = 1;
        }

        private void OnEnable()
        {
            this.EventStartListening<GameEvent>();
        }

        private void OnDisable()
        {
            this.EventStopListening<GameEvent>();
        }
    }
}
