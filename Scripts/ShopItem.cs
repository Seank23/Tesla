using UnityEngine;

[System.Serializable]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public int itemCost;
    public string ItemDescription;
    public Sprite itemThumbnail;
}
