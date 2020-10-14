using System;
using System.Collections;
using System.Collections.Generic;
using TUFG.Battle;
using TUFG.Battle.Abilities;
using TUFG.Dialogue;
using TUFG.Inventory;
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
        StartBattle,
        OpenShop
    }

    public enum ButtonType
    {
        Dialogue,
        Ability,
        Target
    }

    [Serializable]
    public class GenericButton
    {
        public string text;
        public ButtonType buttonType;
        public DialogueButtonFunction dialogueFunction;
        public string shopId;
        public Ability ability;
        public Unit target;

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
                            DialogueManager.Instance.EndConversation();
                            BattleManager.Instance.InitBattle(enemies);
                            break;
                        case DialogueButtonFunction.OpenShop:
                            UIManager.Instance.ShowShop(ShopManager.Instance.GetShop(shopId));
                            break;
                    }
                    break;
                case ButtonType.Ability:
                    BattleManager.Instance.SelectAbility(ability);
                    break;
                case ButtonType.Target:
                    BattleManager.Instance.SelectTarget(target);
                    break;
            }
        }
    }
}
