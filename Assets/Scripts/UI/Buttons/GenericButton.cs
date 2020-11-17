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
    /// <summary>
    /// Enumeration of the dialogue button functions.
    /// </summary>
    public enum DialogueButtonFunction
    {
        GoToNextNode,
        JumpToNode,
        SetGameValue,
        SetDialogueValue,
        EndConversation,
        StartBattle,
        OpenShop,
        AddUnitToParty
    }

    /// <summary>
    /// Enumeration of generic button types.
    /// </summary>
    public enum ButtonType
    {
        Dialogue,
        Ability,
        Target
    }

    /// <summary>
    /// Class representing a generic button data.
    /// </summary>
    [Serializable]
    public class GenericButton
    {
        /// <summary>
        /// Text of the button.
        /// </summary>
        public string text;

        /// <summary>
        /// Type of the button.
        /// </summary>
        public ButtonType buttonType;

        /// <summary>
        /// Function of the button. Only applies to dialogue buttons.
        /// </summary>
        public DialogueButtonFunction dialogueFunction;

        /// <summary>
        /// ID of shop to open. Only applies to OpenShop dialogue button function.
        /// </summary>
        public string shopId;

        /// <summary>
        /// Target to select. Only applies to Target button type.
        /// </summary>
        public Unit target;

        /// <summary>
        /// Unit data used to add units to your party.
        /// </summary>
        public UnitData unitData;

        /// <summary>
        /// ID of node to jump to in dialogue. Only applies to JumpToNode dialogue button function.
        /// </summary>
        public string jumpToNodeId;

        /// <summary>
        /// Array of units to start battle against. Only applies to StartBattle dialogue button function.
        /// </summary>
        public UnitData[] enemies;

        /// <summary>
        /// Ability to select. Only applies to Ability button type.
        /// </summary>
        public Ability ability;

        /// <summary>
        /// Execute the function of the button.
        /// </summary>
        public void ExecuteButtonFunction()
        {
            switch (buttonType)
            {
                case ButtonType.Dialogue:
                    switch (dialogueFunction)
                    {
                        case DialogueButtonFunction.GoToNextNode:
                            if (BattleManager.Instance.IsBattleInProgress())
                                BattleManager.Instance.WaitAction();
                            else
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
                            UIManager.Instance.OpenShop(ShopManager.Instance.GetShop(shopId));
                            break;
                        case DialogueButtonFunction.AddUnitToParty:
                            DialogueManager.Instance.EndConversation();
                            PartyManager.Instance.AddUnit(unitData);
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
