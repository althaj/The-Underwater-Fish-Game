using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TUFG.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace TUFG.UI
{
    public class InventoryContainer : MonoBehaviour
    {
        private GameObject inventoryPanel;

        [SerializeField]private Transform itemListContainer = null;
        [SerializeField]private Transform itemDetailsContainer = null;

        private GameObject buttonPrefab;
        private bool isOpen = false;
        public bool IsOpen { get => isOpen; private set => isOpen = value; }

        #region Unity methods
        void Start()
        {
            buttonPrefab = UIManager.Instance.InventoryButtonPrefab;

            inventoryPanel = transform.GetChild(0).gameObject;

            inventoryPanel.SetActive(false);
        }
        #endregion

        /// <summary>
        /// Display inventory.
        /// </summary>
        /// <param name="equippedItems">Equipped items.</param>
        /// <param name="inventoryItems">All player items.</param>
        public void ShowInventory(List<Item> equippedItems, List<Item> inventoryItems)
        {
            if (!IsOpen)
            {
                // TODO PLAY ANIMATION
                inventoryPanel.SetActive(true);
                IsOpen = true;
            }

            UIManager.Instance.ClearChildren(itemListContainer.gameObject);

            foreach (Item item in equippedItems.OrderBy(x => x.slot))
            {
                CreateButton(item, true);
            }

            foreach (Item item in inventoryItems.OrderBy(x => x.name))
            {
                CreateButton(item, false);
            }

            Vector2 sizeDelta = itemListContainer.GetComponent<RectTransform>().sizeDelta;
            VerticalLayoutGroup layout = itemListContainer.GetComponent<VerticalLayoutGroup>();
            sizeDelta.y = (equippedItems.Count + inventoryItems.Count) * buttonPrefab.GetComponent<RectTransform>().sizeDelta.y + (equippedItems.Count + inventoryItems.Count - 1) * layout.spacing + layout.padding.top + layout.padding.bottom;
            itemListContainer.GetComponent<RectTransform>().sizeDelta = sizeDelta;

            if (itemListContainer.childCount != 0)
                EventSystem.current.SetSelectedGameObject(itemListContainer.GetChild(0).gameObject);
        }

        /// <summary>
        /// Create an item button in the items scroll view.
        /// </summary>
        /// <param name="item">Item to create button for.</param>
        /// <param name="isEquipped">Is the item equipped?</param>
        private void CreateButton(Item item, bool isEquipped)
        {
            GameObject buttonInstance = Instantiate<GameObject>(buttonPrefab);
            buttonInstance.transform.SetParent(itemListContainer);
            buttonInstance.GetComponent<ItemButtonUI>().InitButton(item, isEquipped, this);
        }

        /// <summary>
        /// Hide player inventory panel.
        /// </summary>
        public void HideInventory()
        {
            IsOpen = false;
            inventoryPanel.SetActive(false);
        }

        /// <summary>
        /// Toggle inventory visibility on or off.
        /// </summary>
        /// <param name="equippedItems">Equipped items.</param>
        /// <param name="inventoryItems">All player items.</param>
        public void ToggleInventory(List<Item> equippedItems, List<Item> inventoryItems)
        {
            if (IsOpen)
                HideInventory();
            else
                ShowInventory(equippedItems, inventoryItems);
        }

        /// <summary>
        /// Select an item in the inventory and display its details and buttons.
        /// </summary>
        /// <param name="item">Item to select.</param>
        public void SelectItem(Item item, bool isEquipped)
        {
            itemDetailsContainer.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;
            itemDetailsContainer.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.SlotText;
            itemDetailsContainer.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.description;

            TextMeshProUGUI buttonText = itemDetailsContainer.GetChild(4).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = isEquipped ? "Unequip" : "Equip";
        }

        /// <summary>
        /// Scroll to currently selected item
        /// </summary>
        /// <param name="obj">Item transform to scroll to.</param>
        public void ScrollToObject(Transform obj)
        {
            Canvas.ForceUpdateCanvases();
            ScrollRect scroll = itemListContainer.parent.parent.GetComponent<ScrollRect>();

            Vector2 anchored = itemListContainer.GetComponent<RectTransform>().anchoredPosition;

            anchored.y = scroll.transform.InverseTransformPoint(itemListContainer.position).y - scroll.transform.InverseTransformPoint(obj.position).y;

            itemListContainer.GetComponent<RectTransform>().anchoredPosition = anchored;
        }
    }
}
