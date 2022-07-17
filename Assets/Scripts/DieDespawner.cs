using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieDespawner : MonoBehaviour
{
    GameObject die;

    void Start()
    {
        die = FindObjectOfType<DieMovement>().gameObject;
        DieDetector detector = GetComponentInChildren<DieDetector>();
        detector.SubscribeEntranceOnce(DespawnDie);
    }

    // Update is called once per frame
    void DespawnDie()
    {
        die.SetActive(false);
    }
}
