using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeasonalPlayer : MonoBehaviour
{
    [SerializeField]
    AudioSource springMusic;
    [SerializeField]
    AudioSource summerMusic;
    [SerializeField]
    AudioSource autumnMusic;
    [SerializeField]
    AudioSource winterMusic;
    [SerializeField]
    float switchTime;
    [SerializeField]
    float maxVolume;
    Dictionary<Season, AudioSource> seasonMusicMapping;

    const float MUSIC_STEP = 10;
    float CoroutineDelay => switchTime / MUSIC_STEP;

    // Start is called before the first frame update
    void Start()
    {
        seasonMusicMapping = new Dictionary<Season, AudioSource>
        {
            { Season.Spring, springMusic },
            { Season.Summer, summerMusic },
            { Season.Autumn, autumnMusic },
            { Season.Winter, winterMusic }
        };

        new SeasonObserver().SubscribeToSeason(ChangeBGMusic, true);
    }

    private void ChangeBGMusic(Season season)
    {
        StopAllCoroutines();

        foreach (var item in seasonMusicMapping)
        {
            if (item.Key == season)
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

            yield return new WaitForSecondsRealtime(CoroutineDelay);
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

            yield return new WaitForSecondsRealtime(CoroutineDelay);
        }
    }
}
