using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TUFG.Inventory
{
    /// <summary>
    /// Class managing all the shops in the game.
    /// </summary>
    /// /// <remarks>Uses a singleton pattern.</remarks>
    public class ShopManager : MonoBehaviour
    {
        #region Singleton pattern
        private static ShopManager _instance;

        /// <summary>
        /// Current instance of the shop manager. Creates an Unity object.
        /// </summary>
        public static ShopManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<ShopManager>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("Shop Manager");
                        _instance = container.AddComponent<ShopManager>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        [SerializeField] private List<Shop> shops;
        public List<Shop> Shops { get => shops; set => shops = value; }

        /// <summary>
        /// Load shops from save, preserving existing shops.
        /// </summary>
        /// <param name="shopSaves">List of shop save instances representing all the shops in the game.</param>
        public void LoadShops(List<ShopSave> shopSaves)
        {
            foreach (ShopSave save in shopSaves)
            {
                Shop existingShop = Shops.Where(x => x.ShopId == save.shopId).FirstOrDefault();

                if (existingShop != null)
                    Shops.Remove(existingShop);

                Shops.Add(new Shop(save.shopId, save.itemPaths, save.margin));
            }
        }

        public List<ShopSave> InitializeShops()
        {
            Shops = new List<Shop>();

            Shops.Add(new Shop(
                "tutorial_shop", new List<string>()
                {
                    "Assets/Prefabs/Inventory/Items/Bronze sword of hell.asset"
                },
                1.2f));

            return Shops.Select(x => x.ToShopSave()).ToList();
        }

        /// <summary>
        /// Returns shop by ID.
        /// </summary>
        /// <param name="id">ID of the shop.</param>
        /// <returns></returns>
        public Shop GetShop(string id)
        {
            return Shops.Where(x => x.ShopId == id).FirstOrDefault();
        }

        /// <summary>
        /// Sell an item to the shop.
        /// </summary>
        /// <param name="item">Item to be sold.</param>
        /// <param name="shopId">ID of the shop you are selling to.</param>
        /// <returns>True if success, false if error.</returns>
        public bool SellItem(Item item, string shopId)
        {
            Shop shop = GetShop(shopId);
            if (shop == null)
            {
                Debug.LogError($"Couldn't find a shop with ID {shopId} when trying to sell item {item.name}.");
                return false;
            }

            if (InventoryManager.Instance.RemoveItem(item))
            {
                shop.Items.Add(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Buy an item to the shop.
        /// </summary>
        /// <param name="item">Item to be bought.</param>
        /// <param name="shopId">ID of the shop you are buying from.</param>
        /// <returns>True if success, false if error.</returns>
        public bool BuyItem(Item item, string shopId)
        {
            Shop shop = GetShop(shopId);
            if (shop == null)
            {
                Debug.LogError($"Couldn't find a shop with ID {shopId} when trying to buy item {item.name}.");
                return false;
            }

            if (shop.Items.Contains(item))
            {
                InventoryManager.Instance.PickUpItem(item);
                shop.Items.Remove(item);
                return true;
            } else
            {
                Debug.LogError($"Shop with ID {shopId} doesn't sell item {item.name}.");
                return false;
            }
        }

    }
}
