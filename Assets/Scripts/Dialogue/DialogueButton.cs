using System;
using System.Collections;
using System.Collections.Generic;
using TUFG.Battle;
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
        EndConversation,
        StartBattle
    }

    [Serializable]
    public class DialogueButton
    {
        public string text;
        public DialogueButtonFunction function;

        public string jumpToNodeId;

        public Unit[] allies;
        public Unit[] enemies;
    }
}
