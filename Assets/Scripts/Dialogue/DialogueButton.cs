using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TUFG.Dialogue
{
    public enum DialogueButtonFunction
    {
        GoToNextNode,
        JumpToNode,
        SetGameValue,
        SetDialogueValue,
        EndConversation
    }

    [Serializable]
    public class DialogueButton
    {
        public string text;
        public DialogueButtonFunction function;

        public string jumpToNodeId;
    }
}
