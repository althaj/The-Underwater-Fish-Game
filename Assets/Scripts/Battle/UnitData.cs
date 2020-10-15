using System.Collections;
using System.Collections.Generic;
using TUFG.Battle.Abilities;
using TUFG.Battle.AI;
using UnityEditor.Animations;
using UnityEngine;

namespace TUFG.Battle
{
    [CreateAssetMenu(fileName = "New Unit Data", menuName = "Battle/Unit Data")]
    public class UnitData : ScriptableObject
    {
        public string unitID = "";
        public new string name = "";

        public AnimatorController animator = null;
        public UnitAIType aiType;

        public int maxHealth;
        public int health;
        public int armor;
        public int speed;
        public int strength;
        public int power;

        public Ability[] abilities;
    }
}
