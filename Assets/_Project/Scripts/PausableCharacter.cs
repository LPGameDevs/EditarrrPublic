using System;
using System.Linq;
using Systems;
using UnityEngine;

public class PausableCharacter : MonoBehaviour, IEventListener<GameEvent>
{
    // This is horrible, but for some reason colliders are not fully established when update starts...
    internal bool _active = false;
    internal bool _inputLocked = false;


    public void OnEvent(GameEvent gameEvent)
    {
        // We only care about pause events.
        if (!new[] { GameEventType.Pause, GameEventType.Unpause }.Contains(gameEvent.Type))
        {
            return;
        }

        if (gameEvent.Type == GameEventType.Pause)
        {
            UpdateActiveState(false);
        }
        else
        {
            UpdateActiveState(true);
        }
    }

    internal virtual void Activate()
    {
        _active = true;
    }

    internal virtual void Deactivate()
    {
        _active = false;
    }

    private void UpdateActiveState(bool activate)
    {
        if (activate)
            Activate();
        else
            Deactivate();
    }


    internal virtual void OnEnable()
    {
        this.EventStartListening<GameEvent>();
    }

    internal virtual void OnDisable()
    {
        this.EventStopListening<GameEvent>();
    }
}
