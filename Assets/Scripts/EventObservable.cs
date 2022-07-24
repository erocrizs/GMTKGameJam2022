using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObservable
{
    private List<Action> listeners;
    private List<Action> subscribedOnce;

    public EventObservable ()
    {
        listeners = new List<Action>();
        subscribedOnce = new List<Action>();
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
        subscribedOnce.Add(listener);
    }

    public void Play ()
    {
        foreach (var listener in listeners) {
            listener();
        }
        foreach (var listener in subscribedOnce)
        {
            listener();
        }
        subscribedOnce.Clear();
    }
}

public class EventObservable<T> where T : class
{
    private List<Action<T>> listeners;
    private List<Action<T>> subscribedOnce;

    public EventObservable()
    {
        listeners = new List<Action<T>>();
        subscribedOnce = new List<Action<T>>();
    }

    public void Subscribe(Action<T> listener)
    {
        listeners.Add(listener);
    }

    public void Unsubscribe(Action<T> listener)
    {
        listeners.Remove(listener);
    }

    public void SubscribeOnce(Action<T> listener)
    {
        subscribedOnce.Add(listener);
    }

    public void Play(T target)
    {
        foreach (var listener in listeners)
        {
            listener(target);
        }
        foreach (var listener in subscribedOnce)
        {
            listener(target);
        }
        subscribedOnce.Clear();
    }
}