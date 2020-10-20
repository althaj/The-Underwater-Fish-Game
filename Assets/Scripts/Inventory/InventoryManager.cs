using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TUFG.Battle.Abilities;
using TUFG.Core;
using UnityEditor;
using UnityEngine;

namespace TUFG.Inventory
{
    /// <summary>
    /// Enumeration of item slots.
    /// </summary>
    public enum ItemSlot
    {
        Hands,
        Body,
        Legs,
        Amulet,
        Ring
    }

    /// <summary>
    /// Class managing the inventory of the player.
    /// </summary>
    /// <remarks>Uses a singleton pattern.</remarks>
    public class InventoryManager : MonoBehaviour
    {
        #region Singleton pattern
        private static InventoryManager _instance;

        /// <summary>
        /// Current instance of the inventory manager. Creates an Unity object.
        /// </summary>
        public static InventoryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<InventoryManager>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("Inventory");
                        _instance = container.AddComponent<InventoryManager>();

                        _instance.inventoryItems = new List<Item>();
                        _instance.equippedItems = new List<Item>();
                    }

                    GameManager.LoadPlayerItems();
                }

                return _instance;
            }
        }
        #endregion

        private List<Item> inventoryItems;
        private List<Item> equippedItems;
        private int gold;

        private static UnityEngine.Object droppedItemPrefab = null;
        private static UnityEngine.Object DroppedItemPrefab
        {
            get
            {
                if (droppedItemPrefab == null)
                    droppedItemPrefab = Resources.Load("Inventory/DroppedItem");

                return droppedItemPrefab;
            }
        }

        #region Public methods
        /// <summary>
        /// Equip an item that is in the player's inventory.
        /// </summary>
        /// <param name="item">Item to be equipped.</param>
        public void EquipItem(Item item)
        {
            if (!inventoryItems.Contains(item))
            {
                Debug.LogError($"Cannot equip item {item.name}, because it's not in the inventory!");
                return;
            }

            Item previousItem = GetItemAtSlot(item.slot);
            if (previousItem != null)
            {
                equippedItems.Remove(previousItem);
                inventoryItems.Add(previousItem);
            }

            equippedItems.Add(item);
            inventoryItems.Remove(item);
        }

        /// <summary>
        /// Unequip an item that is in the player's inventory.
        /// </summary>
        /// <param name="item">Item to be unequipped.</param>
        public void UnequipItem(Item item)
        {
            if (!equippedItems.Contains(item))
            {
                Debug.LogError($"Cannot unequip item {item.name}, because it's not equipped!");
                return;
            }

            equippedItems.Remove(item);
            inventoryItems.Add(item);
        }

        /// <summary>
        /// Get an item into the inventory.
        /// </summary>
        /// <param name="item">Item to put into the inventory.</param>
        public void PickUpItem(Item item)
        {
            inventoryItems.Add(item);
        }

        /// <summary>
        /// Drop an item that is currently in the inventory.
        /// </summary>
        /// <param name="item">Item to drop.</param>
        /// <returns>True if success, false if error.</returns>
        public bool DropItem(Item item)
        {
            if (RemoveItem(item))
            {
                GameObject droppedObject = (GameObject)Instantiate(DroppedItemPrefab);
                droppedObject.GetComponent<DroppedItem>().Init(item);
                droppedObject.transform.position = FindObjectOfType<PlayerMovement>().transform.position;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an item that is currently in the inventory. Does not create a dropped item.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>True if success, false if error.</returns>
        public bool RemoveItem(Item item)
        {
            if (!inventoryItems.Contains(item))
            {
                if (!equippedItems.Contains(item))
                {
                    Debug.LogError($"Cannot remove item {item.name}, because it's not in the inventory!");
                    return false;
                }
                equippedItems.Remove(item);
            }
            else
            {
                inventoryItems.Remove(item);
            }

            return true;
        }

        /// <summary>
        /// Get an equipped item on a slot.
        /// </summary>
        /// <param name="slot">Inventory slot of the item.</param>
        /// <returns>Item at the slot. Returns null if the slot is empty.</returns>
        public Item GetItemAtSlot(ItemSlot slot)
        {
            return equippedItems.Where(x => x.slot == slot).FirstOrDefault();
        }

        /// <summary>
        /// Gets the bonus stats of a type.
        /// </summary>
        /// <param name="type">Type of the bonus stats.</param>
        /// <returns>Bonus stats of the type.</returns>
        public int GetStatBonuses(ItemStatType type)
        {
            int result = 0;

            foreach (Item item in equippedItems)
            {
                foreach (ItemStat stat in item.buffs)
                {
                    if (stat.statType == type)
                        result += stat.value;
                }

                foreach (ItemStat stat in item.pentalties)
                {
                    if (stat.statType == type)
                        result -= stat.value;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns all abilities from equipped items.
        /// </summary>
        /// <returns>Array of abilities.</returns>
        public Ability[] GetEquippedAbilities()
        {
            return equippedItems.SelectMany(x => x.abilities).ToArray();
        }

        /// <summary>
        /// Returns paths to the equipped items.
        /// </summary>
        /// <returns></returns>
        public List<string> GetEquippedItemPaths()
        {
            List<string> paths = new List<string>();
            foreach (Item item in EquippedItems)
            {
                paths.Add(AssetDatabase.GetAssetPath(item));
            }
            return paths;
        }

        /// <summary>
        /// Returns paths to the inventory items.
        /// </summary>
        /// <returns></returns>
        public List<string> GetInventoryItemPaths()
        {
            List<string> paths = new List<string>();
            foreach (Item item in InventoryItems)
            {
                paths.Add(AssetDatabase.GetAssetPath(item));
            }
            return paths;
        }

        /// <summary>
        /// Load items from their paths.
        /// </summary>
        /// <param name="equippedPaths"></param>
        /// <param name="inventoryPaths"></param>
        public void LoadItemsFromPaths(List<string> equippedPaths, List<string> inventoryPaths)
        {
            EquippedItems = new List<Item>();
            InventoryItems = new List<Item>();

            foreach (string path in equippedPaths)
            {
                Item item = AssetDatabase.LoadAssetAtPath<Item>(path);
                if (item != null && !string.IsNullOrEmpty(item.id))
                    EquippedItems.Add(item);
            }

            foreach (string path in inventoryPaths)
            {
                Item item = AssetDatabase.LoadAssetAtPath<Item>(path);
                if (item != null && !string.IsNullOrEmpty(item.id))
                    InventoryItems.Add(item);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of unequipped items in the inventory.
        /// </summary>
        public List<Item> InventoryItems { get => inventoryItems; set => inventoryItems = value; }

        /// <summary>
        /// List of equipped items in the inventory.
        /// </summary>
        public List<Item> EquippedItems { get => equippedItems; set => equippedItems = value; }

        /// <summary>
        /// Current gold of the player.
        /// </summary>
        public int Gold { get => gold; set => gold = value; }
        #endregion
    }
}
