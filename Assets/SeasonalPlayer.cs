using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeasonalPlayer : MonoBehaviour
{
    public SeasonManager seasonManager;

    public AudioSource springMusic;
    public AudioSource summerMusic;
    public AudioSource autumnMusic;
    public AudioSource winterMusic;

    private Dictionary<Season, AudioSource> seasonObjectMapping;
    private Season currentSeason;

    [SerializeField]
    float switchTime;
    const int MUSIC_STEP = 10;
    float CoroutineDelay => switchTime / MUSIC_STEP;

    // Start is called before the first frame update
    void Start()
    {
        seasonObjectMapping = new Dictionary<Season, AudioSource>();
        seasonObjectMapping.Add(Season.Spring, springMusic);
        seasonObjectMapping.Add(Season.Summer, summerMusic);
        seasonObjectMapping.Add(Season.Autumn, autumnMusic);
        seasonObjectMapping.Add(Season.Winter, winterMusic);

        if (!seasonManager)
        {
            seasonManager = FindObjectOfType<SeasonManager>();
        }

        changeSeason(seasonManager.season);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSeason != seasonManager.season)
        {
            changeSeason(seasonManager.season);
        }
    }

    private void changeSeason(Season newSeason)
    {
        StopAllCoroutines();

        currentSeason = newSeason;
        HashSet<AudioSource> toActivate = new HashSet<AudioSource>();
        HashSet<AudioSource> toDeactivate = new HashSet<AudioSource>();

        foreach (var item in seasonObjectMapping)
        {
            if (item.Key == currentSeason)
            {
                toActivate.Add(item.Value);
            }
            else
            {
                toDeactivate.Add(item.Value);
            }
        }

        toDeactivate.ExceptWith(toActivate);
        foreach (var seasonMusic in toActivate)
        {
            if (seasonMusic)
            {
                StartCoroutine(Play(seasonMusic));
            }
        }
        foreach (var seasonMusic in toDeactivate)
        {
            if (seasonMusic)
            {
                StartCoroutine(Silence(seasonMusic));
            }
        }
    }

    IEnumerator Silence(AudioSource music)
    {
        float initialVolume = music.volume;
        for (int i = 1; i <= MUSIC_STEP; i++)
        {
            float nextVolume = Mathf.Max(0f, initialVolume - (i / 10f));
            music.volume = nextVolume;

            if (nextVolume == 0)
            {
                break;
            }

            yield return new WaitForSeconds(CoroutineDelay);
        }
    }

    IEnumerator Play(AudioSource music)
    {
        float initialVolume = music.volume;
        for (int i = 1; i <= MUSIC_STEP; i++)
        {
            float nextVolume = Mathf.Min(1f, initialVolume + (i / 10f));
            music.volume = nextVolume;

            if (nextVolume == 1)
            {
                break;
            }

            yield return new WaitForSeconds(CoroutineDelay);
        }
    }
}
