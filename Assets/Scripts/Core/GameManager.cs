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
using System;
using UnityEngine.SceneManagement;

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

                        DontDestroyOnLoad(container);
                    }
                }

                return _instance;
            }
        }
        #endregion

        private string currentSlot = "Demo";

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
            if (!Directory.Exists(GetSaveDirectoryPath()))
                Directory.CreateDirectory(GetSaveDirectoryPath());

            SaveGame save = new SaveGame();

            // Inventory
            save.equippedItemPaths = InventoryManager.Instance.GetEquippedItemPaths();
            save.inventoryItemPaths = InventoryManager.Instance.GetInventoryItemPaths();
            save.gold = InventoryManager.Instance.Gold;
            save.shops = ShopManager.Instance.Shops.Select(x => x.ToShopSave()).ToList();
            save.playerParty = PartyManager.Instance.GetPlayerPartySave();
            save.currentLevelID = SceneManager.GetActiveScene().buildIndex;

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

                PartyManager.Instance.LoadPlayerParty(save.playerParty);

                SceneManager.LoadScene(save.currentLevelID);

                Debug.Log($"Game loaded from slot {slot}.");
            }
            else
            {
                PartyManager.Instance.LoadPlayerParty(null);
                Debug.LogError($"Save file on path {savePath} not found!");
            }
        }

        /// <summary>
        /// Create new game save at the default slot and load it.
        /// </summary>
        public void NewGame()
        {
            NewGame(currentSlot);
        }

        /// <summary>
        /// Create new game save and load it.
        /// </summary>
        /// <param name="slot">Slot to create the game at.</param>
        public void NewGame(string slot)
        {
            if (!Directory.Exists(GetSaveDirectoryPath()))
                Directory.CreateDirectory(GetSaveDirectoryPath());

            SaveGame save = new SaveGame();

            // Inventory
            save.equippedItemPaths = new List<string>();
            save.inventoryItemPaths = new List<string>();
            save.gold = 50;
            save.shops = ShopManager.Instance.InitializeShops();
            save.playerParty = new List<Tuple<string, int>>();
            save.currentLevelID = 2;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(GetSavePath(slot));
            bf.Serialize(file, save);
            file.Close();

            Debug.Log($"Game saved to slot {slot}.");

            LoadGame(slot);
        }

        /// <summary>
        /// Returns a path to the save file on a save slot.
        /// </summary>
        /// <param name="slot">Name of the slot and the save file.</param>
        /// <returns></returns>
        public string GetSavePath(string slot)
        {
            return $"{GetSaveDirectoryPath()}/{slot}.tufg";
        }

        /// <summary>
        /// Return a path to the save directory.
        /// </summary>
        /// <returns></returns>
        public string GetSaveDirectoryPath()
        {
            return $"{Application.persistentDataPath}/save";
        }

        /// <summary>
        /// Is there a save game on a slot?
        /// </summary>
        /// <param name="slot">Slot to be tested.</param>
        /// <returns>If there is a save game on the slot.</returns>
        public bool IsSaveOnSlot(string slot)
        {
            return File.Exists(GetSavePath(slot));
        }

        /// <summary>
        /// Is there a save game on the current slot?
        /// </summary>
        /// <returns>If there is a save game on the current slot.</returns>
        public bool IsSaveOnSlot()
        {
            return IsSaveOnSlot(currentSlot);
        }

        /// <summary>
        /// Is there any save file in the save directory?
        /// </summary>
        /// <returns>If any save file exists.</returns>
        public bool AnySaveExists()
        {
            return Directory.GetFiles(GetSaveDirectoryPath(), "*.tufg").Count() > 0;
        }
        #endregion
    }
}
