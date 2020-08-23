using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUFG.Battle.Abilities;
using UnityEditor;

namespace TUFG.Battle.AI
{
    public enum UnitAIType
    {
        Random,
        Player
    }

    public static class UnitAI
    {
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

            Unit[] allies = battle.allies;
            Unit[] enemies = battle.enemies;

            if (!unit.IsAlly)
            {
                allies = battle.enemies;
                enemies = battle.allies;
            }

            switch (ability.targetting)
            {
                case (AbilityTargetting.Self):
                    target = unit;
                    break;
                case (AbilityTargetting.Single):
                case (AbilityTargetting.Adjescent):
                case (AbilityTargetting.All):
                    target = enemies[Random.Range(0, battle.enemies.Length)];
                    break;
                case (AbilityTargetting.Ally):
                case (AbilityTargetting.AllAllies):
                    target = allies[Random.Range(0, battle.allies.Length)];
                    break;
            }

        }
    }
}
