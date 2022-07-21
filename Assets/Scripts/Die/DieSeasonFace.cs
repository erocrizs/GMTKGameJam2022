using System.Collections.Generic;
using UnityEngine;

public class DieSeasonFace : MonoBehaviour
{
    public Season dieSeason = Season.Spring;

    [SerializeField]
    Season topSeason;
    [SerializeField]
    Season rightSeason;
    [SerializeField]
    Season bottomSeason;
    [SerializeField]
    Season leftSeason;

    public void Update()
    {
        float zRotation = gameObject.transform.rotation.eulerAngles.z;
        float tolerance = 0.001f;

        if (Mathf.Abs(zRotation - 0) < tolerance)
        {
            dieSeason = topSeason;
        }
        else if (Mathf.Abs(zRotation - 90) < tolerance)
        {
            dieSeason = rightSeason;
        }
        else if (Mathf.Abs(zRotation - 180) < tolerance)
        {
            dieSeason = bottomSeason;
        }
        else if (Mathf.Abs(zRotation - 270) < tolerance)
        {
            dieSeason = leftSeason;
        }
    }
}
