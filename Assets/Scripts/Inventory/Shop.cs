using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

namespace TUFG.Inventory
{
    /// <summary>
    /// Class representing a shop.
    /// </summary>
    [System.Serializable]
    public class Shop
    {
        [SerializeField] private string shopId;
        [SerializeField] private List<Item> items;
        [SerializeField] private float margin = 1f;

        /// <summary>
        /// ID of the shop in format shop_id.
        /// </summary>
        public string ShopId { get => shopId; set => shopId = value; }

        /// <summary>
        /// List of items that are being sold in the shop.
        /// </summary>
        public List<Item> Items { get => items; set => items = value; }

        /// <summary>
        /// Selling margin in the shop.
        /// </summary>
        public float Margin { get => margin; set => margin = value; }

        /// <summary>
        /// Create new shop.
        /// </summary>
        /// <param name="shopId">ID of the shop.</param>
        /// <param name="itemPaths">Paths to the items in the shop.</param>
        /// <param name="margin">Price margin of the shop.</param>
        public Shop(string shopId, List<string> itemPaths, float margin)
        {
            this.ShopId = shopId;
            this.Margin = margin;

            Items = new List<Item>();

            foreach (string path in itemPaths)
            {
                Item item = AssetDatabase.LoadAssetAtPath<Item>(path);
                if (item != null && !string.IsNullOrEmpty(item.id))
                    Items.Add(item);
            }
        }

        /// <summary>
        /// Get item paths from items.
        /// </summary>
        /// <returns>List of paths to the scriptable objects of the items in the shop.</returns>
        public List<string> GetItemPaths()
        {
            List<string> paths = new List<string>();
            foreach (Item item in Items)
            {
                paths.Add(AssetDatabase.GetAssetPath(item));
            }
            return paths;
        }

        /// <summary>
        /// Create a ShopSave instance from this Shop.
        /// </summary>
        /// <returns>ShopSave instance representing the shop.</returns>
        public ShopSave ToShopSave()
        {
            ShopSave result = new ShopSave();
            result.shopId = shopId;
            result.margin = margin;
            result.itemPaths = GetItemPaths();
            return result;
        }
    }
}
