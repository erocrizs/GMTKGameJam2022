using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliveDetector : MonoBehaviour
{
    bool isOliveInside;
    public bool IsOliveInside => isOliveInside;
    List<Action> entranceSubscriber;
    List<Action> exitSubscriber;
    List<Action> exitSubscriberOnce;

    void Awake ()
    {
        entranceSubscriber = new List<Action>();
        exitSubscriber = new List<Action>();
        exitSubscriberOnce = new List<Action>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Olive")
        {
            Debug.Log("INSIDE");
            isOliveInside = true;

            foreach (Action subscriber in entranceSubscriber)
            {
                subscriber();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Olive")
        {
            isOliveInside = false;

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

    public void SubscribeExitOnce (Action subscriber)
    {
        exitSubscriberOnce.Add(subscriber);
    }
}
