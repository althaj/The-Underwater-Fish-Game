using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUFG.Battle;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TUFG.UI
{
    /// <summary>
    /// Container holding window for party information and management.
    /// </summary>
    public class PartyContainer : ContainerBehaviour
    {
        private GameObject partyPanel;
        private Unit selectedUnit;

        private GameObject buttonPrefab;
        private TextMeshProUGUI nameText;
        private TextMeshProUGUI healthText;
        private TextMeshProUGUI armorText;
        private TextMeshProUGUI strengthText;
        private TextMeshProUGUI powerText;
        private TextMeshProUGUI speedText;
        private TextMeshProUGUI descriptionText;

        private Button kickOutButton;
        private Button closeButton;

        [SerializeField] private Transform goonsPanel;
        [SerializeField] private Transform detailsPanel;

        #region Unity methods
        private void Start()
        {
            partyPanel = transform.GetChild(0).gameObject;

            nameText = detailsPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
            healthText = detailsPanel.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
            armorText = detailsPanel.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
            strengthText = detailsPanel.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>();
            powerText = detailsPanel.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>();
            speedText = detailsPanel.GetChild(1).GetChild(9).GetComponent<TextMeshProUGUI>();
            descriptionText = detailsPanel.GetChild(2).GetComponent<TextMeshProUGUI>();

            kickOutButton = detailsPanel.GetChild(3).GetChild(0).GetComponent<Button>();
            closeButton = detailsPanel.GetChild(3).GetChild(1).GetComponent<Button>();

            buttonPrefab = UIManager.Instance.GoonButtonPrefab;

            partyPanel.SetActive(false);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Open the party managment window.
        /// </summary>
        /// <param name="units">Units that are in the party.</param>
        public override void Open()
        {
            FindObjectOfType<PlayerMovement>().DisableInput();

            // Get the units
            List<Unit> units = PartyManager.Instance.GetPlayerParty(true);

            if (!IsOpen)
            {
                // TODO PLAY ANIMATION
                partyPanel.SetActive(true);
                IsOpen = true;
            }

            UIManager.Instance.ClearChildren(goonsPanel.gameObject);
            List<Button> buttons = new List<Button>();
            Button playerButton = null;

            foreach (Unit unit in units)
            {
                Button button = CreateButton(unit);

                buttons.Add(button);

                if (unit.IsPlayer)
                    playerButton = button;
            }

            Button rightButton;

            if (BattleManager.Instance.IsBattleInProgress())
            {
                rightButton = closeButton;
                kickOutButton.interactable = false;
            }
            else
            {
                rightButton = kickOutButton;
                kickOutButton.interactable = true;
            }

            UIManager.BuildListButtonNavigation(buttons.ToArray(), rightButton);

            Navigation playerNav = playerButton.navigation;
            playerNav.selectOnRight = closeButton;
            playerButton.navigation = playerNav;
        }

        /// <summary>
        /// Close the party manager window.
        /// </summary>
        public override void Close()
        {
            partyPanel.SetActive(false);
            IsOpen = false;
        }

        /// <summary>
        /// The close button has been pressed.
        /// </summary>
        public void CloseButtonPressed()
        {
            UIManager.Instance.ClosePartyWindow();
        }

        /// <summary>
        /// Create a button for unit in party.
        /// </summary>
        /// <param name="unit">Unit to create the button for.</param>
        /// <returns></returns>
        private Button CreateButton(Unit unit)
        {
            GameObject buttonInstance = Instantiate(buttonPrefab);
            buttonInstance.transform.SetParent(goonsPanel);
            buttonInstance.GetComponent<GoonButton>().InitButton(unit, this);

            return buttonInstance.GetComponent<Button>();
        }

        /// <summary>
        /// Select a unit.
        /// </summary>
        /// <param name="unit">Unit that is selected</param>
        public void SelectItem(Unit unit)
        {
            selectedUnit = unit;

            nameText.text = unit.Name;
            healthText.text = $"{unit.Health} / {unit.MaxHealth}";
            armorText.text = unit.Armor.ToString();
            strengthText.text = unit.Strength.ToString();
            powerText.text = unit.Power.ToString();
            speedText.text = unit.Speed.ToString();
            descriptionText.text = unit.UnitData.description;

            if (unit.IsPlayer)
            {
                kickOutButton.interactable = false;
            }
            else
            {
                kickOutButton.interactable = true;
            }
        }

        /// <summary>
        /// Kick the selected unit out of party.
        /// </summary>
        public void KickOut()
        {
            PartyManager.Instance.KickOut(selectedUnit);
            UIManager.Instance.OpenPartyWindow();
        }
        #endregion
    }
}
