using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonRune : MonoBehaviour
{
    public Season season;
    public SeasonManager seasonManager;

    public GameObject springRune;
    public GameObject summerRune;
    public GameObject autumnRune;
    public GameObject winterRune;

    private Dictionary<Season, GameObject> seasonRuneMapping;

    // Start is called before the first frame update
    void Start()
    {
        seasonRuneMapping = new Dictionary<Season, GameObject>();
        seasonRuneMapping.Add(Season.Spring, springRune);
        seasonRuneMapping.Add(Season.Summer, summerRune);
        seasonRuneMapping.Add(Season.Autumn, autumnRune);
        seasonRuneMapping.Add(Season.Winter, winterRune);

        SetSeasonRune(season);
    }

    // Update is called once per frame
    void Update()
    {
        if (!seasonRuneMapping[season].activeSelf)
        {
            SetSeasonRune(season);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Olive" || collision.name == "Die")
        {
            seasonManager.season = season;
        }
    }

    void SetSeasonRune (Season season)
    {
        foreach (var item in seasonRuneMapping)
        {
            item.Value.SetActive(item.Key == season);
        }
    }
}
