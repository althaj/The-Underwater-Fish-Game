using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUFG.Battle;
using UnityEngine;
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
            List<Unit> units = PartyManager.Instance.GetPlayerParty(false);

            if (!IsOpen)
            {
                // TODO PLAY ANIMATION
                partyPanel.SetActive(true);
                IsOpen = true;
            }

            UIManager.Instance.ClearChildren(goonsPanel.gameObject);
            List<Button> buttons = new List<Button>();

            foreach (Unit unit in units)
            {
                GameObject button = CreateButton(unit);

                buttons.Add(button.GetComponent<Button>());
            }

            UIManager.BuildListButtonNavigation(buttons.ToArray(), detailsPanel.GetComponentInChildren<Button>());
        }

        /// <summary>
        /// Close the party manager window.
        /// </summary>
        public override void Close()
        {
            partyPanel.SetActive(false);
            IsOpen = false;
            UIManager.Instance.OpenPauseMenu();
        }

        /// <summary>
        /// Create a button for unit in party.
        /// </summary>
        /// <param name="unit">Unit to create the button for.</param>
        /// <returns></returns>
        private GameObject CreateButton(Unit unit)
        {
            GameObject buttonInstance = Instantiate<GameObject>(buttonPrefab);
            buttonInstance.transform.SetParent(goonsPanel);
            buttonInstance.GetComponent<GoonButton>().InitButton(unit, this);

            return buttonInstance;
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
        }

        /// <summary>
        /// Kick the selected unit out of party.
        /// </summary>
        public void KickOut()
        {
            PartyManager.Instance.KickOut(selectedUnit);
            Open();
        }
        #endregion
    }
}
