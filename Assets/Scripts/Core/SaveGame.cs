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
}
