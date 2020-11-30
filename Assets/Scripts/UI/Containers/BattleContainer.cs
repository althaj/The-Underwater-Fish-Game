using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUFG.Dialogue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TUFG.UI
{
    /// <summary>
    /// Container with battle UI.
    /// </summary>
    public class BattleContainer : ContainerBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText = null;

        [SerializeField] private GameObject buttonPanel = null;

        private GameObject buttonPrefab;
        private GameObject battlePanel;

        private GenericButton[] currentButtons;
        private string currentText;

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
        /// Shows actions available to the player. Call Open() to show the battle container.
        /// </summary>
        /// <param name="buttons">Buttons with actions</param>
        /// <param name="text">Title text</param>
        public void SetCurrentBattleActions(GenericButton[] buttons, string text)
        {
            currentButtons = buttons;
            currentText = text;
        }

        /// <summary>
        /// Hide player actions panel.
        /// </summary>
        public override void Close()
        {
            IsOpen = false;
            battlePanel.SetActive(false);
            buttonPanel.SetActive(false);
        }

        /// <summary>
        /// Open the battle container.
        /// </summary>
        public override void Open()
        {
            if (!this.IsOpen)
            {
                // TODO PLAY ANIMATION
                battlePanel.SetActive(true);
                buttonPanel.SetActive(true);
                IsOpen = true;
            }

            titleText.text = currentText;

            UIManager.Instance.ClearChildren(buttonPanel);
            UIManager.Instance.BuildButtons(currentButtons, buttonPanel, "Wait");
        }
    }
}

