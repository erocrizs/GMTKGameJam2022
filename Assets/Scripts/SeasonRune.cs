using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonRune : MonoBehaviour
{
    public Season season;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Olive" || collision.name == "Die")
        {
            SeasonManager.Main.season = season;
        }
    }
}
