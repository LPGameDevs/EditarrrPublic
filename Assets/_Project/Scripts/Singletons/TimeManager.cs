using Systems;
using UnityEngine;

namespace Singletons
{
    public class TimeManager : UnitySingleton<TimeManager>, IEventListener<GameEvent>
    {
        public void OnEvent(GameEvent eventType)
        {
            if (eventType.Type == GameEventType.Pause)
            {
                Time.timeScale = 0;
            }
            else if (eventType.Type == GameEventType.Unpause)
            {
                Time.timeScale = 1;
            }
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
