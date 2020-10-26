using System.Collections;
using System.Collections.Generic;
using TMPro;
using TUFG.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace TUFG.UI
{
    public class GoonButton : MonoBehaviour
    {
        private Unit unit;
        private PartyContainer container;

        /// <summary>
        /// Initialize a button with a unit name and it's avatar.
        /// </summary>
        /// <param name="unit">Unit to initialize the button with.</param>
        /// <param name="container">Parent party container.</param>
        public void InitButton(Unit unit, PartyContainer container)
        {
            this.unit = unit;
            this.container = container;

            if (unit.UnitData.avatar != null)
                transform.GetChild(0).GetComponent<Image>().sprite = unit.UnitData.avatar;
            else
                transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = unit.Name;
        }

        /// <summary>
        /// Event listener to the Select event.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
        {
            container.ScrollToObject(transform);
            container.SelectItem(unit);
        }
    } 
}
