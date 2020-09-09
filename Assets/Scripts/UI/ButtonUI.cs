using System;
using TMPro;
using TUFG.Battle;
using TUFG.Dialogue;
using UnityEngine;
using UnityEngine.UI;

namespace TUFG.UI
{
    public class ButtonUI : MonoBehaviour
    {
        private Button button;

        public void InitButton(Button button)
        {
            this.button = button;

            transform.name = button.text;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = button.text;
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(button.ExecuteButtonFunction);
        }
    }
}
