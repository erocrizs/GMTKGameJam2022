using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObservable
{
    private List<Action> listeners;
    
    public EventObservable ()
    {
        listeners = new List<Action>();
    }

    public void Subscribe (Action listener)
    {
        listeners.Add(listener);
    }

    public void Unsubscribe (Action listener)
    {
        listeners.Remove(listener);
    }

    public void SubscribeOnce (Action listener)
    {
        void oneTimeListener()
        {
            listener();
            listeners.Remove(oneTimeListener);
        }

        listeners.Add(oneTimeListener);
    }

    public void Play ()
    {
        foreach (var listener in listeners) {
            listener();
        }
    }
}
