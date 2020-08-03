using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TUFG.Dialogue;

namespace TUFG.UI
{
    public class DialogueContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI authorNameText = null;
        [SerializeField] private TextMeshProUGUI messageText = null;
        [SerializeField] private Image avatarImage = null;
        [SerializeField] private HorizontalLayoutGroup middlePanelLayoutGroup = null;

        [SerializeField] private GameObject buttonPanel = null;
        [SerializeField] private GameObject buttonPrefab = null;

        private GameObject dialoguePanel;
        private bool isOpen = false;

        #region Unity functions

        private void Start()
        {
            dialoguePanel = transform.GetChild(0).gameObject;
            dialoguePanel.SetActive(false);
            buttonPanel.SetActive(false);
        }
        #endregion

        public void ShowMessage(string authorName, string message, Sprite authorAvatar, DialogueButton[] buttons, DialogueAvatarPosition avatarPosition = DialogueAvatarPosition.Left)
        {
            if (!this.isOpen)
            {
                // TODO PLAY ANIMATION
                dialoguePanel.SetActive(true);
                buttonPanel.SetActive(true);
            }

            authorNameText.text = authorName;
            messageText.text = message;
            avatarImage.sprite = authorAvatar;

            middlePanelLayoutGroup.reverseArrangement = avatarPosition == DialogueAvatarPosition.Right;

            ClearButtons();
            
            if(buttons == null || buttons.Length == 0)
            {
                // Create "Next" button
                DialogueButton continueButton = new DialogueButton();
                continueButton.text = "Continue";
                continueButton.function = DialogueButtonFunction.GoToNextNode;

                GameObject buttonInstance = Instantiate<GameObject>(buttonPrefab);
                buttonInstance.transform.SetParent(buttonPanel.transform);
                buttonInstance.GetComponent<DialogueButtonUI>().InitButton(continueButton);

            } else
            {
                for(int i = 0; i < buttons.Length; i++)
                {
                    GameObject buttonInstance = Instantiate<GameObject>(buttonPrefab);
                    buttonInstance.transform.SetParent(buttonPanel.transform);
                    buttonInstance.GetComponent<DialogueButtonUI>().InitButton(buttons[i]);
                }
            }

            isOpen = true;
        }

        public void HideMessage()
        {
            isOpen = false;
            dialoguePanel.SetActive(false);
            buttonPanel.SetActive(false);
        }

        private void ClearButtons()
        {
            for (int i = 0; i < buttonPanel.transform.childCount; i++)
                Destroy(buttonPanel.transform.GetChild(i).gameObject);
        }
    }
}

