using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonButton : MonoBehaviour
{
    public DieSeasonFace dieSeasonFace;
    public SeasonManager seasonManager;

    private Season previousSeason;

    // Start is called before the first frame update
    void Start()
    {
        previousSeason = seasonManager.season;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == dieSeasonFace.gameObject)
        {
            previousSeason = dieSeasonFace.dieSeason;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == dieSeasonFace.gameObject && previousSeason != dieSeasonFace.dieSeason)
        {
            seasonManager.season = dieSeasonFace.dieSeason;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == dieSeasonFace.gameObject && previousSeason != dieSeasonFace.dieSeason)
        {
            seasonManager.season = dieSeasonFace.dieSeason;
        }
    }
}