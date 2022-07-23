using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalGroundTile : MonoBehaviour
{
    [SerializeField]
    Sprite[] spriteSheet;

    public void SetSprite(int index)
    {
        GetComponent<SpriteRenderer>().sprite = spriteSheet[index];
        Destroy(this);
    }
}
