using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeasonalActivation : MonoBehaviour
{
    public SeasonManager seasonManager;

    public GameObject springObject;
    public GameObject summerObject;
    public GameObject autumnObject;
    public GameObject winterObject;

    private Dictionary<Season, GameObject> seasonObjectMapping;
    private Season currentSeason;

    // Start is called before the first frame update
    void Start()
    {
        seasonObjectMapping = new Dictionary<Season, GameObject>();
        seasonObjectMapping.Add(Season.Spring, springObject);
        seasonObjectMapping.Add(Season.Summer, summerObject);
        seasonObjectMapping.Add(Season.Autumn, autumnObject);
        seasonObjectMapping.Add(Season.Winter, winterObject);

        currentSeason = seasonManager.season;
        changeSeason(seasonManager.season);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSeason != seasonManager.season)
        {
            changeSeason(seasonManager.season);
        }
    }

    private void changeSeason(Season newSeason)
    {
        currentSeason = newSeason;
        HashSet<GameObject> toActivate = new HashSet<GameObject>();
        HashSet<GameObject> toDeactivate = new HashSet<GameObject>();
        
        foreach (var item in seasonObjectMapping)
        {
            if (item.Value == null)
            {
                continue;
            }

            if (item.Key == currentSeason)
            {
                toActivate.Add(item.Value);
            } else
            {
                toDeactivate.Add(item.Value);
            }
        }

        toDeactivate.ExceptWith(toActivate);
        foreach (var seasonObject in toActivate)
        {
            seasonObject.SetActive(true);
        }
        foreach (var seasonObject in toDeactivate)
        {
            seasonObject.SetActive(false);
        }
    }
}
