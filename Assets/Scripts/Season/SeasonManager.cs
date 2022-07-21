using System;
using UnityEngine;

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter,
}

public class SeasonManager : MonoBehaviour
{
    private static SeasonManager main;
    private static SeasonManager Main
    {
        get {
            if (main == null || !main.isActiveAndEnabled)
            {
                main = FindObjectOfType<SeasonManager>();
            }
            return main;
        }
    }

    public Season season = Season.Spring;
    private Season lastSeason;

    private EventObservable onSeasonChange;

    private void Awake()
    {
        onSeasonChange = new EventObservable();
    }

    private void Update()
    {
        if (lastSeason != season)
        {
            lastSeason = season;
            onSeasonChange.Play();
        }
    }

    public void Subscribe(Action listener) => onSeasonChange.Subscribe(listener);
    public void Unsubscribe(Action listener) => onSeasonChange.Unsubscribe(listener);

    public static void SubscribeToSeason(Action<Season> listener, bool runImmediately)
    {
        void seasonListener() => listener(Main.season);
        Main.Subscribe(seasonListener);

        if (runImmediately)
        {
            seasonListener();
        }
    }
}
