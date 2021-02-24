using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace TUFG.Camera
{
    /// <summary>
    /// A camera controller for the world.
    /// </summary>
    public class WorldCamera : MonoBehaviour
    {
        [SerializeField] private Vector2 bounds = Vector2.zero;

        /// <summary>
        /// Get a camera position within bounds of the world camera.
        /// </summary>
        /// <param name="targetPosition">Position of an object you want to view.</param>
        /// <returns></returns>
        public Vector2 GetCameraPosition (Vector2 targetPosition)
        {
            Vector2 result = Vector2.zero;

            result.x = Mathf.Clamp(targetPosition.x, transform.position.x - bounds.x, transform.position.x + bounds.x);
            result.y = Mathf.Clamp(targetPosition.y, transform.position.y - bounds.y, transform.position.y + bounds.y);

            return result;
        }

        /// <summary>
        /// Draw helper gizmos when the camera object is selected.
        /// </summary>
        void OnDrawGizmosSelected()
        {
            PixelPerfectCamera pixelPerfect = FindObjectOfType<PixelPerfectCamera>();
            float pixelsPerUnit = pixelPerfect.assetsPPU / 2;
            Vector2 screenSize = new Vector2(pixelPerfect.refResolutionX / pixelsPerUnit, pixelPerfect.refResolutionY / pixelsPerUnit);

            Color drawColor = Color.cyan;

            // Draw the camera frame.
            drawColor.a = 0.3f;
            Gizmos.color = drawColor;
            Gizmos.DrawCube(transform.position, new Vector3(screenSize.x / 2, screenSize.y / 2, 0));

            // Draw the camera bounds.
            drawColor.a = 0.1f;
            Gizmos.color = drawColor;
            Gizmos.DrawCube(transform.position, new Vector3(screenSize.x / 2 + bounds.x / 2, screenSize.y / 2 + bounds.y / 2, 0));
        }

        /// <summary>
        /// Draw helper gizmos.
        /// </summary>
        void OnDrawGizmos()
        {
            PixelPerfectCamera pixelPerfect = FindObjectOfType<PixelPerfectCamera>();
            float pixelsPerUnit = pixelPerfect.assetsPPU / 2;
            Vector2 screenSize = new Vector2(pixelPerfect.refResolutionX / pixelsPerUnit, pixelPerfect.refResolutionY / pixelsPerUnit);

            Color drawColor = Color.cyan;

            // Draw the camera frame.
            drawColor.a = 0.3f;
            Gizmos.color = drawColor;
            Gizmos.DrawWireCube(transform.position, new Vector3(screenSize.x / 2, screenSize.y / 2, 0));

            // Draw the camera bounds.
            drawColor.a = 0.1f;
            Gizmos.color = drawColor;
            Gizmos.DrawWireCube(transform.position, new Vector3(screenSize.x / 2 + bounds.x / 2, screenSize.y / 2 + bounds.y / 2, 0));
        }
    }
}
