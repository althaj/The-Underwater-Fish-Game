using System.Collections;
using System.Collections.Generic;
using TUFG.Battle.Abilities;
using UnityEditor;
using UnityEngine;

namespace TUFG.Inventory
{
    [CreateAssetMenu(fileName = "New item", menuName = "Inventory/Create item")]
    public class Item : ScriptableObject
    {
        public new string name;
        [TextArea(4, 10)]
        public string description;
        public string id;
        public Ability[] abilities;
        public ItemStat[] buffs;
        public ItemStat[] pentalties;
        public ItemSlot slot;

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
                    default:
                        Debug.LogError($"Item slot {slot.ToString()} doesn't have a text representation!");
                        return "";
                }
            }
        }
    }
}
