using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliveDetector : MonoBehaviour
{
    bool isOliveInside;
    public bool IsOliveInside => isOliveInside;
    List<Action> exitSubscriber;

    void Awake ()
    {
        exitSubscriber = new List<Action>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Olive")
        {
            Debug.Log("Olive In");
            isOliveInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Olive")
        {
            Debug.Log("Olive Out");
            isOliveInside = false;

            foreach (Action subscriber in exitSubscriber)
            {
                subscriber();
            }

            exitSubscriber.Clear();
        }
    }

    public void SubscribeExitOnce (Action subscriber)
    {
        exitSubscriber.Add(subscriber);
    }
}
