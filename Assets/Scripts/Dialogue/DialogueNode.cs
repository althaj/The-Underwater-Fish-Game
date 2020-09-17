using System;
using System.Collections;
using System.Collections.Generic;
using TUFG.UI;
using UnityEngine;

namespace TUFG.Dialogue
{
    [Serializable]
    public class DialogueNode
    {
        public string nodeID;
        public string characterName;
        public Sprite avatar;
        public DialogueAvatarPosition avatarPosition;
        
        [Multiline(5)] public string text;
        public float textSpeed;
        public AudioClip textSound;
        public Font font;

        public Button[] dialogueButtons;

        public virtual NodeContent GetNodeContent() {
            NodeContent nodeContent = ScriptableObject.CreateInstance<NodeContent>();
            nodeContent.characterName = characterName;
            nodeContent.avatar = avatar;
            nodeContent.avatarPosition = avatarPosition;
            nodeContent.dialogueButtons = dialogueButtons;
            nodeContent.dialogueText = text;
            nodeContent.textSpeed = textSpeed;
            nodeContent.textSound = textSound;
            nodeContent.font = font;
            return nodeContent;
        }

        public virtual void Process() { }
    }
}
