using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalActivation : MonoBehaviour
{
    [SerializeField]
    GameObject defaultActive;
    SeasonalActivationChild[] children;
    void Start()
    {
        children = GetComponentsInChildren<SeasonalActivationChild>(true);
        new SeasonObserver().SubscribeToSeason(ActivateChildren, true);
    }

    void ActivateChildren (Season season)
    {
        bool hasActiveChildren = false;

        foreach (var child in children)
        {
            child.ChangeState(season);

            if (child.isActiveAndEnabled)
            {
                hasActiveChildren = true;
            }
        }

        if (defaultActive && hasActiveChildren == defaultActive.activeInHierarchy)
        {
            defaultActive.SetActive(!hasActiveChildren);
        }
    }
}
