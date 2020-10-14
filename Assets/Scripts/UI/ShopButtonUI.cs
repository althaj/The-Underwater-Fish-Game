using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUFG.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace TUFG.UI
{
    public class ShopButtonUI : MonoBehaviour, UnityEngine.EventSystems.ISelectHandler
    {
        private Item item;
        private ShopContainer container;
        private bool isSelling;

        public void InitButton(Item item, int price, ShopContainer container, bool isSelling)
        {
            this.item = item;
            this.container = container;
            this.isSelling = isSelling;

            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = price.ToString();
            transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.name;

            switch (item.slot)
            {
                case ItemSlot.Hands:
                    transform.GetChild(2).GetComponent<Image>().sprite = UIManager.Instance.HandsSlotIcon;
                    break;
                case ItemSlot.Body:
                    transform.GetChild(2).GetComponent<Image>().sprite = UIManager.Instance.BodySlotIcon;
                    break;
                case ItemSlot.Legs:
                    transform.GetChild(2).GetComponent<Image>().sprite = UIManager.Instance.LegsSlotIcon;
                    break;
                case ItemSlot.Amulet:
                    transform.GetChild(2).GetComponent<Image>().sprite = UIManager.Instance.AmuletSlotIcon;
                    break;
                case ItemSlot.Ring:
                    transform.GetChild(2).GetComponent<Image>().sprite = UIManager.Instance.RingSlotIcon;
                    break;
            }

        }
        public void OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
        {
            container.ScrollToObject(transform);
            container.SelectItem(item, isSelling);
        }
    } 
}
