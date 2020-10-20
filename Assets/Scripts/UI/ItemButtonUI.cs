using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUFG.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace TUFG.UI
{
    /// <summary>
    /// UI object displaying an inventory item button.
    /// </summary>
    public class ItemButtonUI : MonoBehaviour, UnityEngine.EventSystems.ISelectHandler
    {
        private Item item;
        private bool isEquipped;
        private InventoryContainer container;

        /// <summary>
        /// Initialize the button.
        /// </summary>
        /// <param name="item">Item this button represents.</param>
        /// <param name="isEquipped">Is the item currently equipped?</param>
        /// <param name="container">Parent container of the button.</param>
        public void InitButton(Item item, bool isEquipped, InventoryContainer container)
        {
            this.item = item;
            this.container = container;
            this.isEquipped = isEquipped;

            if (!isEquipped)
                transform.GetChild(0).gameObject.SetActive(false);

            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.name;

            switch (item.slot)
            {
                case ItemSlot.Hands:
                    transform.GetChild(1).GetComponent<Image>().sprite = UIManager.Instance.HandsSlotIcon;
                    break;
                case ItemSlot.Body:
                    transform.GetChild(1).GetComponent<Image>().sprite = UIManager.Instance.BodySlotIcon;
                    break;
                case ItemSlot.Legs:
                    transform.GetChild(1).GetComponent<Image>().sprite = UIManager.Instance.LegsSlotIcon;
                    break;
                case ItemSlot.Amulet:
                    transform.GetChild(1).GetComponent<Image>().sprite = UIManager.Instance.AmuletSlotIcon;
                    break;
                case ItemSlot.Ring:
                    transform.GetChild(1).GetComponent<Image>().sprite = UIManager.Instance.RingSlotIcon;
                    break;
            }

        }

        /// <summary>
        /// Event listener to the Select event.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
        {
            container.ScrollToObject(transform);
            container.SelectItem(item, isEquipped);
        }
    }
}
