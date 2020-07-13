using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Dialogue
{
    public abstract class DialogueNode : ScriptableObject
    {
        public string nodeID;
        public string characterName;
        public Sprite avatar;
        public DialogueAvatarPosition avatarPosition;
        public DialogueButton[] dialogueButtons;

        public virtual NodeContent GetNodeContent() {
            NodeContent nodeContent = CreateInstance<NodeContent>();
            nodeContent.characterName = characterName;
            nodeContent.avatar = avatar;
            nodeContent.avatarPosition = avatarPosition;
            nodeContent.dialogueButtons = dialogueButtons;
            return nodeContent;
        }
    }
}
