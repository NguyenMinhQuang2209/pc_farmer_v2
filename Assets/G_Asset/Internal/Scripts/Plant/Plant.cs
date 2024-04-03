using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private float growingTime = 1f;
    [SerializeField] private Vector2Int collects = new();
    [SerializeField] private List<Sprite> growingSprites = new();

    public float GetGrowingTime()
    {
        return growingTime;
    }

    public Sprite GetSprite(int index)
    {
        return index < growingSprites.Count ? growingSprites[index] : growingSprites[^1];
    }
    public float TotalPeriodPerTime()
    {
        return growingTime / growingSprites.Count;
    }

    public int Collecting()
    {
        int min = Mathf.Min(collects.x, collects.y);
        if (min < 0)
        {
            min = 0;
        }
        int max = Mathf.Max(collects.x, collects.y);
        if (max < 0)
        {
            max = 0;
        }
        int ran = Random.Range(min, max + 1);
        return ran;
    }
}
[System.Serializable]
public class PlantItem
{
    public Plant plant;
    public Sprite seedSprite;
    public ItemName itemName;
}