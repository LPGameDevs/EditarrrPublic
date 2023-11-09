using System;

namespace Singletons
{
    public class TwitchManager : UnitySingleton<TwitchManager>, IUnitySinglton
    {
        public static event Action<string> OnSendNotification;

        public void SendNotification(string message)
        {
            OnSendNotification?.Invoke(message);
        }

        public void Initialize()
        {
            // Nothing to do here.
        }
    }
}
