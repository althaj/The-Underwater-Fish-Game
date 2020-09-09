﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUFG.Dialogue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TUFG.UI
{
    public class BattleContainer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText = null;

        [SerializeField] private GameObject buttonPanel = null;

        private GameObject buttonPrefab;
        private GameObject battlePanel;
        private bool isOpen = false;

        #region Unity functions

        private void Start()
        {
            battlePanel = transform.GetChild(0).gameObject;
            battlePanel.SetActive(false);
            buttonPanel.SetActive(false);

            buttonPrefab = GetComponentInParent<UIManager>().ButtonPrefab;
        }
        #endregion

        /// <summary>
        /// Shows actions available to the player.
        /// </summary>
        /// <param name="buttons">Buttons with actions</param>
        /// <param name="text">Title text</param>
        public void ShowBattleActions(Button[] buttons, string text)
        {
            if (!this.isOpen)
            {
                // TODO PLAY ANIMATION
                battlePanel.SetActive(true);
                buttonPanel.SetActive(true);
                isOpen = true;
            }

            titleText.text = text;

            UIManager.Instance.ClearChildren(buttonPanel);
            UIManager.Instance.BuildButtons(buttons, buttonPanel, "Wait");
        }

        /// <summary>
        /// Hide player actions panel.
        /// </summary>
        public void HideActions()
        {
            isOpen = false;
            battlePanel.SetActive(false);
            buttonPanel.SetActive(false);
        }
    }
}

