using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.World
{
    /// <summary>
    /// Type of the ladder depending on how you climb it.
    /// </summary>
    public enum LadderType
    {
        Top,
        Bottom,
        Any
    }

    /// <summary>
    /// Class representing a ladder in the world.
    /// </summary>
    public class Ladder : MonoBehaviour
    {
        [Range(1, 25)]
        [SerializeField]
        private int height = 1;

        /// <summary>
        /// Height of the ladder.
        /// </summary>
        public int Height { get => height; set => height = value; }

        /// <summary>
        /// Draw gizmo representing the ladder.
        /// </summary>
        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0.75f, 0.75f, 0.25f);
            Gizmos.DrawCube(transform.position + Vector3.up * Height / 2, new Vector3(1, Height, 0));
            Gizmos.color = new Color(0, 0.75f, 0.75f, 0.75f);
            Gizmos.DrawWireCube(transform.position + Vector3.up * Height / 2, new Vector3(1, Height, 0));
        }
    }
}
