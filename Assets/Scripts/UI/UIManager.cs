using System.Collections;
using System.Collections.Generic;
using TUFG.Controls;
using TUFG.Core;
using TUFG.Dialogue;
using TUFG.Inventory;
using TUFG.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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

        private List<ContainerBehaviour> openedContainers;

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

        internal static MainMenuContainer mainMenuContainer;
        internal static MainMenuContainer MainMenuContainer
        {
            get
            {
                if (mainMenuContainer == null)
                    mainMenuContainer = Instance.GetComponentInChildren<MainMenuContainer>();

                if (mainMenuContainer == null)
                    Debug.LogError("Cannot find the main menu container!!");
                return mainMenuContainer;
            }
        }

        internal static PauseContainer pauseContainer;
        internal static PauseContainer PauseContainer
        {
            get
            {
                if (pauseContainer == null)
                    pauseContainer = Instance.GetComponentInChildren<PauseContainer>();

                if (pauseContainer == null)
                    Debug.LogError("Cannot find the pause container!!");
                return pauseContainer;
            }
        }
        #endregion

        #region Unity functions
        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Instance.controlsInput = new ControlsInput();
            Instance.controlsInput.UI.Pause.performed += Instance.PauseButtonPressed;

            openedContainers = new List<ContainerBehaviour>();

            Invoke("OpenMainMenu", 0.5f);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
            //GameManager.Instance.SaveGame();
        }
        #endregion

        #region Show / Hide methods
        /// <summary>
        /// Hide all openend windows. Does not close the windows, just hide them.
        /// </summary>
        public void HideOpenendWindows()
        {
            foreach(ContainerBehaviour container in openedContainers)
            {
                container.Close();
            }
        }

        /// <summary>
        /// Shows openend windows that were hidden by the method HideOpenendWindows().
        /// </summary>
        public void ShowOpenendWindows()
        {
            if(openedContainers.Count > 0)
            {
                foreach (ContainerBehaviour container in openedContainers)
                {
                    container.Open();
                }

                FindObjectOfType<PlayerMovement>().DisableInput();
            }
        }

        /// <summary>
        /// Open main menu and load the main menu scene.
        /// </summary>
        public void OpenMainMenu()
        {
            MainMenuContainer.Open();

            if(SceneManager.GetActiveScene().buildIndex != 1)
                SceneManager.LoadScene(1);

            openedContainers.Add(MainMenuContainer);
        }

        /// <summary>
        /// Close the main menu window.
        /// </summary>
        public void CloseMainMenu()
        {
            MainMenuContainer.Close();

            openedContainers.Remove(MainMenuContainer);
        }

        /// <summary>
        /// Show a dialogue message.
        /// </summary>
        /// <param name="authorName">Name of the author of the message.</param>
        /// <param name="message">Text of the message.</param>
        /// <param name="authorAvatar">Avatar of the author of the message.</param>
        /// <param name="buttons">Interaction buttons of the message.</param>
        /// <param name="avatarPosition">Position of the avatar in the dialogue panel.</param>
        public void OpenMessage(string authorName, string message, Sprite authorAvatar, GenericButton[] buttons, DialogueAvatarPosition avatarPosition = DialogueAvatarPosition.Left)
        {
            if (!openedContainers.Contains(DialogueContainer))
                openedContainers.Add(DialogueContainer);


            DialogueContainer.SetMessage(authorName, message, authorAvatar, buttons, avatarPosition);
            DialogueContainer.Open();
        }

        /// <summary>
        /// Hide current dialogue message.
        /// </summary>
        public void CloseMessage()
        {
            DialogueContainer.Close();

            openedContainers.Remove(DialogueContainer);
        }

        /// <summary>
        /// Display battle actions for player to choose from.
        /// </summary>
        /// <param name="buttons">Buttons to display.</param>
        /// <param name="text">Title text of the panel.</param>
        public void OpenBattleActions(GenericButton[] buttons, string text)
        {
            if (!openedContainers.Contains(BattleContainer))
                openedContainers.Add(BattleContainer);

            BattleContainer.SetCurrentBattleActions(buttons, text);
            BattleContainer.Open();
        }

        /// <summary>
        /// Hide battle actions panel.
        /// </summary>
        public void CloseActions()
        {
            BattleContainer.Close();

            openedContainers.Remove(BattleContainer);
        }

        /// <summary>
        /// Display inventory window.
        /// </summary>
        public void OpenInventory()
        {
            if (!openedContainers.Contains(InventoryContainer))
                openedContainers.Add(InventoryContainer);

            InventoryContainer.Open();
        }

        /// <summary>
        /// Hide inventory window.
        /// </summary>
        public void CloseInventory()
        {
            InventoryContainer.Close();
            OpenPauseMenu();

            openedContainers.Remove(InventoryContainer);
        }

        /// <summary>
        /// Display a shop window.
        /// </summary>
        /// <param name="shop">Shop to display.</param>
        public void OpenShop(Shop shop)
        {
            if (!openedContainers.Contains(ShopContainer))
                openedContainers.Add(ShopContainer);

            DialogueManager.Instance.EndConversation();
            ShopContainer.SetShop(shop);
            ShopContainer.Open();
        }

        /// <summary>
        /// Hide current shop.
        /// </summary>
        public void CloseShop()
        {
            ShopContainer.Close();

            openedContainers.Remove(ShopContainer);
        }

        /// <summary>
        /// Open party management window.
        /// </summary>
        public void OpenPartyWindow()
        {
            if (!openedContainers.Contains(PartyContainer))
                openedContainers.Add(PartyContainer);

            PartyContainer.Open();
        }

        /// <summary>
        /// Close the party management window.
        /// </summary>
        public void ClosePartyWindow()
        {
            PartyContainer.Close();
            OpenPauseMenu();

            openedContainers.Remove(PartyContainer);
        }

        /// <summary>
        /// Open the pause menu.
        /// </summary>
        public void OpenPauseMenu()
        {
            if (!openedContainers.Contains(PauseContainer))
                openedContainers.Add(PauseContainer);

            if (InventoryContainer.IsOpen)
                CloseInventory();

            if (PartyContainer.IsOpen)
                ClosePartyWindow();

            HideOpenendWindows();
            PauseContainer.Open();
        }

        /// <summary>
        /// Close the pause menu.
        /// </summary>
        public void ClosePauseMenu()
        {
            PauseContainer.Close();
            openedContainers.Remove(PauseContainer);

            ShowOpenendWindows();
        }

        /// <summary>
        /// Hide the pause menu without closing it.
        /// </summary>
        public void HidePauseMenu()
        {
            PauseContainer.Close();
        }

        /// <summary>
        /// The pause button has been pressed.
        /// </summary>
        /// <param name="ctx"></param>
        public void PauseButtonPressed(CallbackContext ctx)
        {
            if (MainMenuContainer.IsOpen)
                return;

            if (PauseContainer.IsOpen)
                ClosePauseMenu();
            else
                OpenPauseMenu();
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
            return openedContainers.Count > 0;
        } 
        #endregion
    }
}
