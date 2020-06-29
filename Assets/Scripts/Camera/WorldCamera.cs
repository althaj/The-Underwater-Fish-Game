using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Camera
{
    public class WorldCamera : MonoBehaviour
    {
        private Transform player;
        private Transform target = null;

        [SerializeField]
        private float speed = 4f;

        private void Start()
        {
            player = FindObjectOfType<TUFG.Prototype.PlayerMovement>().transform;
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

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void ClearTarget()
        {
            target = null;
        }
    }
}
