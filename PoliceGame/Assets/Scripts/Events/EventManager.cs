using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager: SingletonMonoBehaviour<EventManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    private Dictionary<EventId, List<Action>> subscribers = new Dictionary<EventId, List<Action>>();
    public void SendEvent(EventId eventId)
    {
        List<Action> subs;
        if (subscribers.TryGetValue(eventId, out subs))
        {
            foreach (Action action in subs)
            {
                action.Invoke();
            }
        }

    }

    public void Sub(EventId eventId, Action action)
    {
        List<Action> subs;
        if (!subscribers.TryGetValue(eventId, out subs))
        {
            subs = new List<Action>();
            subscribers[eventId] = subs;
        }

        subs.Add(action);

    }

    public void Unsub(EventId eventId, Action action)
    {
        //Debug.Log($"Unsub {eventId}");
        List<Action> subs = subscribers[eventId];
        subs.Remove(action);
    }

}
