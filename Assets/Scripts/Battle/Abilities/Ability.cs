using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace TUFG.Battle.Abilities
{
    /// <summary>
    /// Enumeration of ability targetting types.
    /// </summary>
    public enum AbilityTargetting
    {
        Single,
        Adjescent,
        All,
        Ally,
        Self,
        AllAllies
    }

    /// <summary>
    /// Enumeration of targetting priorities.
    /// </summary>
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

    /// <summary>
    /// The class containing information about a single ability.
    /// </summary>
    [Serializable]
    public class Ability
    {
        public string name;
        public string abilityID;
        public AbilityTargetting targetting;
        public AbilityPriority priority;
        public int priorityStrenght;

        public AbilityEffect[] primaryEffects;
        public AbilityEffect[] secondaryEffects;
    }
}
