using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonObserver
{
    private readonly SeasonManager seasonManager;

    public SeasonObserver()
    {
        seasonManager = UnityEngine.Object.FindObjectOfType<SeasonManager>();
    }

    public void SubscribeToSeason (Action<Season> listener, bool runImmediately)
    {
        void seasonListener() => listener(seasonManager.season);
        seasonManager.Subscribe(seasonListener);

        if (runImmediately)
        {
            seasonListener();
        }
    }
}
