using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TUFG.Inventory
{
    public class ShopManager : MonoBehaviour
    {
        #region Singleton pattern
        private static ShopManager _instance;
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
        /// <param name="shopSaves"></param>
        public void LoadShops(List<ShopSave> shopSaves)
        {
            foreach(ShopSave save in shopSaves)
            {
                Shop existingShop = Shops.Where(x => x.ShopId == save.shopId).FirstOrDefault();

                if (existingShop != null)
                    Shops.Remove(existingShop);

                Shops.Add(new Shop(save.shopId, save.itemPaths, save.margin));
            }
        }

    }
}
