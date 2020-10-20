using UnityEngine;
using UnityEditor;
using System;

namespace TUFG.Inventory
{
    /// <summary>
    /// Enumeration of all the item stat types in the game.
    /// </summary>
    public enum ItemStatType
    {
        Health,
        Armor,
        Speed,
        Strenght,
        Power
    }

    /// <summary>
    /// Single instance of an item stat.
    /// </summary>
    [Serializable]
    public class ItemStat
    {
        /// <summary>
        /// Name of the item stat.
        /// </summary>
        public string name;

        /// <summary>
        /// Type of the item stat.
        /// </summary>
        public ItemStatType statType;

        /// <summary>
        /// Value of the item stat buff or penalty.
        /// </summary>
        public int value;
    }
}