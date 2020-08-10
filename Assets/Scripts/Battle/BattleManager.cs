using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Diagnostics;

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
        private static Unit[] turnOrder = null;

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

            List<Tuple<Unit, int>> unitsWithSpeed = new List<Tuple<Unit, int>>();
            foreach(Unit units in currentBattle.allies.Union(currentBattle.enemies))
            {
                unitsWithSpeed.Add(new Tuple<Unit, int>(units, ResolveDiceRoll(units.UnitData.speed)));
            }

            unitsWithSpeed.OrderBy(x => x.Item2);
            turnOrder = new Unit[unitsWithSpeed.Count];
            for (int i = 0; i < turnOrder.Length; i++)
            {
                turnOrder[i] = unitsWithSpeed[i].Item1;
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
            foreach(Unit unit in turnOrder)
            {
                message += unit.name + ", ";
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
