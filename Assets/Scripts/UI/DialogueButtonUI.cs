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
            GetComponent<Button>().onClick.AddListener(ExecuteButtonFunction);
        }

        private void ExecuteButtonFunction()
        {
            Button buttonComponent = GetComponent<Button>();
            switch (button.function)
            {
                case DialogueButtonFunction.GoToNextNode:
                    DialogueManager.Instance.GoToNextNode();
                    break;
                case DialogueButtonFunction.EndConversation:
                    DialogueManager.Instance.EndConversation();
                    break;
                case DialogueButtonFunction.JumpToNode:
                    DialogueManager.Instance.JumpToNode(button.jumpToNodeId);
                    break;
                case DialogueButtonFunction.SetDialogueValue:
                    throw new NotImplementedException();
                case DialogueButtonFunction.SetGameValue:
                    throw new NotImplementedException();
                case DialogueButtonFunction.StartBattle:
                    BattleManager.InitBattle(button.allies, button.enemies);
                    break;
            }
        }
    }
}
