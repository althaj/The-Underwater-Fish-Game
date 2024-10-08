using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Camera
{
    public class CameraManager : MonoBehaviour
    {
        #region Singleton pattern
        private static CameraManager _instance;

        /// <summary>
        /// Current instance of the camera manager. Destroys on new level load.
        /// </summary>
        public static CameraManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<CameraManager>();
                }

                return _instance;
            }
        }
        #endregion

        private WorldCamera[] cameras;
        private WorldCamera currentCamera;
        private WorldCamera previousCamera = null;

        private Transform mainCamera;
        private Transform target;

        // Start is called before the first frame update.
        void Start()
        {
            // Get all cameras in the scene.
            cameras = FindObjectsOfType<WorldCamera>();
            //Deactivate all the cameras.
            foreach (var camera in cameras)
            {
                camera.Deactivate();
            }

            // Set the first camera.
            if (cameras.Length > 0)
                Transition(cameras[0]);
            else
                Transition(null);

            // Get the main camera.
            mainCamera = UnityEngine.Camera.main.transform;
            // Set the target to the player.
            target = FindObjectOfType<PlayerMovement>().transform;
        }

        // Update is called once per frame
        void Update()
        {
            if(mainCamera != null && currentCamera != null && target != null)
                mainCamera.position = currentCamera.GetCameraPosition(target.position);
        }

        /// <summary>
        /// Set the position of the camera. This sets the target to null.
        /// </summary>
        /// <param name="position">New position of the camera.</param>
        public void SetPosition(Vector3 position)
        {
            mainCamera.position = position;
            SetTarget(null);
        }

        /// <summary>
        /// Set a new target for the camera.
        /// </summary>
        /// <param name="target">New target for the camera.</param>
        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        /// <summary>
        /// Transition to a new screen.
        /// </summary>
        /// <param name="camera">World camera to transition to.</param>
        public void Transition(WorldCamera camera)
        {
            previousCamera = currentCamera;
            currentCamera = camera;

            currentCamera.Activate();

            if (previousCamera != null)
                previousCamera.Deactivate();
        }
    }
}
