using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Dialogue
{
    [CreateAssetMenu(fileName = "New Text Node", menuName = "Dialogues/Nodes/Text Node")]
    public class DialogueNodeText : DialogueNode
    {
        [Multiline(5)] public string text;
        public float textSpeed;
        public AudioClip textSound;
        public Font font;

        public override NodeContent GetNodeContent()
        {
            NodeContent nodeContent = base.GetNodeContent();
            nodeContent.dialogueText = text;
            nodeContent.textSpeed = textSpeed;
            nodeContent.textSound = textSound;
            nodeContent.font = font;
            return nodeContent;
        }
    }
}
