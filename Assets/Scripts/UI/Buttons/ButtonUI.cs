using System;
using TMPro;
using TUFG.Battle;
using TUFG.Dialogue;
using UnityEngine;
using UnityEngine.UI;

namespace TUFG.UI
{
    /// <summary>
    /// UI object representing a generic button.
    /// </summary>
    public class ButtonUI : MonoBehaviour
    {
        private GenericButton button;

        /// <summary>
        /// Initialize the button UI object.
        /// </summary>
        /// <param name="button">Generic button to get the data from.</param>
        public void InitButton(GenericButton button)
        {
            this.button = button;

            transform.name = button.text;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = button.text;
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(button.ExecuteButtonFunction);
        }
    }
}
