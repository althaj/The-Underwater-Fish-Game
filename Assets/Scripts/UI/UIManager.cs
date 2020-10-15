using System.Collections;
using System.Collections.Generic;
using TUFG.Controls;
using TUFG.Core;
using TUFG.Dialogue;
using TUFG.Inventory;
using TUFG.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace TUFG.UI
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton pattern
        private static UIManager _instance;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<UIManager>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("UI Manager");
                        _instance = container.AddComponent<UIManager>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        #region Fields and properties
        [SerializeField] private GameObject buttonPrefab = null;
        [SerializeField] private GameObject inventoryButtonPrefab = null;
        [SerializeField] private GameObject shopButtonPrefab = null;
        [SerializeField] private Sprite handsSlotIcon = null;
        [SerializeField] private Sprite bodySlotIcon = null;
        [SerializeField] private Sprite legsSlotIcon = null;
        [SerializeField] private Sprite amuletSlotIcon = null;
        [SerializeField] private Sprite ringSlotIcon = null;
        public GameObject ButtonPrefab { get => buttonPrefab; set => buttonPrefab = value; }
        public GameObject InventoryButtonPrefab { get => inventoryButtonPrefab; set => inventoryButtonPrefab = value; }
        public GameObject ShopButtonPrefab { get => shopButtonPrefab; set => shopButtonPrefab = value; }
        public Sprite HandsSlotIcon { get => handsSlotIcon; set => handsSlotIcon = value; }
        public Sprite BodySlotIcon { get => bodySlotIcon; set => bodySlotIcon = value; }
        public Sprite LegsSlotIcon { get => legsSlotIcon; set => legsSlotIcon = value; }
        public Sprite AmuletSlotIcon { get => amuletSlotIcon; set => amuletSlotIcon = value; }
        public Sprite RingSlotIcon { get => ringSlotIcon; set => ringSlotIcon = value; }

        private ControlsInput controlsInput;
        #endregion

        #region Containers
        internal static DialogueContainer dialogueContainer;
        internal static DialogueContainer DialogueContainer
        {
            get
            {
                if (dialogueContainer == null)
                    dialogueContainer = Instance.GetComponentInChildren<DialogueContainer>();

                if (dialogueContainer == null)
                    Debug.LogError("Cannot find the dialogue container!!");
                return dialogueContainer;
            }
        }

        internal static BattleContainer battleContainer;
        internal static BattleContainer BattleContainer
        {
            get
            {
                if (battleContainer == null)
                    battleContainer = Instance.GetComponentInChildren<BattleContainer>();

                if (battleContainer == null)
                    Debug.LogError("Cannot find the battle container!!");
                return battleContainer;
            }
        }

        internal static InventoryContainer inventoryContainer;
        internal static InventoryContainer InventoryContainer
        {
            get
            {
                if (inventoryContainer == null)
                    inventoryContainer = Instance.GetComponentInChildren<InventoryContainer>();

                if (inventoryContainer == null)
                    Debug.LogError("Cannot find the inventory container!!");
                return inventoryContainer;
            }
        }

        internal static ShopContainer shopContainer;
        internal static ShopContainer ShopContainer
        {
            get
            {
                if (shopContainer == null)
                    shopContainer = Instance.GetComponentInChildren<ShopContainer>();

                if (shopContainer == null)
                    Debug.LogError("Cannot find the shop container!!");
                return shopContainer;
            }
        }
        #endregion

        #region Unity functions
        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Instance.controlsInput = new ControlsInput();
            Instance.controlsInput.UI.OpenInventory.performed += _instance.InventoryButtonPressed;

            GameManager.Instance.LoadGame();
        }
        private void OnEnable()
        {
            Instance.controlsInput.UI.Enable();
        }

        private void OnDisable()
        {
            Instance.controlsInput.UI.Disable();
        }

        private void OnApplicationQuit()
        {
            GameManager.Instance.SaveGame();
        }
        #endregion

        #region Show / Hide methods
        public void ShowMessage(string authorName, string message, Sprite authorAvatar, GenericButton[] buttons, DialogueAvatarPosition avatarPosition = DialogueAvatarPosition.Left)
        {
            DialogueContainer.ShowMessage(authorName, message, authorAvatar, buttons, avatarPosition);
        }

        public void HideMessage()
        {
            DialogueContainer.HideMessage();
        }

        public void ShowBattleActions(GenericButton[] buttons, string text)
        {
            BattleContainer.ShowBattleActions(buttons, text);
        }

        public void HideActions()
        {
            BattleContainer.HideActions();
        }

        public void ShowInventory()
        {
            InventoryContainer.ShowInventory();
        }

        public void InventoryButtonPressed(CallbackContext ctx)
        {
            if(!IsAnyWindowOpen() || InventoryContainer.IsOpen)
                InventoryContainer.ToggleInventory();
        }

        public void HideInventory()
        {
            InventoryContainer.HideInventory();
        }

        public void ShowShop(Shop shop)
        {
            DialogueManager.Instance.EndConversation();
            ShopContainer.ShowShop(shop);
        }

        public void HideShop()
        {
            ShopContainer.HideShop();
        }
        #endregion

        public void BuildButtons(GenericButton[] buttons, GameObject buttonPanel, string noButtonText)
        {
            if (buttons == null || buttons.Length == 0)
            {
                // Create "Next" button
                GenericButton continueButton = new GenericButton();
                continueButton.text = noButtonText;
                continueButton.dialogueFunction = DialogueButtonFunction.GoToNextNode;

                GameObject buttonInstance = Instantiate<GameObject>(buttonPrefab);
                buttonInstance.transform.SetParent(buttonPanel.transform);
                buttonInstance.GetComponent<ButtonUI>().InitButton(continueButton);

                EventSystem.current.SetSelectedGameObject(buttonInstance);
            }
            else
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    GameObject buttonInstance = Instantiate<GameObject>(buttonPrefab);
                    buttonInstance.transform.SetParent(buttonPanel.transform);
                    buttonInstance.GetComponent<ButtonUI>().InitButton(buttons[i]);

                    if (i == 0)
                        EventSystem.current.SetSelectedGameObject(buttonInstance);
                }

            }
        }

        /// <summary>
        /// Build navigation of buttons in a panel and select the first button.
        /// </summary>
        /// <param name="parentContainer">Panel containing all the buttons in the list.</param>
        /// <param name="rightButton">Button to select with a right arrow.</param>
        public static void BuildListButtonNavigation(Button[] listButtons, Button rightButton)
        {
            // Build button navigation
            for (int i = 0; i < listButtons.Length; i++)
            {
                Button button = listButtons[i];
                Navigation nav = button.navigation;

                if (i == 0)
                    EventSystem.current.SetSelectedGameObject(button.gameObject);

                if (i > 0)
                    nav.selectOnUp = listButtons[i - 1];

                if (i < listButtons.Length - 1)
                    nav.selectOnDown = listButtons[i + 1];

                nav.selectOnRight = rightButton;
                button.navigation = nav;
            }
        }

        public void ClearChildren(GameObject panel)
        {
            for (int i = 0; i < panel.transform.childCount; i++)
                Destroy(panel.transform.GetChild(i).gameObject);
        }

        /// <summary>
        /// Is any window opened? Only windows that disrupt the game flow (for example inventory window) count.
        /// </summary>
        /// <returns></returns>
        public bool IsAnyWindowOpen()
        {
            return InventoryContainer.IsOpen || BattleContainer.IsOpen || DialogueContainer.IsOpen || ShopContainer.IsOpen;
        }
    }
}
