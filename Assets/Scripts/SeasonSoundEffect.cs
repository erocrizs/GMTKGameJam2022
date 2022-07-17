using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonSoundEffect : MonoBehaviour
{

    public AudioClip springAudio;
    public AudioClip summerAudio;
    public AudioClip autumnAudio;
    public AudioClip winterAudio;

    private Dictionary<Season, AudioClip> seasonAudioMapping;
    private Season currentSeason;
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
        currentSeason = newSeason;
        audioSource.PlayOneShot(seasonAudioMapping[newSeason]);
    }
}
