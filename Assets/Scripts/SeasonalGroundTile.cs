using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalGroundTile : MonoBehaviour
{
    [SerializeField]
    Season season;

    [SerializeField]
    Sprite[] autumnSheet;

    [SerializeField]
    Sprite[] winterSheet;

    [SerializeField]
    Sprite[] springSheet;

    [SerializeField]
    Sprite[] summerSheet;

    public void SetSprite(int index)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        switch (season)
        {
            case Season.Autumn:
                sr.sprite = autumnSheet[index];
                break;
            case Season.Winter:
                sr.sprite = winterSheet[index];
                break;
            case Season.Spring:
                sr.sprite = springSheet[index];
                break;
            case Season.Summer:
            default:
                sr.sprite = summerSheet[index];
                break;
        }

        Destroy(this);
    }
}
