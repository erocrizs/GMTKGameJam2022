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
        if (!seasonManager)
        {
            seasonManager = FindObjectOfType<SeasonManager>();
        }

        if (!dieSeasonFace)
        {
            dieSeasonFace = FindObjectOfType<DieSeasonFace>();
        }

        previousSeason = seasonManager.season;
    }

    private void Update()
    {
        if (Physics2D.Raycast(transform.position, Vector2.up, 0.01f, LayerMask.GetMask("Die")))
        {
            if (previousSeason != dieSeasonFace.dieSeason)
            {
                seasonManager.season = dieSeasonFace.dieSeason;
            }
            previousSeason = dieSeasonFace.dieSeason;
        }
    }
}
