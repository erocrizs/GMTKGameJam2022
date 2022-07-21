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
    public Season season = Season.Spring;
    private Season lastSeason;

    private EventObservable onSeasonChange;

    private void Awake()
    {
        onSeasonChange = new EventObservable();
    }

    private void Update()
    {
        if (lastSeason != season) {
            lastSeason = season;
            onSeasonChange.Play();
        }
    }

    public void Subscribe(Action listener) => onSeasonChange.Subscribe(listener);
    public void Unsubscribe(Action listener) => onSeasonChange.Unsubscribe(listener);
}
