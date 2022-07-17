using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieDetector : MonoBehaviour
{
    bool isDieInside;
    public bool IsDieInside => isDieInside;
    List<Action> entranceSubscriber;
    List<Action> entranceSubscriberOnce;
    List<Action> exitSubscriber;
    List<Action> exitSubscriberOnce;

    void Awake()
    {
        entranceSubscriber = new List<Action>();
        entranceSubscriberOnce = new List<Action>();
        exitSubscriber = new List<Action>();
        exitSubscriberOnce = new List<Action>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Die"))
        {
            isDieInside = true;

            foreach (Action subscriber in entranceSubscriberOnce)
            {
                subscriber();
            }

            entranceSubscriberOnce.Clear();

            foreach (Action subscriber in entranceSubscriber)
            {
                subscriber();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Die"))
        {
            isDieInside = false;

            foreach (Action subscriber in exitSubscriberOnce)
            {
                subscriber();
            }

            exitSubscriberOnce.Clear();

            foreach (Action subscriber in exitSubscriber)
            {
                subscriber();
            }
        }
    }

    public void SubscribeEntrance(Action subscriber)
    {
        entranceSubscriber.Add(subscriber);
    }

    public void SubscribeExit(Action subscriber)
    {
        exitSubscriber.Add(subscriber);
    }

    public void SubscribeEntranceOnce(Action subscriber)
    {
        entranceSubscriberOnce.Add(subscriber);
    }

    public void SubscribeExitOnce(Action subscriber)
    {
        exitSubscriberOnce.Add(subscriber);
    }
}
