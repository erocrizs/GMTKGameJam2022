using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagDespawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TagDetector>().OnEnter.SubscribeOnce(Despawn);
    }

    // Update is called once per frame
    void Despawn(GameObject target)
    {
        target.SetActive(false);
    }
}
