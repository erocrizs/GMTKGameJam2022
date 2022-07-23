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
        if (45 < zRotation && zRotation <= 135)
        {
            dieSeason = rightSeason;
        }
        else if (135 < zRotation && zRotation <= 225)
        {
            dieSeason = bottomSeason;
        }
        else if (225 < zRotation && zRotation <= 315)
        {
            dieSeason = leftSeason;
        }
        else
        {
            dieSeason = topSeason;
        }
    }
}
