using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TUFG.World;
using TUFG.Camera;
using TUFG.Core;
using TUFG.Battle.Abilities;
using TUFG.Battle.AI;
using TUFG.UI;
using TUFG.Dialogue;

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
        // Current player selected ability
        private static Ability currentAbility = null;

        private static bool isPlayerTurn = false;

        /// <summary>
        /// Initialize a battle with array of allies and enemies
        /// </summary>
        /// <param name="allies"></param>
        /// <param name="enemies"></param>
        public void InitBattle(UnitData[] enemies)
        {
            currentBattle = new Battle();

            InstantiateBattle(enemies);
            BuildTurnOrder();
            Instance.StartCoroutine(ProcessBattle());
        }

        /// <summary>
        /// Instantiates all units.
        /// </summary>
        private static void InstantiateBattle(UnitData[] enemyData)
        {
            if (currentBattle == null)
                return;

            UnitData[] alliesData = GameManager.GetPlayerParty();

            Transform battleArena = FindObjectOfType<WorldInfo>().GetRandomArena();
            if (battleArena != null)
            {
                WorldCamera camera = FindObjectOfType<WorldCamera>();
                camera.SetPosition(battleArena.position + (Vector3.up * 1));
                camera.SetTarget(battleArena);

                currentBattle.allies = new Unit[alliesData.Length];
                currentBattle.enemies = new Unit[enemyData.Length];

                Vector2 position = battleArena.position - (Vector3.up * 1);
                position.x -= 2;

                for (int i = 0; i < alliesData.Length; i++)
                {
                    currentBattle.allies[i] = InstantiateUnit(alliesData[i], position, true);
                    currentBattle.allies[i].IsAlly = true;
                    if (alliesData[i].aiType == AI.UnitAIType.Player)
                        currentBattle.allies[i].IsPlayer = true;

                    position.x--;
                }

                position = battleArena.position - (Vector3.up * 1);
                position.x += 2;

                for (int i = 0; i < enemyData.Length; i++)
                {
                    currentBattle.enemies[i] = InstantiateUnit(enemyData[i], position, false);
                    currentBattle.enemies[i].IsAlly = false;
                    position.x++;
                }
            }
        }

        /// <summary>
        /// Creates an instance of a unit.
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="position"></param>
        private static Unit InstantiateUnit(UnitData unitData, Vector2 position, bool isAlly)
        {
            GameObject unitObject = new GameObject(unitData.name);

            unitObject.transform.position = position;

            SpriteRenderer sr = unitObject.AddComponent<SpriteRenderer>();
            sr.flipX = !isAlly;

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
            if (currentBattle == null)
            {
                turnOrder = null;
                return;
            }

            IOrderedEnumerable<Unit> orderedUnits = currentBattle.allies.Concat(currentBattle.enemies).OrderBy(x => ResolveDiceRoll(x.Speed));
            turnOrder = orderedUnits.ToList();
        }

        internal static void DebugBattle()
        {
            string message = "Current turn order: ";
            foreach (Unit unit in turnOrder)
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

        /// <summary>
        /// Processes the AI turns of the battle and the player turn.
        /// </summary>
        /// <returns></returns>
        internal static IEnumerator ProcessBattle()
        {
            while (turnOrder.Count != 0)
            {
                Unit unit = turnOrder[0];
                if (unit.IsPlayer)
                {
                    if (!isPlayerTurn)
                    {
                        Debug.Log("Player turn");

                        Button[] buttons = new Button[unit.Abilities.Length];

                        for (int i = 0; i < buttons.Length; i++)
                        {
                            buttons[i] = new Button
                            {
                                text = unit.Abilities[i].name,
                                buttonType = ButtonType.Ability,
                                ability = unit.Abilities[i]
                            };
                        }
                        UIManager.Instance.ShowBattleActions(buttons);

                        isPlayerTurn = true;
                    }
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    Ability ability = null;
                    Unit target = null;

                    UnitAI.GetChosenAbility(currentBattle, unit, out ability, out target);

                    if (ability != null && target != null)
                    {
                        string abilityString = $"{unit.name} used {ability.name} on {target.name}";
                        switch (ability.targetting)
                        {
                            case AbilityTargetting.Single:
                            case AbilityTargetting.Self:
                            case AbilityTargetting.Ally:
                                abilityString += $", dealing {ability.primaryEffects[0].effectValue} damage.";
                                break;
                            case AbilityTargetting.All:
                            case AbilityTargetting.AllAllies:
                            case AbilityTargetting.Adjescent:
                                abilityString += $", dealing {ability.primaryEffects[0].effectValue} damage and hitting other units for {ability.secondaryEffects[0].effectValue}.";
                                break;
                        }

                        Debug.Log(abilityString);
                        yield return new WaitForSeconds(2);
                    }
                    else
                    {
                        Debug.LogError($"ProcessBattle: No ability or target from {unit.name}!");
                    }
                    turnOrder.Remove(unit);
                }
            }
        }

        /// <summary>
        /// Select current ability for player, followed buy the process of selecting the target.
        /// </summary>
        /// <param name="ability">Selected ability</param>
        public void SelectAbility(Ability ability)
        {
            currentAbility = ability;

            // TODO target selection
            Debug.Log($"Player selected ability {currentAbility.name}.");
        }
    }
}
