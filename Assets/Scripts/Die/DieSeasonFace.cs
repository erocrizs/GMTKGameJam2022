using System.Collections.Generic;
using UnityEngine;

public class DieSeasonFace : MonoBehaviour
{
    public Season dieSeason = Season.Spring;

    public void Update()
    {
        float zRotation = gameObject.transform.rotation.eulerAngles.z;
        float tolerance = 0.001f;

        if (Mathf.Abs(zRotation - 0) < tolerance)
        {
            dieSeason = Season.Spring;
        }
        else if (Mathf.Abs(zRotation - 90) < tolerance)
        {
            dieSeason = Season.Summer;
        }
        else if (Mathf.Abs(zRotation - 180) < tolerance)
        {
            dieSeason = Season.Autumn;
        }
        else if (Mathf.Abs(zRotation - 270) < tolerance)
        {
            dieSeason = Season.Winter;
        }
    }
}
