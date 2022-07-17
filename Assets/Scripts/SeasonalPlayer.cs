using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeasonalPlayer : MonoBehaviour
{
    public AudioSource springMusic;
    public AudioSource summerMusic;
    public AudioSource autumnMusic;
    public AudioSource winterMusic;

    private Dictionary<Season, AudioSource> seasonMusicMapping;
    private Season currentSeason;

    [SerializeField]
    float switchTime;

    [SerializeField]
    float maxVolume;

    const float MUSIC_STEP = 10;
    float CoroutineDelay => switchTime / MUSIC_STEP;

    // Start is called before the first frame update
    void Start()
    {
        seasonMusicMapping = new Dictionary<Season, AudioSource>();
        seasonMusicMapping.Add(Season.Spring, springMusic);
        seasonMusicMapping.Add(Season.Summer, summerMusic);
        seasonMusicMapping.Add(Season.Autumn, autumnMusic);
        seasonMusicMapping.Add(Season.Winter, winterMusic);

        SeasonManager seasonManager = FindObjectOfType<SeasonManager>();
        changeSeason(seasonManager.season);
    }

    // Update is called once per frame
    void Update()
    {
        SeasonManager seasonManager = FindObjectOfType<SeasonManager>();
        if (currentSeason != seasonManager.season)
        {
            changeSeason(seasonManager.season);
        }
    }

    private void changeSeason(Season newSeason)
    {
        StopAllCoroutines();

        currentSeason = newSeason;

        foreach (var item in seasonMusicMapping)
        {
            if (item.Key == currentSeason)
            {
                StartCoroutine(Play(item.Value));
            }
            else
            {
                StartCoroutine(Silence(item.Value));
            }
        }
    }

    IEnumerator Silence(AudioSource music)
    {
        float initialVolume = music.volume;
        for (int i = 1; i <= MUSIC_STEP; i++)
        {
            float nextVolume = Mathf.Max(0f, initialVolume - (maxVolume * (i / MUSIC_STEP)));
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
            float nextVolume = Mathf.Min(maxVolume, initialVolume + (maxVolume * (i / MUSIC_STEP)));
            music.volume = nextVolume;

            if (nextVolume == maxVolume)
            {
                break;
            }

            yield return new WaitForSeconds(CoroutineDelay);
        }
    }
}
