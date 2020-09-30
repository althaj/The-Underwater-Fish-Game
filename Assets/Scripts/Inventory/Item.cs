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
        public string id;
        public Ability[] abilities;
        public ItemStat[] buffs;
        public ItemStat[] pentalties;
        public ItemSlot slot;
    } 
}
