using System.Collections;
using System.Collections.Generic;
using TUFG.Dialogue;
using TUFG.UI;
using UnityEngine;
using UnityEngine.EventSystems;

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

        [SerializeField] private GameObject buttonPrefab = null;
        public GameObject ButtonPrefab { get => buttonPrefab; set => buttonPrefab = value; }

        internal static DialogueContainer dialogueContainer;
        public static DialogueContainer DialogueContainer
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
        public static BattleContainer BattleContainer
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

        #region Unity functions
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        public void ShowMessage(string authorName, string message, Sprite authorAvatar, DialogueButton[] buttons, DialogueAvatarPosition avatarPosition = DialogueAvatarPosition.Left)
        {
            DialogueContainer.ShowMessage(authorName, message, authorAvatar, buttons, avatarPosition);
        }

        public void HideMessage()
        {
            DialogueContainer.HideMessage();
        }

        public void ShowBattleActions(DialogueButton[] buttons)
        {
            BattleContainer.ShowBattleActions(buttons);
        }

        public void BuildButtons(DialogueButton[] buttons, GameObject buttonPanel, string noButtonText)
        {
            if (buttons == null || buttons.Length == 0)
            {
                // Create "Next" button
                DialogueButton continueButton = new DialogueButton();
                continueButton.text = noButtonText;
                continueButton.function = DialogueButtonFunction.GoToNextNode;

                GameObject buttonInstance = Instantiate<GameObject>(buttonPrefab);
                buttonInstance.transform.SetParent(buttonPanel.transform);
                buttonInstance.GetComponent<DialogueButtonUI>().InitButton(continueButton);

                EventSystem.current.SetSelectedGameObject(buttonInstance);
            }
            else
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    GameObject buttonInstance = Instantiate<GameObject>(buttonPrefab);
                    buttonInstance.transform.SetParent(buttonPanel.transform);
                    buttonInstance.GetComponent<DialogueButtonUI>().InitButton(buttons[i]);

                    if (i == 0)
                        EventSystem.current.SetSelectedGameObject(buttonInstance);
                }

            }
        }

        public void ClearChildren(GameObject panel)
        {
            for (int i = 0; i < panel.transform.childCount; i++)
                Destroy(panel.transform.GetChild(i).gameObject);
        }
    }
}
