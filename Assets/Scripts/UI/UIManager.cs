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
    /// <summary>
    /// Class managing the UI.
    /// <remarks>Uses a singleton pattern.</remarks>
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Singleton pattern
        private static UIManager _instance;

        /// <summary>
        /// Current instance of the UI manager. Creates an Unity object.
        /// </summary>
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
        [SerializeField] private GameObject goonButtonPrefab = null;
        [SerializeField] private Sprite handsSlotIcon = null;
        [SerializeField] private Sprite bodySlotIcon = null;
        [SerializeField] private Sprite legsSlotIcon = null;
        [SerializeField] private Sprite amuletSlotIcon = null;
        [SerializeField] private Sprite ringSlotIcon = null;

        /// <summary>
        /// Prefab of a generic button.
        /// </summary>
        public GameObject ButtonPrefab { get => buttonPrefab; set => buttonPrefab = value; }

        /// <summary>
        /// Prefab of an inventory button.
        /// </summary>
        public GameObject InventoryButtonPrefab { get => inventoryButtonPrefab; set => inventoryButtonPrefab = value; }

        /// <summary>
        /// Prefab of a shop button.
        /// </summary>
        public GameObject ShopButtonPrefab { get => shopButtonPrefab; set => shopButtonPrefab = value; }

        /// <summary>
        /// Prefab for a goon button in party manager.
        /// </summary>
        public GameObject GoonButtonPrefab { get => goonButtonPrefab; set => goonButtonPrefab = value; }

        /// <summary>
        /// Icon of hand slot in shop and inventory.
        /// </summary>
        public Sprite HandsSlotIcon { get => handsSlotIcon; set => handsSlotIcon = value; }

        /// <summary>
        /// Icon of body slot in shop and inventory.
        /// </summary>
        public Sprite BodySlotIcon { get => bodySlotIcon; set => bodySlotIcon = value; }

        /// <summary>
        /// Icon of legs slot in shop and inventory.
        /// </summary>
        public Sprite LegsSlotIcon { get => legsSlotIcon; set => legsSlotIcon = value; }

        /// <summary>
        /// Icon of amulet slot in shop and inventory.
        /// </summary>
        public Sprite AmuletSlotIcon { get => amuletSlotIcon; set => amuletSlotIcon = value; }

        /// <summary>
        /// Icon of ring slot in shop and inventory.
        /// </summary>
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

        internal static PartyContainer partyContainer;
        internal static PartyContainer PartyContainer
        {
            get
            {
                if (partyContainer == null)
                    partyContainer = Instance.GetComponentInChildren<PartyContainer>();

                if (partyContainer == null)
                    Debug.LogError("Cannot find the party container!!");
                return partyContainer;
            }
        }
        #endregion

        #region Unity functions
        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Instance.controlsInput = new ControlsInput();
            Instance.controlsInput.UI.OpenInventory.performed += Instance.InventoryButtonPressed;
            Instance.controlsInput.UI.OpenPartyManagement.performed += Instance.PartyButtonPressed;

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
        /// <summary>
        /// Show a dialogue message.
        /// </summary>
        /// <param name="authorName">Name of the author of the message.</param>
        /// <param name="message">Text of the message.</param>
        /// <param name="authorAvatar">Avatar of the author of the message.</param>
        /// <param name="buttons">Interaction buttons of the message.</param>
        /// <param name="avatarPosition">Position of the avatar in the dialogue panel.</param>
        public void ShowMessage(string authorName, string message, Sprite authorAvatar, GenericButton[] buttons, DialogueAvatarPosition avatarPosition = DialogueAvatarPosition.Left)
        {
            DialogueContainer.ShowMessage(authorName, message, authorAvatar, buttons, avatarPosition);
        }

        /// <summary>
        /// Hide current dialogue message.
        /// </summary>
        public void HideMessage()
        {
            DialogueContainer.HideMessage();
        }

        /// <summary>
        /// Display battle actions for player to choose from.
        /// </summary>
        /// <param name="buttons">Buttons to display.</param>
        /// <param name="text">Title text of the panel.</param>
        public void ShowBattleActions(GenericButton[] buttons, string text)
        {
            BattleContainer.ShowBattleActions(buttons, text);
        }

        /// <summary>
        /// Hide battle actions panel.
        /// </summary>
        public void HideActions()
        {
            BattleContainer.HideActions();
        }

        /// <summary>
        /// Display inventory window.
        /// </summary>
        public void ShowInventory()
        {
            InventoryContainer.ShowInventory();
        }

        /// <summary>
        /// Callback for inventory button pressed, to toggle the inventory.
        /// </summary>
        /// <param name="ctx"></param>
        public void InventoryButtonPressed(CallbackContext ctx)
        {
            if(!IsAnyWindowOpen() || InventoryContainer.IsOpen)
                InventoryContainer.ToggleInventory();
        }

        /// <summary>
        /// Hide inventory window.
        /// </summary>
        public void HideInventory()
        {
            InventoryContainer.HideInventory();
        }

        /// <summary>
        /// Display a shop window.
        /// </summary>
        /// <param name="shop">Shop to display.</param>
        public void ShowShop(Shop shop)
        {
            DialogueManager.Instance.EndConversation();
            ShopContainer.ShowShop(shop);
        }

        /// <summary>
        /// Hide current shop.
        /// </summary>
        public void HideShop()
        {
            ShopContainer.HideShop();
        }

        /// <summary>
        /// Listener for the party button pressed.
        /// </summary>
        /// <param name="ctx"></param>
        public void PartyButtonPressed(CallbackContext ctx)
        {
            if (!IsAnyWindowOpen())
                ShowPartyWindow();
        }

        /// <summary>
        /// Open party management window.
        /// </summary>
        public void ShowPartyWindow()
        {
            PartyContainer.ShowParty();
        }

        /// <summary>
        /// Close the party management window.
        /// </summary>
        public void HidePartyWindow()
        {
            PartyContainer.HideParty();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Build generic buttons.
        /// </summary>
        /// <param name="buttons">Buttons to add.</param>
        /// <param name="buttonPanel">Panel to put buttons to.</param>
        /// <param name="noButtonText">Text to display on a button when no buttons are present.</param>
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

        /// <summary>
        /// Destroy children of a panel.
        /// </summary>
        /// <param name="panel">Parent for destroying children elements.</param>
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
        #endregion
    }
}
