using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUFG.Battle.Abilities;
using UnityEditor;

namespace TUFG.Battle.AI
{
    /// <summary>
    /// Type of unit AI.
    /// </summary>
    public enum UnitAIType
    {
        Random,
        Player
    }

    /// <summary>
    /// Class that handles unit AI during battles.
    /// </summary>
    public static class UnitAI
    {
        /// <summary>
        /// Get an ability based on the current battle and the AI of the unit.
        /// </summary>
        /// <param name="battle">Current battle.</param>
        /// <param name="unit">Unit that is using an ability at the moment.</param>
        /// <param name="ability">The resulting ability to be used.</param>
        /// <param name="target">The target that the ability is used on.</param>
        public static void GetChosenAbility(Battle battle, Unit unit, out Ability ability, out Unit target)
        {
            ability = null;
            target = null;

            switch (unit.UnitData.aiType)
            {
                case UnitAIType.Random:
                    GetRandomAbility(battle, unit, out ability, out target);
                    break;
            }
        }

        /// <summary>
        /// Gets a random ability from the units abilities and targets it on a valid target.
        /// </summary>
        /// <param name="battle">Current battle.</param>
        /// <param name="unit">Unit that is using an ability at the moment.</param>
        /// <param name="ability">The resulting ability to be used.</param>
        /// <param name="target">The target that the ability is used on.</param>
        internal static void GetRandomAbility(Battle battle, Unit unit, out Ability ability, out Unit target)
        {
            ability = null;
            target = null;

            if (battle == null)
            {
                Debug.LogErrorFormat("GetChosenAbility: Battle is null!");
                return;
            }

            if (unit == null)
            {
                Debug.LogErrorFormat("GetChosenAbility: Unit is null!");
                return;
            }

            if (unit.Abilities == null || unit.Abilities.Length == 0)
            {
                Debug.LogErrorFormat("GetChosenAbility: Unit {0} has no abilities!", unit.name);
                return;
            }

            ability = unit.Abilities[Random.Range(0, unit.Abilities.Length)];

            List<Unit> allies = battle.allies;
            List<Unit> enemies = battle.enemies;

            if (!unit.IsAlly)
            {
                allies = battle.enemies;
                enemies = battle.allies;
            }

            int index;
            switch (ability.targetting)
            {
                case (AbilityTargetting.Self):
                    target = unit;
                    break;
                case (AbilityTargetting.Single):
                case (AbilityTargetting.Adjescent):
                case (AbilityTargetting.All):
                    index = Random.Range(0, enemies.Count);
                    target = enemies[index];
                    break;
                case (AbilityTargetting.Ally):
                case (AbilityTargetting.AllAllies):
                    index = Random.Range(0, allies.Count);
                    target = allies[index];
                    break;
            }
        }
    }
}
