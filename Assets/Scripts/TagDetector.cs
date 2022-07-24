using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagDetector : MonoBehaviour
{
    [SerializeField]
    List<string> targetTags;
    public EventObservable<GameObject> OnEnter;
    public EventObservable<GameObject> OnExit;

    void Awake()
    {
        OnEnter = new EventObservable<GameObject>();
        OnExit = new EventObservable<GameObject>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetTags.Contains(collision.tag))
        {
            OnEnter.Play(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (targetTags.Contains(collision.tag))
        {
            OnExit.Play(collision.gameObject);
        }
    }
}
