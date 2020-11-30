using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TUFG.Dialogue;
using UnityEngine.EventSystems;

namespace TUFG.UI
{
    /// <summary>
    /// Container with the dialogue panels.
    /// </summary>
    public class DialogueContainer : ContainerBehaviour
    {
        [SerializeField] private TextMeshProUGUI authorNameText = null;
        [SerializeField] private TextMeshProUGUI messageText = null;
        [SerializeField] private Image avatarImage = null;
        [SerializeField] private HorizontalLayoutGroup middlePanelLayoutGroup = null;

        [SerializeField] private GameObject buttonPanel = null;
                
        private GameObject buttonPrefab;
        private GameObject dialoguePanel;

        private string currentAuthorName;
        private string currentMessage;
        private Sprite currentAuthorAvatar;
        private GenericButton[] currentButtons;
        private DialogueAvatarPosition currentAvatarPosition;

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
        /// Show dialogue message. You need to call Open() to display the container.
        /// </summary>
        /// <param name="authorName">Author name to be displayed</param>
        /// <param name="message">Message to be displayed</param>
        /// <param name="authorAvatar">Avatar of the speaker</param>
        /// <param name="buttons">Action buttons</param>
        /// <param name="avatarPosition">Position on left or right</param>
        public void SetMessage(string authorName, string message, Sprite authorAvatar, GenericButton[] buttons, DialogueAvatarPosition avatarPosition)
        {
            currentAuthorName = authorName;
            currentMessage = message;
            currentAuthorAvatar = authorAvatar;
            currentButtons = buttons;
            currentAvatarPosition = avatarPosition;
        }

        /// <summary>
        /// Open the dialogue container.
        /// </summary>
        public override void Open()
        {
            if (!this.IsOpen)
            {
                // TODO PLAY ANIMATION
                dialoguePanel.SetActive(true);
                buttonPanel.SetActive(true);
                IsOpen = true;
            }

            authorNameText.text = currentAuthorName;
            messageText.text = currentMessage;
            avatarImage.sprite = currentAuthorAvatar;

            middlePanelLayoutGroup.reverseArrangement = currentAvatarPosition == DialogueAvatarPosition.Right;

            UIManager.Instance.ClearChildren(buttonPanel);
            UIManager.Instance.BuildButtons(currentButtons, buttonPanel, "Continue");
        }

        /// <summary>
        /// Hide dialogue container.
        /// </summary>
        public override void Close()
        {
            IsOpen = false;
            dialoguePanel.SetActive(false);
            buttonPanel.SetActive(false);
        }
    }
}

