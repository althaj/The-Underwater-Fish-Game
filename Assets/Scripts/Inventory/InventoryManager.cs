using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TUFG.Inventory
{
    public enum ItemSlot
    {
        Hands,
        Body,
        Legs,
        Amulet,
        Ring
    }

    public class InventoryManager : MonoBehaviour
    {
        #region Singleton pattern
        private static InventoryManager _instance;
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

                        inventoryItems = new List<Item>();
                        equippedItems = new List<Item>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        private static List<Item> inventoryItems;
        private static List<Item> equippedItems;

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
                equippedItems.Remove(previousItem);

            equippedItems.Add(item);
        }

        /// <summary>
        /// Unequip an item that is in the player's inventory.
        /// </summary>
        /// <param name="item">Item to be unequipped.</param>
        public void UnequipItem(Item item)
        {
            if (!inventoryItems.Contains(item))
            {
                Debug.LogError($"Cannot unequip item {item.name}, because it's not in the inventory!");
                return;
            }

            equippedItems.Remove(item);
        }

        /// <summary>
        /// Get an item into the inventory.
        /// </summary>
        /// <param name="item">Item to put into the inventory.</param>
        public void GetItem(Item item)
        {
            inventoryItems.Add(item);
        }

        /// <summary>
        /// Drop an item that is currently in the inventory.
        /// </summary>
        /// <param name="item">Item to drop.</param>
        public void DropItem(Item item)
        {
            if (!inventoryItems.Contains(item))
            {
                Debug.LogError($"Cannot drop item {item.name}, because it's not in the inventory!");
                return;
            }

            inventoryItems.Remove(item);
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
                foreach(ItemStat stat in item.buffs)
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
        #endregion

        #region Properties
        public static List<Item> InventoryItems { get => inventoryItems; }
        public static List<Item> EquippedItems { get => equippedItems; }
        #endregion
    }
}
