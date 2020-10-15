using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUFG.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TUFG.UI
{
    public class ShopContainer : MonoBehaviour
    {
        private GameObject shopPanel;
        private Item currentItem;
        private bool currentItemIsSelling;
        private string shopId;

        [SerializeField] private Transform buyItemsContainer = null;
        [SerializeField] private Transform sellItemsContainer = null;
        [SerializeField] private Transform itemDetailsContainer = null;
        [SerializeField] private TextMeshProUGUI goldText = null;

        private GameObject buttonPrefab;
        private bool isOpen = false;
        public bool IsOpen { get => isOpen; private set => isOpen = value; }

        #region Unity methods
        void Start()
        {
            buttonPrefab = UIManager.Instance.ShopButtonPrefab;

            shopPanel = transform.GetChild(0).gameObject;

            shopPanel.SetActive(false);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Display shop.
        /// </summary>
        public void ShowShop(Shop shop)
        {
            shopId = shop.ShopId;

            FindObjectOfType<PlayerMovement>().DisableInput();

            List<Item> buyItems = shop.Items;
            List<Item> sellItems = InventoryManager.Instance.InventoryItems;

            List<Button> itemButtons = new List<Button>();

            if (!IsOpen)
            {
                // TODO PLAY ANIMATION
                shopPanel.SetActive(true);
                IsOpen = true;
            }

            UIManager.Instance.ClearChildren(buyItemsContainer.gameObject);

            for (int i = 0; i < buyItems.Count; i++)
            {
                GameObject button = CreateButton(buyItems[i], Mathf.RoundToInt((float)buyItems[i].price * shop.Margin), false);

                itemButtons.Add(button.GetComponent<Button>());

                if (i == 0)
                    EventSystem.current.SetSelectedGameObject(button);
            }

            UIManager.Instance.ClearChildren(sellItemsContainer.gameObject);

            goldText.text = InventoryManager.Instance.Gold.ToString();

            for (int i = 0; i < sellItems.Count; i++)
            {
                GameObject button = CreateButton(sellItems[i], Mathf.RoundToInt((float)sellItems[i].price * shop.Margin), true);

                itemButtons.Add(button.GetComponent<Button>());

                if (i == 0 && buyItems.Count == 0)
                    EventSystem.current.SetSelectedGameObject(button);
            }

            Vector2 sizeDelta = buyItemsContainer.GetComponent<RectTransform>().sizeDelta;
            VerticalLayoutGroup layout = buyItemsContainer.GetComponent<VerticalLayoutGroup>();
            sizeDelta.y = buyItems.Count * buttonPrefab.GetComponent<RectTransform>().sizeDelta.y + (buyItems.Count - 1) * layout.spacing + layout.padding.top + layout.padding.bottom;
            buyItemsContainer.GetComponent<RectTransform>().sizeDelta = sizeDelta;

            sizeDelta = sellItemsContainer.GetComponent<RectTransform>().sizeDelta;
            layout = sellItemsContainer.GetComponent<VerticalLayoutGroup>();
            sizeDelta.y = sellItems.Count * buttonPrefab.GetComponent<RectTransform>().sizeDelta.y + (sellItems.Count - 1) * layout.spacing + layout.padding.top + layout.padding.bottom;
            sellItemsContainer.GetComponent<RectTransform>().sizeDelta = sizeDelta;

            // Build button navigation
            for (int i = 0; i < itemButtons.Count; i++)
            {
                Button button = itemButtons[i];

                Navigation nav = button.navigation;

                if (i > 0)
                    nav.selectOnUp = itemButtons[i - 1];

                if (i < itemButtons.Count - 1)
                    nav.selectOnDown = itemButtons[i + 1];

                nav.selectOnRight = itemDetailsContainer.GetComponentInChildren<Button>();

                button.navigation = nav;
            }
        }

        /// <summary>
        /// Hide shop panel.
        /// </summary>
        public void HideShop()
        {
            FindObjectOfType<PlayerMovement>().EnableInput();

            IsOpen = false;
            shopPanel.SetActive(false);
        }

        /// <summary>
        /// Toggle shop visibility on or off.
        /// </summary>
        public void ToggleShop(Shop shop)
        {
            if (IsOpen)
                HideShop();
            else
                ShowShop(shop);
        }

        /// <summary>
        /// Scroll to currently selected item
        /// </summary>
        /// <param name="obj">Item transform to scroll to.</param>
        public void ScrollToObject(Transform obj)
        {
            Canvas.ForceUpdateCanvases();
            ScrollRect scroll = buyItemsContainer.parent.parent.GetComponent<ScrollRect>();

            Vector2 anchored = buyItemsContainer.GetComponent<RectTransform>().anchoredPosition;

            anchored.y = scroll.transform.InverseTransformPoint(buyItemsContainer.position).y - scroll.transform.InverseTransformPoint(obj.position).y;

            buyItemsContainer.GetComponent<RectTransform>().anchoredPosition = anchored;
        }

        /// <summary>
        /// Select an item in the shop and display its details and buttons.
        /// </summary>
        /// <param name="item">Item to select.</param>
        public void SelectItem(Item item, bool isSelling)
        {
            itemDetailsContainer.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;
            itemDetailsContainer.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.SlotText;
            itemDetailsContainer.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.description;

            TextMeshProUGUI buttonText = itemDetailsContainer.GetChild(4).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = isSelling ? "Sell item" : "Buy item";

            currentItem = item;
            currentItemIsSelling = isSelling;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Create an item button in the items scroll view.
        /// </summary>
        /// <param name="item">Item to create button for.</param>
        private GameObject CreateButton(Item item, int price, bool isSelling)
        {
            GameObject buttonInstance = Instantiate<GameObject>(buttonPrefab);
            if(isSelling)
                buttonInstance.transform.SetParent(sellItemsContainer);
            else
                buttonInstance.transform.SetParent(buyItemsContainer);
            buttonInstance.GetComponent<ShopButtonUI>().InitButton(item, price, this, isSelling);

            return buttonInstance;
        }

        /// <summary>
        /// Buy / sell currently selected item.
        /// </summary>
        public void BuySellItem()
        {
            if (currentItemIsSelling)
                ShopManager.Instance.SellItem(currentItem, shopId);
            else
                ShopManager.Instance.BuyItem(currentItem, shopId);

            ShowShop(ShopManager.Instance.GetShop(shopId));
        }
        #endregion
    }
}
