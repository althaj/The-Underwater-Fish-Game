using System.Collections;
using System.Collections.Generic;
using TUFG.Battle;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using TUFG.Battle.Abilities;
using TUFG.Inventory;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

namespace TUFG.Core
{
    /// <summary>
    /// Class handling the core game mechanics, such as getting player information and saving / loading the game.
    /// </summary>
    /// <remarks>Uses a singleton pattern.</remarks>
    public class GameManager : MonoBehaviour
    {
        #region Singleton pattern
        private static GameManager _instance;

        /// <summary>
        /// Current instance of the game manager. Creates an Unity object.
        /// </summary>
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

        private string currentSlot = "Demo";

        /// <summary>
        /// Returns the current player party data.
        /// </summary>
        /// <remarks>This is not properly implemented yet.</remarks>
        /// <returns>Array of player party units, including the player.</returns>
        public static UnitData[] GetPlayerParty()
        {
            UnitData[] playerParty = new UnitData[3];

            playerParty[0] = Instance.GetPlayerUnitData();

            playerParty[1] = AssetDatabase.LoadAssetAtPath<UnitData>("Assets/Prefabs/Battle/Units/GoonUnit.asset");
            playerParty[2] = AssetDatabase.LoadAssetAtPath<UnitData>("Assets/Prefabs/Battle/Units/GoonUnit.asset");

            return playerParty;
        }

        /// <summary>
        /// Returns the current player unit data.
        /// </summary>
        /// <remarks>This is not properly implemented yet.</remarks>
        /// <returns>Current player unit data.</returns>
        public UnitData GetPlayerUnitData()
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

            LoadPlayerItems();

            playerUnitData.abilities = LoadPlayerAbilities();

            return playerUnitData;
        }

        /// <summary>
        /// Load player items.
        /// </summary>
        /// <remarks>This is not properly implemented yet.</remarks>
        public static void LoadPlayerItems()
        {
            Item sword = AssetDatabase.LoadAssetAtPath<Item>("Assets/Prefabs/Inventory/Items/Sword of destiny.asset");
            Item clothes = AssetDatabase.LoadAssetAtPath<Item>("Assets/Prefabs/Inventory/Items/Beggar's clothes.asset");

            InventoryManager.Instance.PickUpItem(sword);
            InventoryManager.Instance.EquipItem(sword);
            InventoryManager.Instance.PickUpItem(clothes);
            InventoryManager.Instance.EquipItem(clothes);
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

        #region Save game
        /// <summary>
        /// Save game to the current slot.
        /// </summary>
        public void SaveGame()
        {
            SaveGame(currentSlot);
        }

        /// <summary>
        /// Save game to a slot.
        /// </summary>
        /// <param name="slot">Name of the slot and the save file.</param>
        public void SaveGame(string slot)
        {
            if (!Directory.Exists(GetSaveDirectory()))
                Directory.CreateDirectory(GetSaveDirectory());

            SaveGame save = new SaveGame();

            // Inventory
            save.equippedItemPaths = InventoryManager.Instance.GetEquippedItemPaths();
            save.inventoryItemPaths = InventoryManager.Instance.GetInventoryItemPaths();
            save.gold = InventoryManager.Instance.Gold;
            save.shops = ShopManager.Instance.Shops.Select(x => x.ToShopSave()).ToList();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(GetSavePath(slot));
            bf.Serialize(file, save);
            file.Close();

            Debug.Log($"Game saved to slot {slot}.");
        }

        /// <summary>
        /// Load game from the current slot.
        /// </summary>
        public void LoadGame()
        {
            LoadGame(currentSlot);
        }

        /// <summary>
        /// Load game at a slot.
        /// </summary>
        /// <param name="slot">Name of the slot and the save file.</param>
        public void LoadGame(string slot)
        {
            string savePath = GetSavePath(slot);
            if (File.Exists(savePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(savePath, FileMode.Open);
                SaveGame save = (SaveGame)bf.Deserialize(file);
                file.Close();

                InventoryManager.Instance.LoadItemsFromPaths(save.equippedItemPaths, save.inventoryItemPaths);
                InventoryManager.Instance.Gold = save.gold;
                if(save.shops != null)
                    ShopManager.Instance.LoadShops(save.shops);

                Debug.Log($"Game loaded from slot {slot}.");
            }
            else
            {
                Debug.LogError($"Save file on path {savePath} not found!");
            }
        }

        /// <summary>
        /// Returns a path to the save file on a save slot.
        /// </summary>
        /// <param name="slot">Name of the slot and the save file.</param>
        /// <returns></returns>
        public string GetSavePath(string slot)
        {
            return $"{GetSaveDirectory()}/{slot}.tufg";
        }

        /// <summary>
        /// Return a path to the save directory.
        /// </summary>
        /// <returns></returns>
        public string GetSaveDirectory()
        {
            return $"{Application.persistentDataPath}/save";
        }
        #endregion
    }
}
