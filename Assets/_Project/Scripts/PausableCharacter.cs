using System;
using System.Linq;
using Systems;
using UnityEngine;

public class PausableCharacter : MonoBehaviour, IEventListener<GameEvent>
{
    // This is horrible, but for some reason colliders are not fully established when update starts...
    public bool _active = false;
    public bool _inputLocked = false;


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

    public virtual void Activate()
    {
        _active = true;
    }

    public virtual void Deactivate()
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


    private void OnEnable()
    {
        this.EventStartListening<GameEvent>();
    }

    private void OnDisable()
    {
        this.EventStopListening<GameEvent>();
    }
}
