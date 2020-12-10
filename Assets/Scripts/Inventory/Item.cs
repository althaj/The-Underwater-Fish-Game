using System.Collections;
using System.Collections.Generic;
using TUFG.Battle.Abilities;
using UnityEditor;
using UnityEngine;

namespace TUFG.Inventory
{
    /// <summary>
    /// Data representing an item.
    /// </summary>
    [CreateAssetMenu(fileName = "New item", menuName = "Inventory/Create item")]
    public class Item : ScriptableObject
    {
        /// <summary>
        /// Name of the item. This will be displayed in shops and in the inventory.
        /// </summary>
        public new string name;

        /// <summary>
        /// Initial price of the item before margin.
        /// </summary>
        public int price;

        /// <summary>
        /// Description of the item.
        /// </summary>
        [TextArea(4, 10)]
        public string description;

        /// <summary>
        /// ID of the item in format item_id.
        /// </summary>
        public string id;

        /// <summary>
        /// Abilities added by equipping this item.
        /// </summary>
        public Ability[] abilities;

        /// <summary>
        /// Stats that are buffed by equipping this item.
        /// </summary>
        public ItemStat[] buffs;

        /// <summary>
        /// Stats that are penalized by equipping this item.
        /// </summary>
        public ItemStat[] pentalties;

        /// <summary>
        /// Slot this item is equipped to.
        /// </summary>
        public ItemSlot slot;

        /// <summary>
        /// Text representation of the slot this item is equipped to.
        /// </summary>
        public string SlotText
        {
            get
            {
                switch (slot)
                {
                    case ItemSlot.Hands: return "Hands";
                    case ItemSlot.Body: return "Body";
                    case ItemSlot.Legs: return "Legs";
                    case ItemSlot.Amulet: return "Amulet";
                    case ItemSlot.Ring: return "Ring";
                    case ItemSlot.None: return "";
                    default:
                        Debug.LogError($"Item slot {slot.ToString()} doesn't have a text representation!");
                        return "";
                }
            }
        }
    }
}
