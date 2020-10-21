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
    /// <summary>
    /// Class responsible for controling battles.
    /// </summary>
    /// <remarks>Uses a singleton pattern.</remarks>
    public class BattleManager : MonoBehaviour
    {
        #region Singleton pattern
        private static BattleManager _instance;

        /// <summary>
        /// Current instance of the battle manager. Creates an Unity object.
        /// </summary>
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

        private static bool isSelectingAbility = false;
        private static bool isSelectingTarget = false;

        private static UnityEngine.Object unitPrefab = null;
        private static UnityEngine.Object UnitPrefab
        {
            get
            {
                if(unitPrefab == null)
                    unitPrefab = Resources.Load("Battle/Unit");

                return unitPrefab;
            }
        }

        /// <summary>
        /// Initialize a battle with array of allies and enemies
        /// </summary>
        /// <param name="allies"></param>
        /// <param name="enemies"></param>
        public void InitBattle(UnitData[] enemies)
        {
            currentBattle = new Battle();

            FindObjectOfType<PlayerMovement>().DisableInput();

            InstantiateBattle(enemies);
            Instance.StartCoroutine(ProcessBattle());
        }

        /// <summary>
        /// Instantiates all units.
        /// </summary>
        private void InstantiateBattle(UnitData[] enemyData)
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

                currentBattle.allies = new List<Unit>();
                currentBattle.enemies = new List<Unit>();

                Vector2 position = battleArena.position - (Vector3.up * 1);
                position.x -= 1;

                for (int i = 0; i < alliesData.Length; i++)
                {
                    currentBattle.allies.Add(InstantiateUnit(alliesData[i], position, true));

                    position.x--;
                }

                position = battleArena.position - (Vector3.up * 1);
                position.x += 1;

                for (int i = 0; i < enemyData.Length; i++)
                {
                    currentBattle.enemies.Add(InstantiateUnit(enemyData[i], position, false));
                    position.x++;
                }
            }
        }

        /// <summary>
        /// Creates an instance of a unit.
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="position"></param>
        private Unit InstantiateUnit(UnitData unitData, Vector2 position, bool isAlly)
        {
            GameObject unitObject = (GameObject)Instantiate(UnitPrefab);

            unitObject.name = unitData.name;
            unitObject.transform.position = position;

            SpriteRenderer sr = unitObject.GetComponent<SpriteRenderer>();
            sr.flipX = !isAlly;

            Animator animator = unitObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = unitData.animator;

            Unit unit = unitObject.GetComponent<Unit>();
            unit.UnitData = unitData;
            unit.IsAlly = isAlly;
            unit.IsPlayer = unitData.aiType == UnitAIType.Player;
            unit.Health = unit.MaxHealth;
            unit.UpdateHealthUI();

            return unit;
        }

        /// <summary>
        /// Builds turn order based on speed attribute of units in the current battle.
        /// </summary>
        public void BuildTurnOrder()
        {
            IOrderedEnumerable<Unit> orderedUnits = currentBattle.allies.Concat(currentBattle.enemies).OrderByDescending(x => ResolveDiceRoll(x.Speed));
            turnOrder = orderedUnits.ToList();
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
        /// Use an ability by a unit on a target.
        /// </summary>
        /// <remarks>Gets all the valid targets of the ability and applies all primary and secondary effects on the targets.</remarks>
        /// <param name="author">Unit that uses the ability.</param>
        /// <param name="ability">Ability to be used.</param>
        /// <param name="target">Target of the ability.</param>
        private void UseAbility(Unit author, Ability ability, Unit target)
        {
            ApplyAbilityEffect(author, ability, target, ability.primaryEffects);

            switch (ability.targetting)
            {
                case AbilityTargetting.All:
                    ApplyAbilityEffect(author, ability, author.IsAlly ? currentBattle.enemies : currentBattle.allies, ability.secondaryEffects);
                    break;

                case AbilityTargetting.AllAllies:
                    ApplyAbilityEffect(author, ability, author.IsAlly ? currentBattle.allies : currentBattle.enemies, ability.secondaryEffects);
                    break;

                case AbilityTargetting.Adjescent:
                    List<Unit> targets = new List<Unit>();
                    List<Unit> possibleTargets = target.IsAlly ? currentBattle.allies : currentBattle.enemies;

                    int index = possibleTargets.IndexOf(target);
                    
                    if (index < 0 || index >= possibleTargets.Count)
                        break;

                    if (index > 0)
                        targets.Add(possibleTargets[index - 1]);
                    if (index < possibleTargets.Count - 1)
                        targets.Add(possibleTargets[index + 1]);

                    ApplyAbilityEffect(author, ability, targets, ability.secondaryEffects);
                    break;
            }
        }

        /// <summary>
        /// Apply an ability effect on a unit.
        /// </summary>
        /// <param name="author">Unit that used the ability.</param>
        /// <param name="ability">The ability used.</param>
        /// <param name="target">Target of the effect (not target of the ability).</param>
        /// <param name="effects">Array of ability effects to use on the target.</param>
        private void ApplyAbilityEffect(Unit author, Ability ability, Unit target, AbilityEffect[] effects)
        {
            List<Unit> targets = new List<Unit> { target };
            ApplyAbilityEffect(author, ability, targets, effects);
        }

        /// <summary>
        /// Apply an ability effect on multiple units.
        /// </summary>
        /// <param name="author">Unit that used the ability.</param>
        /// <param name="ability">The ability used.</param>
        /// <param name="targets">Array of units that are targets of the effect (not target of the ability).</param>
        /// <param name="effects">Array of ability effects to use on the targets.</param>
        private void ApplyAbilityEffect(Unit author, Ability ability, List<Unit> targets, AbilityEffect[] effects)
        {
            Animator animator = author.GetComponent<Animator>();
            if (animator.parameters.Select(x => x.name == ability.name).Count() > 0)
                animator.SetTrigger(ability.name);

            foreach (AbilityEffect effect in effects)
            {
                foreach(Unit target in targets)
                {
                    switch (effect.effectType)
                    {
                        case AbilityEffectType.Damage:
                            target.DealDamage((int)(effect.effectValue + effect.strenghtMultiplier * author.Strength + effect.powerMultiplier * author.Power));
                            if (target.Health <= 0)
                                KillUnit(target);
                            break;
                        case AbilityEffectType.Heal:
                            target.Heal((int)(effect.effectValue + effect.strenghtMultiplier * author.Strength + effect.powerMultiplier * author.Power));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Kill a unit in the battle.
        /// </summary>
        /// <param name="target">Unit to be killed.</param>
        private void KillUnit(Unit target)
        {
            if (target.IsAlly)
                currentBattle.allies.Remove(target);
            else
                currentBattle.enemies.Remove(target);

            if (turnOrder.Contains(target))
                turnOrder.Remove(target);

            Destroy(target.gameObject);


            if (currentBattle.allies.Count == 0 || currentBattle.enemies.Count == 0 || currentBattle == null)
                EndBattle();
        }

        /// <summary>
        /// Processes the AI turns of the battle and the player turn.
        /// </summary>
        /// <returns></returns>
        internal IEnumerator ProcessBattle()
        {
            int roundNumber = 0;

            // Main battle loop
            while(currentBattle != null) {
                BuildTurnOrder();
                roundNumber++;

                // Main round loop
                while (turnOrder.Count != 0 && currentBattle != null)
                {
                    Unit unit = turnOrder[0];
                    if (unit.IsPlayer)
                    {
                        if (!isSelectingAbility && !isSelectingTarget)
                        {
                            GenericButton[] buttons = new GenericButton[unit.Abilities.Length];

                            for (int i = 0; i < buttons.Length; i++)
                            {
                                buttons[i] = new GenericButton
                                {
                                    text = unit.Abilities[i].name,
                                    buttonType = ButtonType.Ability,
                                    ability = unit.Abilities[i]
                                };
                            }
                            UIManager.Instance.ShowBattleActions(buttons, "Choose your action");

                            isSelectingAbility = true;
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
                            UseAbility(unit, ability, target);
                        }
                        else
                        {
                            Debug.LogError($"ProcessBattle: No ability or target from {unit.name}!");
                        }
                        yield return new WaitForSeconds(2);
                        turnOrder.Remove(unit);
                    }
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

            isSelectingTarget = true;
            isSelectingAbility = false;

            List<Unit> targets = new List<Unit>();

            switch (ability.targetting)
            {
                case (AbilityTargetting.Self):
                    targets = currentBattle.allies.Where(x => x.IsPlayer).ToList();
                    break;
                case (AbilityTargetting.Single):
                case (AbilityTargetting.Adjescent):
                case (AbilityTargetting.All):
                    targets = currentBattle.enemies;
                    break;
                case (AbilityTargetting.Ally):
                case (AbilityTargetting.AllAllies):
                    targets = currentBattle.allies;
                    break;
            }

            List<GenericButton> buttons = new List<GenericButton>();

            foreach(Unit target in targets)
            {
                buttons.Add(new GenericButton
                {
                    text = target.Name,
                    buttonType = ButtonType.Target,
                    target = target
                });
            }
            UIManager.Instance.ShowBattleActions(buttons.ToArray(), "Select target");
        }

        /// <summary>
        /// Select target for the current ability.
        /// </summary>
        /// <param name="target">Selected unit</param>
        public void SelectTarget(Unit target)
        {
            isSelectingTarget = false;
            isSelectingAbility = false;

            UseAbility(turnOrder[0], currentAbility, target);

            UIManager.Instance.HideActions();

            turnOrder.Remove(turnOrder[0]);
        }

        /// <summary>
        /// End the current battle.
        /// </summary>
        public void EndBattle()
        {
            currentBattle = null;

            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            player.EnableInput();

            WorldCamera camera = FindObjectOfType<WorldCamera>();
            camera.SetPosition(player.transform.position + (Vector3.up * 1));
            camera.SetTarget(player.transform);

            Instance.StartCoroutine(ProcessBattle());
        }
    }
}
