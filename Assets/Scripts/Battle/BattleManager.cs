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
        private static List<Unit> turnOrder = null;

        /// <summary>
        /// Initialize a battle with array of allies and enemies
        /// </summary>
        /// <param name="allies"></param>
        /// <param name="enemies"></param>
        public static void InitBattle(UnitData[] allies, UnitData[] enemies)
        {
            currentBattle = new Battle();

            InstantiateBattle(allies, enemies);
            BuildTurnOrder();
        }

        /// <summary>
        /// Instantiates all units.
        /// </summary>
        private static void InstantiateBattle(UnitData[] alliesData, UnitData[] enemyData)
        {
            if (currentBattle == null)
                return;

            currentBattle.allies = new Unit[alliesData.Length];
            currentBattle.enemies = new Unit[enemyData.Length];

            Vector2 position = new Vector2(-4, 1);

            for (int i = 0; i < alliesData.Length; i++)
            {
                currentBattle.allies[i] = InstantiateUnit(alliesData[i], position);
                currentBattle.allies[i].IsAlly = true;

                position.x++;
            }

            position.x++;

            for (int i = 0; i < enemyData.Length; i++)
            {
                currentBattle.enemies[i] = InstantiateUnit(enemyData[i], position);
                currentBattle.allies[i].IsAlly = false;
                position.x++;
            }
        }

        /// <summary>
        /// Creates an instance of a unit.
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="position"></param>
        private static Unit InstantiateUnit(UnitData unitData, Vector2 position)
        {
            GameObject unitObject = new GameObject(unitData.name);

            unitObject.transform.position = position;

            SpriteRenderer sr = unitObject.AddComponent<SpriteRenderer>();

            Animator animator = unitObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = unitData.animator;

            Unit unit = unitObject.AddComponent<Unit>();
            unit.name = unitData.name;
            unit.UnitData = unitData;

            return unit;
        }

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

            IOrderedEnumerable<Unit> orderedUnits = currentBattle.allies.Concat(currentBattle.enemies).OrderBy(x => ResolveDiceRoll(x.Speed));
            turnOrder = orderedUnits.ToList();

            DebugBattle();
        }

        internal static void DebugBattle()
        {
            string message = "Current turn order: ";
            foreach(Unit unit in turnOrder)
            {
                string ally = unit.IsAlly ? "ally" : "enemy";
                message += $"{unit.name} ({ally}), ";
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
