using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeasonalActivation : MonoBehaviour
{
    [SerializeField]
    GameObject springObject;
    [SerializeField]
    GameObject summerObject;
    [SerializeField]
    GameObject autumnObject;
    [SerializeField]
    GameObject winterObject;

    Dictionary<Season, GameObject> seasonObjectMapping;

    // Start is called before the first frame update
    void Start()
    {
        seasonObjectMapping = new Dictionary<Season, GameObject>
        {
            { Season.Spring, springObject },
            { Season.Summer, summerObject },
            { Season.Autumn, autumnObject },
            { Season.Winter, winterObject }
        };

        new SeasonObserver().SubscribeToSeason(ChangeState, true);
    }

    private void ChangeState (Season season)
    {
        HashSet<GameObject> toActivate = new HashSet<GameObject>();
        HashSet<GameObject> toDeactivate = new HashSet<GameObject>();
        
        foreach (var item in seasonObjectMapping)
        {
            if (item.Value == null)
            {
                continue;
            }

            if (item.Key == season)
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
