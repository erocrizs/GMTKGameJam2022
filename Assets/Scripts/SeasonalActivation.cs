using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeasonalActivation : MonoBehaviour
{
    [SerializeField]
    private GameObject springObject;
    [SerializeField]
    private GameObject summerObject;
    [SerializeField]
    private GameObject autumnObject;
    [SerializeField]
    private GameObject winterObject;

    private SeasonManager seasonManager;
    private Dictionary<Season, GameObject> seasonObjectMapping;

    // Start is called before the first frame update
    void Start()
    {
        seasonObjectMapping = new Dictionary<Season, GameObject>();
        seasonObjectMapping.Add(Season.Spring, springObject);
        seasonObjectMapping.Add(Season.Summer, summerObject);
        seasonObjectMapping.Add(Season.Autumn, autumnObject);
        seasonObjectMapping.Add(Season.Winter, winterObject);

        seasonManager = FindObjectOfType<SeasonManager>();
        seasonManager.Subscribe(ChangeState);
        ChangeState();
    }

    private void ChangeState ()
    {
        HashSet<GameObject> toActivate = new HashSet<GameObject>();
        HashSet<GameObject> toDeactivate = new HashSet<GameObject>();
        
        foreach (var item in seasonObjectMapping)
        {
            if (item.Value == null)
            {
                continue;
            }

            if (item.Key == seasonManager.season)
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
