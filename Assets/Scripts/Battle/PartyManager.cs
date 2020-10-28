using System.Collections;
using System.Collections.Generic;
using TUFG.Core;
using UnityEditor;
using UnityEngine;

namespace TUFG.Battle
{
    /// <summary>
    /// Class responsible for managing player perty.
    /// </summary>
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

                        _instance.playerParty.Add(_instance.GetDefaultUnit(AssetDatabase.LoadAssetAtPath<UnitData>("Assets/Prefabs/Battle/Units/GoonUnit.asset")));
                        _instance.playerParty.Add(_instance.GetDefaultUnit(AssetDatabase.LoadAssetAtPath<UnitData>("Assets/Prefabs/Battle/Units/GoonUnit.asset")));
                    }
                }

                return _instance;
            }
        }
        #endregion

        public static int MAX_PARTY_SIZE = 4;
        List<Unit> playerParty;

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
                UnitData playerUnitData = GameManager.Instance.GetPlayerUnitData();
                party.Add(GetDefaultUnit(playerUnitData));
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
        #endregion
    } 
}
