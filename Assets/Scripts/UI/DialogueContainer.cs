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

        private GameObject dialoguePanel;
        private bool isOpen = false;

        #region Unity functions

        private void Start()
        {
            dialoguePanel = transform.GetChild(0).gameObject;
            dialoguePanel.SetActive(false);
        }
        #endregion

        public void ShowMessage(string authorName, string message, Sprite authorAvatar, DialogueAvatarPosition avatarPosition = DialogueAvatarPosition.Left)
        {
            if (!this.isOpen)
            {
                // TODO PLAY ANIMATION
                dialoguePanel.SetActive(true);
            }

            authorNameText.text = authorName;
            messageText.text = message;
            avatarImage.sprite = authorAvatar;

            middlePanelLayoutGroup.reverseArrangement = avatarPosition == DialogueAvatarPosition.Right;

            isOpen = true;
        }

        public void HideMessage()
        {
            isOpen = false;
            dialoguePanel.SetActive(false);
        }
    }
}

