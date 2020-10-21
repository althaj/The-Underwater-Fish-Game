using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Battle.Abilities
{
    /// <summary>
    /// Type of the effect an ability has.
    /// </summary>
    public enum AbilityEffectType
    {
        Damage,
        Heal
    }

    /// <summary>
    /// The class containing a single ability effect.
    /// </summary>
    [Serializable]
    public class AbilityEffect
    {
        public AbilityEffectType effectType = AbilityEffectType.Damage;
        public int effectValue = 0;
        public float strenghtMultiplier = 0f;
        public float powerMultiplier = 0f;
    }
}
