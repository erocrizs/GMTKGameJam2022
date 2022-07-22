using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeasonalPlayer : MonoBehaviour
{
    [SerializeField]
    float switchTime;
    [SerializeField]
    float maxVolume;
    [SerializeField]
    SeasonalAudioClip[] audioClips;
    Dictionary<Season, AudioSource> seasonMusicMapping;
    Season currentSeason;

    const float MUSIC_STEP = 10;
    float CoroutineDelay => switchTime / MUSIC_STEP;

    // Start is called before the first frame update
    void Start()
    {
        currentSeason = SeasonManager.Main.season;
        seasonMusicMapping = new Dictionary<Season, AudioSource>();
        foreach (var pair in audioClips)
        {
            var childObject = new GameObject(pair.season.ToString());
            childObject.transform.parent = transform;
            var audioSource = childObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = true;
            audioSource.loop = true;
            audioSource.volume = (currentSeason == pair.season) ? 1f : 0f;
            audioSource.spatialBlend = 0;
            audioSource.clip = pair.audioClip;
            audioSource.Play();
            seasonMusicMapping[pair.season] = audioSource;
        }
    }

    void Update()
    {
        var currentSeason = SeasonManager.Main.season;
        if (this.currentSeason != currentSeason)
        {
            this.currentSeason = currentSeason;
            ChangeBGMusic(this.currentSeason);
        }
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

    [Serializable]
    private struct SeasonalAudioClip
    {
        public Season season;
        public AudioClip audioClip;
    }
}
