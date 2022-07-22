using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeasonalActivationChild : MonoBehaviour
{
    [SerializeField]
    Season[] activeSeasons;

    // Start is called before the first frame update
    void Start()
    {
        SeasonManager.SubscribeToSeason(ChangeState, true);
    }

    public void ChangeState(Season season)
    {
        bool shouldBeActive = activeSeasons.Contains(season);
        if (shouldBeActive != isActiveAndEnabled)
        {
            gameObject.SetActive(!isActiveAndEnabled);
        }
    }
}
