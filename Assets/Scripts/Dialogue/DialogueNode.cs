using System;
using System.Collections;
using System.Collections.Generic;
using TUFG.UI;
using UnityEngine;

namespace TUFG.Dialogue
{
    /// <summary>
    /// A class representing a dialogue node.
    /// </summary>
    [Serializable]
    public class DialogueNode
    {
        /// <summary>
        /// ID of the node in format node_id.
        /// </summary>
        public string nodeID;

        /// <summary>
        /// Name of the character saying the message.
        /// </summary>
        public string characterName;

        /// <summary>
        /// Avatar of the character saying the message.
        /// </summary>
        public Sprite avatar;

        /// <summary>
        /// Position of the avatar in the dialogue panel.
        /// </summary>
        public DialogueAvatarPosition avatarPosition;
        
        /// <summary>
        /// The message that the character is saying.
        /// </summary>
        [Multiline(5)] public string text;

        /// <summary>
        /// Speed of the text appearing in the dialogue panel.
        /// </summary>
        public float textSpeed;

        /// <summary>
        /// Sound the text makes when it appears in the dialogue panel.
        /// </summary>
        public AudioClip textSound;

        /// <summary>
        /// Font used for the text of the message.
        /// </summary>
        public Font font;

        /// <summary>
        /// Array of buttons that are the player choices for this dialogue node.
        /// </summary>
        public GenericButton[] dialogueButtons;

        /// <summary>
        /// Get the content of the node.
        /// </summary>
        /// <returns>Content of the node.</returns>
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

        /// <summary>
        /// Process the current dialogue node.
        /// </summary>
        public virtual void Process() { }
    }
}
