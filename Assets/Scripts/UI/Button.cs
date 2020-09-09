using System;
using System.Collections;
using System.Collections.Generic;
using TUFG.Battle;
using TUFG.Battle.Abilities;
using TUFG.Dialogue;
using UnityEngine;
using UnityEngine.Events;

namespace TUFG.UI
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

    public enum ButtonType
    {
        Dialogue,
        Ability
    }

    [Serializable]
    public class Button
    {
        public string text;
        public ButtonType buttonType;
        public DialogueButtonFunction dialogueFunction;
        public Ability ability;

        public string jumpToNodeId;

        public UnitData[] enemies;

        public void ExecuteButtonFunction()
        {
            switch (buttonType)
            {
                case ButtonType.Dialogue:
                    switch (dialogueFunction)
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
                            BattleManager.Instance.InitBattle(enemies);
                            DialogueManager.Instance.EndConversation();
                            break;
                    }
                    break;
                case ButtonType.Ability:
                    BattleManager.Instance.SelectAbility(ability);
                    break;
            }
        }
    }
}
