﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TUFG.Battle;
using TUFG.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TUFG.UI
{
    /// <summary>
    /// Container with the inventory window.
    /// </summary>
    public class InventoryContainer : ContainerBehaviour
    {
        private GameObject inventoryPanel;
        private Item currentItem;
        private bool currentItemIsEquipped;

        [SerializeField] private Transform itemListContainer = null;
        [SerializeField] private Transform itemDetailsContainer = null;
        [SerializeField] private TextMeshProUGUI goldText = null;

        private GameObject buttonPrefab;

        #region Unity methods
        void Start()
        {
            buttonPrefab = UIManager.Instance.InventoryButtonPrefab;

            inventoryPanel = transform.GetChild(0).gameObject;

            inventoryPanel.SetActive(false);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Display the inventory.
        /// </summary>
        public override void Open()
        {
            FindObjectOfType<PlayerMovement>().DisableInput();

            List<Item> equippedItems = InventoryManager.Instance.EquippedItems.OrderBy(x => x.slot).ToList();
            List<Item> inventoryItems = InventoryManager.Instance.InventoryItems.OrderBy(x => x.name).OrderBy(x => x.slot).ToList();

            List<Button> itemButtons = new List<Button>();


            if (!IsOpen)
            {
                // TODO PLAY ANIMATION
                inventoryPanel.SetActive(true);
                IsOpen = true;
            }

            UIManager.Instance.ClearChildren(itemListContainer.gameObject);

            goldText.text = InventoryManager.Instance.Gold.ToString();

            for (int i = 0; i < equippedItems.Count; i++)
            {
                GameObject button = CreateButton(equippedItems[i], true);

                itemButtons.Add(button.GetComponent<Button>());
            }

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                GameObject button = CreateButton(inventoryItems[i], false);

                itemButtons.Add(button.GetComponent<Button>());
            }

            Vector2 sizeDelta = itemListContainer.GetComponent<RectTransform>().sizeDelta;
            VerticalLayoutGroup layout = itemListContainer.GetComponent<VerticalLayoutGroup>();
            sizeDelta.y = itemButtons.Count * buttonPrefab.GetComponent<RectTransform>().sizeDelta.y + (itemButtons.Count - 1) * layout.spacing + layout.padding.top + layout.padding.bottom;
            itemListContainer.GetComponent<RectTransform>().sizeDelta = sizeDelta;

            if (itemButtons.Count > 0)
                ScrollToObject(itemButtons[0].transform);

            Button rightButton;
            if (BattleManager.Instance.IsBattleInProgress())
            {
                rightButton = itemDetailsContainer.GetChild(4).GetChild(1).GetComponent<Button>();
                itemDetailsContainer.GetChild(4).GetChild(0).GetComponent<Button>().interactable = false;
            } else
            {
                rightButton = itemDetailsContainer.GetComponentInChildren<Button>();
                itemDetailsContainer.GetChild(4).GetChild(0).GetComponent<Button>().interactable = true;
            }

            UIManager.BuildListButtonNavigation(itemButtons.ToArray(), rightButton);
        }

        /// <summary>
        /// Hide player inventory panel.
        /// </summary>
        public override void Close()
        {
            IsOpen = false;
            inventoryPanel.SetActive(false);
        }

        /// <summary>
        /// Close button was pressed.
        /// </summary>
        public void CloseButtonPressed()
        {
            UIManager.Instance.CloseInventory();
        }

        /// <summary>
        /// Select an item in the inventory and display its details and buttons.
        /// </summary>
        /// <param name="item">Item to select.</param>
        /// <param name="isEquipped">Is the item currently eqipped?</param>
        public void SelectItem(Item item, bool isEquipped)
        {
            itemDetailsContainer.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;
            itemDetailsContainer.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.SlotText;
            itemDetailsContainer.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.description;

            TextMeshProUGUI buttonText = itemDetailsContainer.GetChild(4).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = isEquipped ? "Unequip" : "Equip";

            currentItem = item;
            currentItemIsEquipped = isEquipped;
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

            anchored.y = scroll.transform.InverseTransformPoint(itemListContainer.position).y - scroll.transform.InverseTransformPoint(obj.position).y - itemListContainer.GetComponent<VerticalLayoutGroup>().padding.top;

            itemListContainer.GetComponent<RectTransform>().anchoredPosition = anchored;
        }

        /// <summary>
        /// Equip / unequip currently selected item.
        /// </summary>
        public void ToggleCurrentItem()
        {
            if (currentItemIsEquipped)
                InventoryManager.Instance.UnequipItem(currentItem);
            else
                InventoryManager.Instance.EquipItem(currentItem);

            currentItem = null;
            UIManager.Instance.OpenInventory();
        }

        /// <summary>
        /// Drop currently selected item.
        /// </summary>
        public void DropCurrentItem()
        {
            InventoryManager.Instance.DropItem(currentItem);
            UIManager.Instance.OpenInventory();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Create an item button in the items scroll view.
        /// </summary>
        /// <param name="item">Item to create button for.</param>
        /// <param name="isEquipped">Is the item equipped?</param>
        private GameObject CreateButton(Item item, bool isEquipped)
        {
            GameObject buttonInstance = Instantiate<GameObject>(buttonPrefab);
            buttonInstance.transform.SetParent(itemListContainer);
            buttonInstance.GetComponent<ItemButtonUI>().InitButton(item, isEquipped, this);

            return buttonInstance;
        }
        #endregion
    }
}
