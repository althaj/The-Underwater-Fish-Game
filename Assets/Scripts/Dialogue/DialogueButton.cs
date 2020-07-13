using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TUFG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue Button", menuName = "Dialogues/Dialogue Button")]
    public class DialogueButton : ScriptableObject
    {
        public string text;
        public UnityEvent function;
    }
}
