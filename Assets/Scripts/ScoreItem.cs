using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public ItemData itemdata;
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = itemdata.itemSprite;
    }
}
