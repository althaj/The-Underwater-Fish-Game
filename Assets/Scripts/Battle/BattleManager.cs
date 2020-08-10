using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace TUFG.Battle
{
    public class BattleManager : MonoBehaviour
    {
        #region Singleton pattern
        private static BattleManager _instance;
        public static BattleManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<BattleManager>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("Battle Manager");
                        _instance = container.AddComponent<BattleManager>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        private static Battle currentBattle = null;
        // Array of units with bool representing wheter the unit is ally or not
        private static Tuple<Unit, bool>[] turnOrder = null;

        /// <summary>
        /// Builds turn order based on speed attribute of units in the current battle.
        /// </summary>
        public static void BuildTurnOrder()
        {
            if(currentBattle == null)
            {
                turnOrder = null;
                return;
            }

            List<Tuple<Tuple<Unit, bool>, int>> unitsWithSpeed = new List<Tuple<Tuple<Unit, bool>, int>>();
            foreach(Unit ally in currentBattle.allies)
            {
                unitsWithSpeed.Add(new Tuple<Tuple<Unit, bool>, int>(new Tuple<Unit, bool>(ally, true), ResolveDiceRoll(ally.UnitData.speed)));
            }
            foreach (Unit enemy in currentBattle.enemies)
            {
                unitsWithSpeed.Add(new Tuple<Tuple<Unit, bool>, int>(new Tuple<Unit, bool>(enemy, false), ResolveDiceRoll(enemy.UnitData.speed)));
            }

            Tuple<Unit, bool>[] orderedUnits = unitsWithSpeed.OrderBy(x => x.Item2).Select(x => x.Item1).ToArray();
            turnOrder = new Tuple<Unit, bool>[orderedUnits.Count()];
            for (int i = 0; i < turnOrder.Length; i++)
            {
                turnOrder[i] = orderedUnits[i];
            }

            DebugBattle();
        }

        public static void InitBattle(Unit[] allies, Unit[] enemies)
        {
            currentBattle = new Battle();
            currentBattle.allies = allies;
            currentBattle.enemies = enemies;
            BuildTurnOrder();
        }

        internal static void DebugBattle()
        {
            string message = "Current turn order: ";
            foreach(Tuple<Unit, bool> unit in turnOrder)
            {
                string ally = unit.Item2 ? "ally" : "enemy";
                message += $"{unit.Item1.name} ({ally}), ";
            }
            Debug.Log(message);
        }

        /// <summary>
        /// Return a sum of dice rolls.
        /// </summary>
        /// <param name="dices">Number of dice to roll.</param>
        /// <returns></returns>
        internal static int ResolveDiceRoll(int dices)
        {
            int result = 0;
            for (int i = 0; i < dices; i++)
            {
                result += UnityEngine.Random.Range(1, 7);
            }
            return result;
        }
    }
}
