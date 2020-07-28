using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TUFG.UI
{
    public class DialogueButtonUI : MonoBehaviour
    {
        UnityEvent function = null;

        public void InitButton(string text, UnityEvent function)
        {
            transform.name = text;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
            this.function = function;
            GetComponent<Button>().onClick.AddListener(ButtonFunction);
        }

        public void ButtonFunction()
        {
            function.Invoke();
        }
    }
}
