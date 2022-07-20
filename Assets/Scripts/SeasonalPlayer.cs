using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeasonalPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource springMusic;
    [SerializeField]
    private AudioSource summerMusic;
    [SerializeField]
    private AudioSource autumnMusic;
    [SerializeField]
    private AudioSource winterMusic;
    [SerializeField]
    float switchTime;
    [SerializeField]
    float maxVolume;


    private Dictionary<Season, AudioSource> seasonMusicMapping;
    private SeasonManager seasonManager;

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

        seasonManager = FindObjectOfType<SeasonManager>();
        seasonManager.Subscribe(ChangeBGMusic);
        ChangeBGMusic();
    }

    private void ChangeBGMusic()
    {
        StopAllCoroutines();

        foreach (var item in seasonMusicMapping)
        {
            if (item.Key == seasonManager.season)
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
