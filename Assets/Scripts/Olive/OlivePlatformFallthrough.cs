using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlivePlatformFallthrough : MonoBehaviour
{
    [SerializeField]
    string[] layerNames;
    [SerializeField]
    float platformIgnoreDuration;
    Collider2D oliveCollider;
    List<Collider2D> platformColliders;

    private void Start()
    {
        oliveCollider = GetComponent<Collider2D>();
        platformColliders = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical Olive") < 0 && platformColliders.Count > 0)
        {
            StartCoroutine(DisablePlatformCollision());
        }
    }

    IEnumerator DisablePlatformCollision ()
    {
        IgnorePlatformCollision(true);
        yield return new WaitForSeconds(platformIgnoreDuration);
        IgnorePlatformCollision(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsPlatformLayer(collision.gameObject))
        {
            platformColliders.Add(collision.gameObject.GetComponent<Collider2D>());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (IsPlatformLayer(collision.gameObject))
        {
            platformColliders.Remove(collision.gameObject.GetComponent<Collider2D>());
        }
    }

    bool IsPlatformLayer (GameObject gameObject)
    {
        int layerMask = LayerMask.GetMask(layerNames);
        return layerMask == (layerMask | 1 << gameObject.layer);
    }

    void IgnorePlatformCollision (bool ignore)
    {
        foreach (var collider in platformColliders)
        {
            Physics2D.IgnoreCollision(oliveCollider, collider, ignore);
        }
    }
}
