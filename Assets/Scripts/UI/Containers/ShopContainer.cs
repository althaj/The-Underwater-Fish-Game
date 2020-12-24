using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TUFG.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Editor;
using UnityEngine.UI;

namespace TUFG.UI
{
    /// <summary>
    /// UI container displaying a shop.
    /// </summary>
    public class ShopContainer : ContainerBehaviour
    {
        private GameObject shopPanel;
        private Item currentItem;
        private bool currentItemIsSelling;
        private Shop shop;

        [SerializeField] private Transform buyItemsContainer = null;
        [SerializeField] private Transform sellItemsContainer = null;
        [SerializeField] private Transform itemDetailsContainer = null;
        [SerializeField] private TextMeshProUGUI goldText = null;

        private GameObject buttonPrefab;

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
        /// Set the current shop. You need to call Open() to display the shop.
        /// </summary>
        /// <param name="shop">Shop to be displayed.</param>
        public void SetShop(Shop shop)
        {
            this.shop = shop;
        }

        /// <summary>
        /// Open the shop container.
        /// </summary>
        public override void Open()
        {
            FindObjectOfType<PlayerMovement>().DisableInput();

            List<Item> buyItems = shop.Items;
            List<Item> sellItems = InventoryManager.Instance.InventoryItems;

            if (!IsOpen)
            {
                // TODO PLAY ANIMATION
                shopPanel.SetActive(true);
                IsOpen = true;
            }

            goldText.text = InventoryManager.Instance.Gold.ToString();

            Button[] buyButtons = BuildButtons(buyItems, false);
            Button[] sellButtons = BuildButtons(sellItems, true);
            Button[] buttons = buyButtons.Concat(sellButtons).ToArray();
            UIManager.BuildListButtonNavigation(buttons, itemDetailsContainer.GetComponentInChildren<Button>());

            // If there are no buttons, disable the buy / sell button and select the close button.
            if(buttons.Length == 0)
            {
                itemDetailsContainer.GetChild(4).GetChild(0).GetComponent<Button>().interactable = false;
                EventSystem.current.SetSelectedGameObject(itemDetailsContainer.GetChild(4).GetChild(1).gameObject);
                SetItemDescription(new Item());
            } else
            {
                itemDetailsContainer.GetChild(4).GetChild(0).GetComponent<Button>().interactable = true;
            }
        }

        /// <summary>
        /// Hide shop panel.
        /// </summary>
        public override void Close()
        {
            FindObjectOfType<PlayerMovement>().EnableInput();

            IsOpen = false;
            shopPanel.SetActive(false);
        }

        /// <summary>
        /// The close button has been pressed.
        /// </summary>
        public void CloseButtonPressed()
        {
            UIManager.Instance.CloseShop();
        }

        /// <summary>
        /// Scroll to currently selected item
        /// </summary>
        /// <param name="obj">Item transform to scroll to.</param>
        public void ScrollToObject(Transform obj)
        {
            Transform container = obj.parent;
            Canvas.ForceUpdateCanvases();
            ScrollRect scroll = container.parent.parent.GetComponent<ScrollRect>();

            Vector2 anchored = container.GetComponent<RectTransform>().anchoredPosition;

            anchored.y = scroll.transform.InverseTransformPoint(container.position).y - scroll.transform.InverseTransformPoint(obj.position).y - container.GetComponent<VerticalLayoutGroup>().padding.top;

            container.GetComponent<RectTransform>().anchoredPosition = anchored;
        }

        /// <summary>
        /// Select an item in the shop and display its details and buttons.
        /// </summary>
        /// <param name="item">Item to select.</param>
        /// <param name="isSelling">Is player selling the item?</param>
        public void SelectItem(Item item, bool isSelling)
        {
            SetItemDescription(item);

            TextMeshProUGUI sellButton = itemDetailsContainer.GetChild(4).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            sellButton.text = isSelling ? "Sell item" : "Buy item";

            if (!isSelling && !CanBuyItem(item))
                sellButton.GetComponentInParent<Button>().enabled = false;
            else
                sellButton.GetComponentInParent<Button>().enabled = true;

            currentItem = item;
            currentItemIsSelling = isSelling;
        }

        /// <summary>
        /// Buy / sell currently selected item.
        /// </summary>
        public void BuySellItem()
        {
            if (currentItemIsSelling)
            {
                if (ShopManager.Instance.SellItem(currentItem, shop.ShopId))
                {
                    InventoryManager.Instance.Gold += GetItemPrice(currentItem);
                }
            }
            else
            {
                if(ShopManager.Instance.BuyItem(currentItem, shop.ShopId)){
                    InventoryManager.Instance.Gold -= GetItemPrice(currentItem);
                }
            }
            currentItem = null;

            UIManager.Instance.OpenShop(shop);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Create an item button in the items scroll view.
        /// </summary>
        /// <param name="item">Item to create button for.</param>
        /// <param name="price">Price of the item including the shop margin.</param>
        /// <param name="isSelling">Is player selling the item?</param>
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
        /// Build the shop buttons.
        /// </summary>
        /// <param name="items">List of items to build buttons for.</param>
        /// <param name="isSelling">Is player selling the items?</param>
        private Button[] BuildButtons(List<Item> items, bool isSelling)
        {
            Button[] result = new Button[items.Count];

            Transform container;
            if (isSelling)
                container = sellItemsContainer;
            else
                container = buyItemsContainer;

            UIManager.Instance.ClearChildren(container.gameObject);

            for (int i = 0; i < items.Count; i++)
            {
                GameObject button = CreateButton(items[i], GetItemPrice(items[i]), isSelling);
                result[i] = button.GetComponent<Button>();
            }

            Vector2 sizeDelta = container.GetComponent<RectTransform>().sizeDelta;
            VerticalLayoutGroup layout = container.GetComponent<VerticalLayoutGroup>();
            sizeDelta.y = items.Count * buttonPrefab.GetComponent<RectTransform>().sizeDelta.y + (items.Count - 1) * layout.spacing + layout.padding.top + layout.padding.bottom;
            container.GetComponent<RectTransform>().sizeDelta = sizeDelta;

            if (result.Length > 0)
                ScrollToObject(result[0].transform);

            return result;
        }

        /// <summary>
        /// Does player have enough money to buy this item?
        /// </summary>
        /// <param name="item">Item to check the price.</param>
        /// <returns>True if player has enough moneoy to buy the item.</returns>
        private bool CanBuyItem(Item item)
        {
            return GetItemPrice(item) <= InventoryManager.Instance.Gold;
        }

        /// <summary>
        /// Get item price with shop margin.
        /// </summary>
        /// <param name="item">Item to get the price for.</param>
        /// <returns>Price of the item including shop margin.</returns>
        private int GetItemPrice(Item item)
        {
            return Mathf.RoundToInt((float)item.price * shop.Margin);
        }

        /// <summary>
        /// Set the description of the item to display in the shop.
        /// </summary>
        /// <param name="item">Item to display.</param>
        private void SetItemDescription(Item item)
        {
            itemDetailsContainer.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;
            itemDetailsContainer.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.SlotText;

            if (item.slot == ItemSlot.None)
                itemDetailsContainer.GetChild(1).gameObject.SetActive(false);
            else
                itemDetailsContainer.GetChild(1).gameObject.SetActive(true);

            itemDetailsContainer.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.description;
        }
        #endregion
    }
}
