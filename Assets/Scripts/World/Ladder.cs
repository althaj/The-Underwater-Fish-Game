using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.World
{
    public enum LadderType
    {
        Top,
        Bottom,
        Any
    }

    public class Ladder : MonoBehaviour
    {
        [Range(1, 25)]
        public int height = 1;

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0.75f, 0.75f, 0.25f);
            Gizmos.DrawCube(transform.position + Vector3.up * height / 2, new Vector3(1, height, 0));
            Gizmos.color = new Color(0, 0.75f, 0.75f, 0.75f);
            Gizmos.DrawWireCube(transform.position + Vector3.up * height / 2, new Vector3(1, height, 0));
        }
    }
}
