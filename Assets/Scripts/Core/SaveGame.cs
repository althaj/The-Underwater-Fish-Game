using System.Collections;
using System.Collections.Generic;
using TUFG.Inventory;
using UnityEngine;

/// <summary>
/// A class that holds all the data to be saved and loaded from the game.
/// </summary>
[System.Serializable]
public class SaveGame
{
    /// <summary>
    /// Paths to the scriptable objects representing equipped items.
    /// </summary>
    public List<string> equippedItemPaths;

    /// <summary>
    /// Paths to the scriptable objects representing unequipped items.
    /// </summary>
    public List<string> inventoryItemPaths;

    /// <summary>
    /// Current player gold.
    /// </summary>
    public int gold;

    /// <summary>
    /// List of all shops in the game.
    /// </summary>
    public List<ShopSave> shops;
}

/// <summary>
/// A class used to save and load data from one shop.
/// </summary>
[System.Serializable]
public class ShopSave
{
    public string shopId;
    public List<string> itemPaths;
    public float margin;
}
