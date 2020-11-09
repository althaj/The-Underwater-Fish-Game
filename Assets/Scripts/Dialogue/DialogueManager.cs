using System.Collections;
using System.Collections.Generic;
using TUFG.UI;
using UnityEngine;

namespace TUFG.Dialogue
{
    /// <summary>
    /// Class responsible for controling dialogues.
    /// </summary>
    /// <remarks>Uses a singleton pattern.</remarks>
    public class DialogueManager : MonoBehaviour
    {
        #region Singleton pattern
        private static DialogueManager _instance;

        /// <summary>
        /// Current instance of the dialogue manager. Creates an Unity object.
        /// </summary>
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

                        DontDestroyOnLoad(container);
                    }
                }

                return _instance;
            }
        }
        #endregion

        private static DialogueNode[] currentConversation = new DialogueNode[0];
        private static int currentNodeID = -1;

        /// <summary>
        /// The current dialogue node, or null if there is no dialogue node.
        /// </summary>
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

        /// <summary>
        /// Display the current dialogue node in UI. If there is no node, hides the dialogue UI.
        /// </summary>
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

        /// <summary>
        /// Start new conversation and display the dialogue in the UI.
        /// </summary>
        /// <param name="conversation">The conversation to begin and display.</param>
        public void InitConversation(DialogueConversation conversation)
        {
            currentConversation = conversation.dialogueNodes;
            currentNodeID = 0;
            DisplayCurrentDialogueNode();

            FindObjectOfType<PlayerMovement>().DisableInput();
        }

        /// <summary>
        /// Go to next dialogue node. If there is another node, it will get displayed, otherwise the conversation will end.
        /// </summary>
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

        /// <summary>
        /// End the current conversation and hide the dialogue UI.
        /// </summary>
        public void EndConversation()
        {
            currentNodeID = -1;
            currentConversation = null;
            DisplayCurrentDialogueNode();

            FindObjectOfType<PlayerMovement>().EnableInput();
        }

        /// <summary>
        /// Does the current conversation have another node?
        /// </summary>
        public bool HasNextNode
        {
            get
            {
                return (currentConversation != null && currentNodeID >= 0 && currentNodeID < currentConversation.Length - 1);
            }
        }

        /// <summary>
        /// Go to a node by ID.
        /// </summary>
        /// <param name="nodeId">ID of the node in the current conversation to open.</param>
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

        /// <summary>
        /// Jump to node by it's position in the array of nodes.
        /// </summary>
        /// <param name="nodeIndex">Index of the position in the current conversation.</param>
        public void JumpToNode(int nodeIndex)
        {
            currentNodeID = nodeIndex;
            DisplayCurrentDialogueNode();
        }
    }
}
