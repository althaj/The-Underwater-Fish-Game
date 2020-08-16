using System;
using TMPro;
using TUFG.Battle;
using TUFG.Dialogue;
using UnityEngine;
using UnityEngine.UI;

namespace TUFG.UI
{
    public class DialogueButtonUI : MonoBehaviour
    {
        private DialogueButton button;

        public void InitButton(DialogueButton button)
        {
            this.button = button;

            transform.name = button.text;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = button.text;
            GetComponent<Button>().onClick.AddListener(button.ExecuteButtonFunction);
        }
    }
}
