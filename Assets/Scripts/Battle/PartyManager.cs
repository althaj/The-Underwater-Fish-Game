using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TUFG.Battle.Abilities;
using TUFG.Core;
using TUFG.Inventory;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace TUFG.Battle
{
    /// <summary>
    /// Class responsible for managing player perty.
    /// </summary>
    /// <remarks>Uses a singleton pattern.</remarks>
    public class PartyManager : MonoBehaviour
    {
        #region Singleton pattern
        private static PartyManager _instance;

        /// <summary>
        /// Current instance of the party manager. Creates an Unity object.
        /// </summary>
        public static PartyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<PartyManager>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("Party Manager");
                        _instance = container.AddComponent<PartyManager>();

                        _instance.playerParty = new List<Unit>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        public static int MAX_PARTY_SIZE = 4;
        private List<Unit> playerParty;
        private Unit playerUnit;

        #region Public methods
        /// <summary>
        /// Get party members.
        /// </summary>
        /// <param name="addPlayer">Add player as the first unit in the party.</param>
        /// <returns>List of units in the player party.</returns>
        public List<Unit> GetPlayerParty(bool addPlayer)
        {
            List<Unit> party = playerParty;

            if (addPlayer)
            {
                party.Add(playerUnit);
            }

            return party;
        }

        /// <summary>
        /// Generate a friendly, default (full HP) unit from unit data.
        /// </summary>
        /// <param name="data">Unit data to generate the unit from.</param>
        /// <returns>Generated unit from unit data.</returns>
        private Unit GetDefaultUnit(UnitData data)
        {
            Unit unit = new Unit();

            unit.UnitData = data;
            unit.Health = unit.MaxHealth;
            unit.IsAlly = true;
            unit.IsPlayer = data.aiType == AI.UnitAIType.Player;

            return unit;
        }

        /// <summary>
        /// Kick a unit out the party.
        /// </summary>
        /// <param name="unit">Unit to be kicked out.</param>
        public void KickOut(Unit unit)
        {
            if (!playerParty.Contains(unit))
            {
                Debug.LogError($"Cannot kick unit {unit.Name} out of party, because it is not in the party!");
                return;
            }

            playerParty.Remove(unit);
        }

        /// <summary>
        /// Add a unit to the player party.
        /// </summary>
        /// <param name="unit">Unit to be added.</param>
        public bool AddUnit(Unit unit)
        {
            if (CanAddUnits())
                return false;

            playerParty.Add(unit);
            return true;
        }

        /// <summary>
        /// Add a unit to the player party.
        /// </summary>
        /// <param name="unitData">Unit data for default unit creation to be added.</param>
        public bool AddUnit(UnitData unitData)
        {
            if (CanAddUnits())
                return false;

            playerParty.Add(GetDefaultUnit(unitData));
            return true;
        }

        /// <summary>
        /// Can there be more units added to the player party?
        /// </summary>
        /// <returns>True if you can add more units to your party.</returns>
        public bool CanAddUnits()
        {
            return playerParty.Count >= MAX_PARTY_SIZE;
        }

        /// <summary>
        /// Gets player party for saving the game.
        /// </summary>
        /// <returns>List of tuples with path to the unit data and current health points.</returns>
        public List<Tuple<string, int>> GetPlayerPartySave()
        {
            List<Tuple<string, int>> units = new List<Tuple<string, int>>();
            units.Add(new Tuple<string, int>(AssetDatabase.GetAssetPath(playerUnit.UnitData), playerUnit.Health));

            foreach (Unit unit in GetPlayerParty(false))
            {
                units.Add(new Tuple<string, int>(AssetDatabase.GetAssetPath(unit.UnitData), unit.Health));
            }

            return units;
        }

        /// <summary>
        /// Loads player party from saved game.
        /// </summary>
        /// <param name="units">List of tuples with path to the unit data and current health points.</param>
        public void LoadPlayerParty(List<Tuple<string, int>> units)
        {
            if (units != null && units.Count > 0)
            {
                playerParty = new List<Unit>();

                UnitData playerData = AssetDatabase.LoadAssetAtPath<UnitData>(units[0].Item1);
                playerUnit = GetDefaultUnit(playerData);
                playerUnit.Health = units[0].Item2;

                if (units.Count > 1)
                {
                    for (int i = 1; i < units.Count; i++)
                    {
                        UnitData data = AssetDatabase.LoadAssetAtPath<UnitData>(units[i].Item1);
                        Unit u = GetDefaultUnit(data);
                        u.Health = units[i].Item2;
                        playerParty.Add(u);
                    }
                }
            }
            else
            {
                UnitData playerUnitData = AssetDatabase.LoadAssetAtPath<UnitData>("Assets/Prefabs/Battle/Units/Player.asset");
                playerUnit = GetDefaultUnit(playerUnitData);
            }
        }

        /// <summary>
        /// Load abilities from equipped items. If no abilities are found, add a default punch ability.
        /// </summary>
        /// <returns>Array of current player abilities.</returns>
        public static Ability[] LoadPlayerAbilities()
        {
            Ability[] result = InventoryManager.Instance.GetEquippedAbilities();
            if (result.Length == 0)
            {
                result = new Ability[]
                {
                    new Ability
                    {
                        abilityID = "PlayerPunch",
                        name = "Punch",
                        targetting = AbilityTargetting.Single,
                        primaryEffects = new AbilityEffect[]
                        {
                            new AbilityEffect
                            {
                                effectType = AbilityEffectType.Damage,
                                effectValue = 3,
                                powerMultiplier = 0,
                                strenghtMultiplier = 1
                            }
                        }
                    }
                };
            }
            return result;
        }
        #endregion
    }
}
