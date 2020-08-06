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

        public Ability[] Abilities { get => unitData.abilities; set => unitData.abilities = value; }
    }
}
