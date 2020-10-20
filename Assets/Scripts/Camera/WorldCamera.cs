using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Camera
{
    /// <summary>
    /// A camera controller for the world.
    /// </summary>
    public class WorldCamera : MonoBehaviour
    {
        private Transform player;
        private Transform target = null;

        [SerializeField]
        private float speed = 4f;

        private void Start()
        {
            player = FindObjectOfType<TUFG.PlayerMovement>().transform;
            if (player == null)
                Debug.LogError("WorldCamera: No player found!!");
        }

        private void Update()
        {
            Transform realTarget = player;

            if(target != null)
                realTarget = target;

            if (realTarget != null)
                transform.position = Vector3.Lerp(transform.position, new Vector3(realTarget.position.x, realTarget.position.y, transform.position.z), speed * Time.deltaTime);
        }

        /// <summary>
        /// Set the target of the camera to follow.
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        /// <summary>
        /// Clear the current target of the camera.
        /// </summary>
        public void ClearTarget()
        {
            target = null;
        }

        /// <summary>
        /// Set the current position of the camera.
        /// </summary>
        /// <remarks>Note that the camera will still folow it's target if it has one.</remarks>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }
    }
}
