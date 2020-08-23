using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUFG.Battle.Abilities;
using TUFG.Battle.AI;

namespace TUFG.Battle
{
    public class Unit: MonoBehaviour
    {
        [SerializeField] private new string name;
        [SerializeField] private UnitData unitData;

        public bool IsAlly { get; set; }
        public bool IsPlayer { get; set; } = false;
        public int Speed { get => unitData.speed; private set => unitData.speed = value; }
        public Ability[] Abilities { get => UnitData.abilities; set => UnitData.abilities = value; }
        public UnitData UnitData { get => unitData; set => unitData = value; }
    }
}
