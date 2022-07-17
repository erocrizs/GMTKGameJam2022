using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliveDespawner : MonoBehaviour
{
    GameObject olive;

    void Start()
    {
        olive = FindObjectOfType<OliveMovement>().gameObject;
        OliveDetector detector = GetComponentInChildren<OliveDetector>();
        detector.SubscribeEntranceOnce(DespawnOlive);
    }

    // Update is called once per frame
    void DespawnOlive()
    {
        olive.SetActive(false);
    }
}
