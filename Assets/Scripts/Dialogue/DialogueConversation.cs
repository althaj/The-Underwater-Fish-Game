using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Dialogue
{
    [CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogues/Conversation")]
    public class DialogueConversation : ScriptableObject
    {
        public string conversationID;
        public DialogueNode[] dialogueNodes;
    }
}
