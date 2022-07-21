using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonSoundEffect : MonoBehaviour
{
    [SerializeField]
    AudioClip springAudio;
    [SerializeField]
    AudioClip summerAudio;
    [SerializeField]
    AudioClip autumnAudio;
    [SerializeField]
    AudioClip winterAudio;
    Dictionary<Season, AudioClip> seasonAudioMapping;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        seasonAudioMapping = new Dictionary<Season, AudioClip>
        {
            { Season.Spring, springAudio },
            { Season.Summer, summerAudio },
            { Season.Autumn, autumnAudio },
            { Season.Winter, winterAudio }
        };
        audioSource = GetComponent<AudioSource>();
        SeasonManager.SubscribeToSeason(PlaySFX, false);
    }

    private void PlaySFX (Season season) => audioSource.PlayOneShot(seasonAudioMapping[season]);
}
