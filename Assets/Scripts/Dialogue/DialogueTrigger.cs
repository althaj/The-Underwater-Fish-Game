using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Dialogue
{
    [RequireComponent(typeof(Collider2D))]
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueConversation conversation = null;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerMovement>() != null)
            {
                DialogueManager.InitConversation(conversation);
            }
        }
    }
}
