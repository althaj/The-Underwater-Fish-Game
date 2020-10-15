using UnityEngine;
using UnityEditor;
using System;

namespace TUFG.Inventory
{
    [Serializable]
    public enum ItemStatType
    {
        Health,
        Armor,
        Speed,
        Strenght,
        Power
    }

    [Serializable]
    public class ItemStat
    {
        public string name;
        public ItemStatType statType;
        public int value;
    }
}