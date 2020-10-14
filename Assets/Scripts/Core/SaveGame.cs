using System.Collections;
using System.Collections.Generic;
using TUFG.Inventory;
using UnityEngine;

[System.Serializable]
public class SaveGame
{
    // Inventory
    public List<string> equippedItemPaths;
    public List<string> inventoryItemPaths;
    public int gold;
    public List<ShopSave> shops;
}

[System.Serializable]
public class ShopSave
{
    public string shopId;
    public List<string> itemPaths;
    public float margin;
}
