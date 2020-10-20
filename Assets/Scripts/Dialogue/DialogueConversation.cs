using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Dialogue
{
    /// <summary>
    /// A full conversation.
    /// </summary>
    [CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogues/Conversation")]
    public class DialogueConversation : ScriptableObject
    {
        /// <summary>
        /// ID of the conversation in format conversation_id.
        /// </summary>
        public string conversationID;

        /// <summary>
        /// Nodes of the conversation.
        /// </summary>
        public DialogueNode[] dialogueNodes;
    }
}
