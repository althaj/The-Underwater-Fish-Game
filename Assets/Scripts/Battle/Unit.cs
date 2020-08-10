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

        public Ability[] Abilities { get => UnitData.abilities; set => UnitData.abilities = value; }
        public UnitData UnitData { get => unitData; private set => unitData = value; }
    }
}
