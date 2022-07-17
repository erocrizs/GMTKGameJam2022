using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliveGroundActivation : MonoBehaviour
{
    [SerializeField]
    OliveDetector aboveDetector;

    [SerializeField]
    OliveDetector belowDetector;

    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        aboveDetector.SubscribeEntrance(ChangeActivation);
        aboveDetector.SubscribeExit(ChangeActivation);
        belowDetector.SubscribeEntrance(ChangeActivation);
        belowDetector.SubscribeExit(ChangeActivation);
    }

    void ChangeActivation()
    {
        bool enabled = (aboveDetector.IsOliveInside && !belowDetector.IsOliveInside);

        boxCollider.enabled = enabled;
    }
}
