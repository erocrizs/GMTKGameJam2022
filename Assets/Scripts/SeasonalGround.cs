using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalGround : MonoBehaviour
{
    [SerializeField]
    int index;

    void Start()
    {
        foreach(SeasonalGroundTile child in GetComponentsInChildren<SeasonalGroundTile>(true))
        {
            child.SetSprite(index);
        }

       Destroy(this);
    }
}
