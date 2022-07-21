using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonSoundEffect : MonoBehaviour
{
    [SerializeField]
    SeasonalAudioClip[] audioClips;

    Dictionary<Season, AudioClip> seasonAudioMapping;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        seasonAudioMapping = new Dictionary<Season, AudioClip>();
        foreach (var pair in audioClips)
        {
            seasonAudioMapping[pair.season] = pair.audioClip;
        }
        audioClips = null;
        audioSource = GetComponent<AudioSource>();
        SeasonManager.SubscribeToSeason(PlaySFX, false);
    }

    private void PlaySFX(Season season)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(seasonAudioMapping[season]);
    }

    [Serializable]
    private struct SeasonalAudioClip
    {
        public Season season;
        public AudioClip audioClip;
    }
}