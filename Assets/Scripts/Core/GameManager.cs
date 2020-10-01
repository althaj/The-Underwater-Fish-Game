﻿using System.Collections;
using System.Collections.Generic;
using TUFG.Battle;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using TUFG.Battle.Abilities;
using TUFG.Inventory;

namespace TUFG.Core
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton pattern
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<GameManager>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("Game Manager");
                        _instance = container.AddComponent<GameManager>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        public static UnitData[] GetPlayerParty()
        {
            UnitData[] playerParty = new UnitData[3];

            playerParty[0] = GetPlayerUnitData();

            playerParty[1] = AssetDatabase.LoadAssetAtPath<UnitData>("Assets/Prefabs/Battle/Units/GoonUnit.asset");
            playerParty[2] = AssetDatabase.LoadAssetAtPath<UnitData>("Assets/Prefabs/Battle/Units/GoonUnit.asset");

            return playerParty;
        }

        public static UnitData GetPlayerUnitData()
        {
            UnitData playerUnitData = ScriptableObject.CreateInstance<UnitData>();
            playerUnitData.unitID = "Player";
            playerUnitData.name = "Player";
            playerUnitData.animator = AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/AnimatorControllers/Units/Player/PlayerAnimator.controller");
            playerUnitData.aiType = Battle.AI.UnitAIType.Player;
            playerUnitData.maxHealth = 100;
            playerUnitData.health = 100;
            playerUnitData.armor = 1;
            playerUnitData.speed = 3;
            playerUnitData.power = 12;
            playerUnitData.strength = 4;
            playerUnitData.abilities = new Ability[]
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
                },
                new Ability
                {
                    abilityID = "PlayerSlash",
                    name = "Slash",
                    targetting = AbilityTargetting.Adjescent,
                    primaryEffects = new AbilityEffect[]
                    {
                        new AbilityEffect
                        {
                            effectType = AbilityEffectType.Damage,
                            effectValue = 5,
                            powerMultiplier = 0,
                            strenghtMultiplier = 0.5f
                        }
                    },
                    secondaryEffects = new AbilityEffect[]
                    {
                        new AbilityEffect
                        {
                            effectType = AbilityEffectType.Damage,
                            effectValue = 3,
                            powerMultiplier = 0,
                            strenghtMultiplier = 0.5f
                        }
                    }
                },
                new Ability
                {
                    abilityID = "PlayerHeal",
                    name = "Heal",
                    targetting = AbilityTargetting.Self,
                    primaryEffects = new AbilityEffect[]
                    {
                        new AbilityEffect
                        {
                            effectType = AbilityEffectType.Heal,
                            effectValue = 10,
                            powerMultiplier = 1,
                            strenghtMultiplier = 0
                        }
                    }
                },
                new Ability
                {
                    abilityID = "PlayerPartyHeal",
                    name = "Heal party",
                    targetting = AbilityTargetting.Ally,
                    primaryEffects = new AbilityEffect[]
                    {
                        new AbilityEffect
                        {
                            effectType = AbilityEffectType.Heal,
                            effectValue = 5,
                            powerMultiplier = 1,
                            strenghtMultiplier = 0
                        }
                    }
                }
            };

            LoadPlayerItems();

            return playerUnitData;
        }

        public static void LoadPlayerItems()
        {
            Item sword = AssetDatabase.LoadAssetAtPath<Item>("Assets/Prefabs/Inventory/Items/Sword of destiny.asset");

            InventoryManager.Instance.GetItem(sword);
            InventoryManager.Instance.EquipItem(sword);
        }
    }
}
