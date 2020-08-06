using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace TUFG.Battle.Abilities
{
    public enum AbilityTargetting
    {
        Single,
        Adjescent,
        All,
        Ally,
        Self,
        AllAllies
    }

    public enum AbilityPriority
    {
        LowHP,
        LowPercHP,
        HighHP,
        HighPercHP,
        LowArmor,
        HighArmor,
        Random
    }

    [Serializable]
    public class Ability
    {
        public string name;
        public string abilityID;
        public AbilityTargetting targetting;
        public AbilityPriority priority;
        public int priorityStrenght;
    }
}
