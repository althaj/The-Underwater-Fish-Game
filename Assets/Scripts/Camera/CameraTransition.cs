using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Camera
{
    public class CameraTransition : MonoBehaviour
    {
        [SerializeField] private WorldCamera transitionCamera;

        /// <summary>
        /// Handle player transitioning to another screen.
        /// </summary>
        /// <param name="col"></param>
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<PlayerMovement>() != null)
            {
                CameraManager.Instance.Transition(transitionCamera);
            }
        }
    } 
}
