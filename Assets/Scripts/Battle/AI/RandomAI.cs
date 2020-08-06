using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TUFG.Battle.Abilities;

namespace TUFG.Battle.AI
{
    [ClassTooltip("Most basic AI - randomly chooses one ability and then randomly chooses target based on it's targetting.")]
    public class RandomAI : UnitAI
    {
        public override void GetChosenAbility(Battle battle, Unit unit, out Ability ability, out Unit target)
        {
            ability = null;
            target = null;

            if(battle == null)
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

            switch (ability.targetting)
            {
                case (AbilityTargetting.Self):
                    target = unit;
                    break;
                case (AbilityTargetting.Single):
                case (AbilityTargetting.Adjescent):
                case (AbilityTargetting.All):
                    target = battle.enemies[Random.Range(0, battle.enemies.Length)];
                    break;
                case (AbilityTargetting.Ally):
                case (AbilityTargetting.AllAllies):
                    target = battle.allies[Random.Range(0, battle.allies.Length)];
                    break;
            }

        }
    }
}
