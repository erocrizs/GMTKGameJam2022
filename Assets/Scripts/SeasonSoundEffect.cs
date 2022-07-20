using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonSoundEffect : MonoBehaviour
{
    [SerializeField]
    private AudioClip springAudio;
    [SerializeField]
    private AudioClip summerAudio;
    [SerializeField]
    private AudioClip autumnAudio;
    [SerializeField]
    private AudioClip winterAudio;

    private Dictionary<Season, AudioClip> seasonAudioMapping;
    private SeasonManager seasonManager;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        seasonAudioMapping = new Dictionary<Season, AudioClip>();
        seasonAudioMapping.Add(Season.Spring, springAudio);
        seasonAudioMapping.Add(Season.Summer, summerAudio);
        seasonAudioMapping.Add(Season.Autumn, autumnAudio);
        seasonAudioMapping.Add(Season.Winter, winterAudio);
        seasonManager = GetComponent<SeasonManager>();
        audioSource = GetComponent<AudioSource>();
        seasonManager.Subscribe(PlaySFX);
    }

    private void PlaySFX () => audioSource.PlayOneShot(seasonAudioMapping[seasonManager.season]);
}
