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

        public UnitData[] allies;
        public UnitData[] enemies;

        public void ExecuteButtonFunction()
        {
            switch (function)
            {
                case DialogueButtonFunction.GoToNextNode:
                    DialogueManager.Instance.GoToNextNode();
                    break;
                case DialogueButtonFunction.EndConversation:
                    DialogueManager.Instance.EndConversation();
                    break;
                case DialogueButtonFunction.JumpToNode:
                    DialogueManager.Instance.JumpToNode(jumpToNodeId);
                    break;
                case DialogueButtonFunction.SetDialogueValue:
                    throw new NotImplementedException();
                case DialogueButtonFunction.SetGameValue:
                    throw new NotImplementedException();
                case DialogueButtonFunction.StartBattle:
                    BattleManager.InitBattle(allies, enemies);
                    DialogueManager.Instance.EndConversation();
                    break;
            }
        }
    }
}
