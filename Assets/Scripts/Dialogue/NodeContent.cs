using System.Collections;
using System.Collections.Generic;
using TUFG.UI;
using UnityEngine;

namespace TUFG.Dialogue
{
    public enum DialogueAvatarPosition {
        Right,
        Left
    }

    public class NodeContent : ScriptableObject
    {
        public string characterName;
        public Sprite avatar;
        public DialogueAvatarPosition avatarPosition;
        public string dialogueText;
        public float textSpeed;
        public AudioClip textSound;
        public Font font;
        public GenericButton[] dialogueButtons;
    }
}
