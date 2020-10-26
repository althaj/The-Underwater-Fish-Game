using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUFG.Battle;
using UnityEngine;

namespace TUFG.UI
{
    /// <summary>
    /// Container holding window for party information and management.
    /// </summary>
    public class PartyContainer : MonoBehaviour
    {
        private GameObject partyPanel;
        private bool isOpen = false;

        private TextMeshProUGUI nameText;
        private TextMeshProUGUI healthText;
        private TextMeshProUGUI armorText;
        private TextMeshProUGUI strengthText;
        private TextMeshProUGUI powerText;
        private TextMeshProUGUI speedText;
        private TextMeshProUGUI descriptionText;

        [SerializeField] private Transform goonsPanel;
        [SerializeField] private Transform detailsPanel;

        /// <summary>
        /// Is the dialogue panel currently open?
        /// </summary>
        public bool IsOpen { get => isOpen; private set => isOpen = value; }

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

            partyPanel.SetActive(false);
        }
        #endregion

        #region Public methods
        public void SelectItem(Unit unit)
        {
            throw new NotImplementedException();
        }

        public void ScrollToObject(Transform transform)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
