using System;
using System.Collections.Generic;

namespace Systems
{
    public enum GameEventType { Pause, Unpause }

    public struct GameEvent
    {
        public GameEvent(GameEventType type)
        {
            Type = type;
        }

        static GameEvent _instance;
        public GameEventType Type { get; set; }

        public static void Trigger(GameEventType type)
        {
            _instance.Type = type;
            EventManager.TriggerEvent(_instance);
        }
    }

    public static class EventManager
    {
        private static Dictionary<Type, List<IEventListenerBase>> _subscribersList;

        static EventManager()
        {
            _subscribersList = new Dictionary<Type, List<IEventListenerBase>>();
        }

        public static void AddListener<TEvent>(IEventListener<TEvent> listener) where TEvent : struct
        {
            Type eventType = typeof(TEvent);

            if (!_subscribersList.ContainsKey(eventType))
                _subscribersList[eventType] = new List<IEventListenerBase>();

            if (!SubscriptionExists(eventType, listener))
                _subscribersList[eventType].Add(listener);
        }

        public static void RemoveListener<TEvent>(IEventListener<TEvent> listener) where TEvent : struct
        {
            Type eventType = typeof(TEvent);

            if (!_subscribersList.ContainsKey(eventType))
            {
                return;
            }

            List<IEventListenerBase> subscriberList = _subscribersList[eventType];

            for (int i = 0; i < subscriberList.Count; i++)
            {
                if (subscriberList[i] == listener)
                {
                    subscriberList.Remove(subscriberList[i]);

                    if (subscriberList.Count == 0)
                    {
                        _subscribersList.Remove(eventType);
                    }

                    return;
                }
            }
        }

        public static void TriggerEvent<Event>(Event newEvent) where Event : struct
        {
            List<IEventListenerBase> list;
            if (!_subscribersList.TryGetValue(typeof(Event), out list))
            {
                return;
            }


            foreach (var listener in list)
            {
                (listener as IEventListener<Event>)?.OnEvent(newEvent);
            }
        }

        private static bool SubscriptionExists(Type type, IEventListenerBase receiver)
        {
            List<IEventListenerBase> receivers;

            if (!_subscribersList.TryGetValue(type, out receivers)) return false;

            bool exists = false;

            for (int i = 0; i < receivers.Count; i++)
            {
                if (receivers[i] == receiver)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }
    }


    public static class EventRegister
    {
        public delegate void Delegate<T>(T eventType);

        public static void EventStartListening<TEventType>(this IEventListener<TEventType> caller)
            where TEventType : struct
        {
            EventManager.AddListener<TEventType>(caller);
        }

        public static void EventStopListening<TEventType>(this IEventListener<TEventType> caller)
            where TEventType : struct
        {
            EventManager.RemoveListener<TEventType>(caller);
        }
    }

    public interface IEventListenerBase
    {
    };


    public interface IEventListener<T> : IEventListenerBase
    {
        void OnEvent(T eventType);
    }
}
