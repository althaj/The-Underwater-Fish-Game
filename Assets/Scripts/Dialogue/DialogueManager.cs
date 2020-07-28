﻿using System.Collections;
using System.Collections.Generic;
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
                    Debug.LogError("CurrentDialogueNode: Current conversation is null!");
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

        public static void DisplayCurrentDialogueNode()
        {
            DialogueNode currentNode = CurrentDialogueNode;
            if (currentNode == null)
                return;

            NodeContent content = currentNode.GetNodeContent();
            FindObjectOfType<TUFG.UI.DialogueContainer>().ShowMessage(content.characterName, content.dialogueText, content.avatar, content.dialogueButtons, content.avatarPosition);
        }

        public static void InitConversation(DialogueConversation conversation)
        {
            currentConversation = conversation.dialogueNodes;
            currentNodeID = 0;
            DisplayCurrentDialogueNode();
        }

        public static void GoToNextNode()
        {
            if (HasNextNode)
            {
                currentNodeID++;
                CurrentDialogueNode.Process();
                DisplayCurrentDialogueNode();
            } else
            {
                currentNodeID = -1;
                currentConversation = null;
            }
        }

        public static bool HasNextNode
        {
            get
            {
                return (currentConversation != null && currentNodeID >= 0 && currentNodeID < currentConversation.Length - 1);
            }
        }
    }
}
