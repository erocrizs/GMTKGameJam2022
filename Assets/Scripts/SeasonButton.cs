using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonButton : MonoBehaviour
{
    public DieSeasonFace dieSeasonFace;

    private Season currentSeason;

    // Start is called before the first frame update
    void Start()
    {
        SeasonManager.SubscribeToSeason(UpdateSeason, true);

        if (!dieSeasonFace)
        {
            dieSeasonFace = FindObjectOfType<DieSeasonFace>();
        }
    }

    void UpdateSeason (Season season)
    {
        currentSeason = season;
    }

    void ChangeSeasonBasedOnDie ()
    {
        SeasonManager.Main.season = FindObjectOfType<DieSeasonFace>().dieSeason;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Die"))
        {
            FindObjectOfType<DieMovement>().OnStop.Subscribe(ChangeSeasonBasedOnDie);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Die"))
        {
            FindObjectOfType<DieMovement>().OnStop.Unsubscribe(ChangeSeasonBasedOnDie);
        }
    }
}
