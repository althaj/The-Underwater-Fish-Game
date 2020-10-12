using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TUFG.Dialogue;
using UnityEngine.EventSystems;

namespace TUFG.UI
{
    public class DialogueContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI authorNameText = null;
        [SerializeField] private TextMeshProUGUI messageText = null;
        [SerializeField] private Image avatarImage = null;
        [SerializeField] private HorizontalLayoutGroup middlePanelLayoutGroup = null;

        [SerializeField] private GameObject buttonPanel = null;
                
        private GameObject buttonPrefab;
        private GameObject dialoguePanel;
        private bool isOpen = false;

        #region Unity functions

        private void Start()
        {
            dialoguePanel = transform.GetChild(0).gameObject;
            dialoguePanel.SetActive(false);
            buttonPanel.SetActive(false);

            buttonPrefab = GetComponentInParent<UIManager>().ButtonPrefab;
        }
        #endregion

        /// <summary>
        /// Show dialogue message.
        /// </summary>
        /// <param name="authorName">Author name to be displayed</param>
        /// <param name="message">Message to be displayed</param>
        /// <param name="authorAvatar">Avatar of the speaker</param>
        /// <param name="buttons">Action buttons</param>
        /// <param name="avatarPosition">Position on left or right</param>
        public void ShowMessage(string authorName, string message, Sprite authorAvatar, GenericButton[] buttons, DialogueAvatarPosition avatarPosition)
        {
            if (!this.isOpen)
            {
                // TODO PLAY ANIMATION
                dialoguePanel.SetActive(true);
                buttonPanel.SetActive(true);
                isOpen = true;
            }

            authorNameText.text = authorName;
            messageText.text = message;
            avatarImage.sprite = authorAvatar;

            middlePanelLayoutGroup.reverseArrangement = avatarPosition == DialogueAvatarPosition.Right;

            UIManager.Instance.ClearChildren(buttonPanel);
            UIManager.Instance.BuildButtons(buttons, buttonPanel, "Continue");
        }

        /// <summary>
        /// Hide dialogue message.
        /// </summary>
        public void HideMessage()
        {
            isOpen = false;
            dialoguePanel.SetActive(false);
            buttonPanel.SetActive(false);
        }
    }
}

