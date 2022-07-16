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
}
