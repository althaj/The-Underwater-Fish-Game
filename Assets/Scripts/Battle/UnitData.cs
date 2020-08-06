using System.Collections;
using System.Collections.Generic;
using TUFG.Battle.Abilities;
using UnityEngine;

namespace TUFG.Battle
{
    [CreateAssetMenu(fileName = "New Unit Data", menuName = "Battle/Unit Data")]
    public class UnitData : ScriptableObject
    {
        public string unitID = "";
        public new string name = "";

        public int maxHealth;
        public int health;
        public int armor;
        public int speed;

        public Ability[] abilities;
    }
}
