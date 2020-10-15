using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Battle.Abilities
{
    public enum AbilityEffectType
    {
        Damage,
        Heal
    }
    [Serializable]
    public class AbilityEffect
    {
        public AbilityEffectType effectType = AbilityEffectType.Damage;
        public int effectValue = 0;
        public float strenghtMultiplier = 0f;
        public float powerMultiplier = 0f;
    }
}
