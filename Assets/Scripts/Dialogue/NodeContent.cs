using System.Collections;
using System.Collections.Generic;
using TUFG.UI;
using UnityEngine;

namespace TUFG.Dialogue
{
    /// <summary>
    /// Enumeration of possible avatar positions in dialogue panel.
    /// </summary>
    public enum DialogueAvatarPosition {
        Right,
        Left
    }

    /// <summary>
    /// Content of a dialogue node.
    /// </summary>
    public class NodeContent : ScriptableObject
    {
        /// <summary>
        /// Name of the character saying the message.
        /// </summary>
        public string characterName;

        /// <summary>
        /// Avatar of the character saying the message.
        /// </summary>
        public Sprite avatar;

        /// <summary>
        /// Position of the avatar of the author.
        /// </summary>
        public DialogueAvatarPosition avatarPosition;

        /// <summary>
        /// Text of the message. BB code can be used.
        /// </summary>
        public string dialogueText;

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
    }
}
