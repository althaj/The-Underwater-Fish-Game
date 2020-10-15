using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUFG.Controls;
using TUFG.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace TUFG.Inventory
{
    public class DroppedItem : MonoBehaviour
    {
        [SerializeField] private Item item;
        public Item Item { get => item; set => item = value; }

        private GameObject canvasObject;
        private ControlsInput controlsInput;

        public void Awake()
        {
            canvasObject = GetComponentInChildren<Canvas>().gameObject;

            canvasObject.SetActive(false);

            controlsInput = new ControlsInput();
            controlsInput.World.PickUp.performed += PickUp;

            if (Item != null)
                Init(Item);
        }

        public void Init(Item item)
        {
            Item = item;

            canvasObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Item.name;
        }

        public void PickUp(CallbackContext ctx)
        {
            if (!UIManager.Instance.IsAnyWindowOpen())
            {
                InventoryManager.Instance.PickUpItem(Item);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerMovement>() != null)
            {
                canvasObject.SetActive(true);
                controlsInput.World.Enable();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerMovement>() != null)
            {
                canvasObject.SetActive(false);
                controlsInput.World.Disable();
            }
        }
    } 
}
