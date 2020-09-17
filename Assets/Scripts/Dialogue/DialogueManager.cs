using System.Collections;
using System.Collections.Generic;
using TUFG.UI;
using UnityEngine;

namespace TUFG.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        #region Singleton pattern
        private static DialogueManager _instance;
        public static DialogueManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<DialogueManager>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("Dialogue Manager");
                        _instance = container.AddComponent<DialogueManager>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        private static DialogueNode[] currentConversation = new DialogueNode[0];
        private static int currentNodeID = -1;

        public static DialogueNode CurrentDialogueNode
        {
            get
            {
                if(currentConversation == null)
                {
                    return null;
                }

                if(currentNodeID < 0 || currentNodeID >= currentConversation.Length)
                {
                    Debug.LogErrorFormat("CurrentDialogueNode: Out of array bounds: \nConversation length = {0}, current node ID = {1}", currentConversation.Length, currentNodeID);
                    return null;
                }
                return currentConversation[currentNodeID];
            }
        }

        public void DisplayCurrentDialogueNode()
        {
            DialogueNode currentNode = CurrentDialogueNode;
            if (currentNode == null)
            {
                UIManager.Instance.HideMessage();
            }
            else
            {
                NodeContent content = currentNode.GetNodeContent();
                UIManager.Instance.ShowMessage(content.characterName, content.dialogueText, content.avatar, content.dialogueButtons, content.avatarPosition);
            }
        }

        public void InitConversation(DialogueConversation conversation)
        {
            currentConversation = conversation.dialogueNodes;
            currentNodeID = 0;
            DisplayCurrentDialogueNode();

            FindObjectOfType<PlayerMovement>().DisableInput();
        }

        public void GoToNextNode()
        {
            if (HasNextNode)
            {
                currentNodeID++;
                CurrentDialogueNode.Process();
                DisplayCurrentDialogueNode();
            } else
            {
                EndConversation();
            }
        }

        public void EndConversation()
        {
            currentNodeID = -1;
            currentConversation = null;
            DisplayCurrentDialogueNode();

            FindObjectOfType<PlayerMovement>().EnableInput();
        }

        public bool HasNextNode
        {
            get
            {
                return (currentConversation != null && currentNodeID >= 0 && currentNodeID < currentConversation.Length - 1);
            }
        }

        public void JumpToNode(string nodeId)
        {
            for(int i = 0; i < currentConversation.Length; i++)
            {
                if (currentConversation[i].nodeID.CompareTo(nodeId) == 0)
                {
                    JumpToNode(i);
                    return;
                }
            }
            Debug.LogErrorFormat("JumpToNode: Could not find a node with ID {0}", nodeId);
        }

        public void JumpToNode(int nodeIndex)
        {
            currentNodeID = nodeIndex;
            DisplayCurrentDialogueNode();
        }
    }
}
