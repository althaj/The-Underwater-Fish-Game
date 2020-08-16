using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.World
{
    public class WorldInfo : MonoBehaviour
    {
        [SerializeField] private string worldName = "";
        [SerializeField] private Transform[] battleArenas = null;

        public Transform GetRandomArena()
        {
            if (battleArenas == null || battleArenas.Length == 0) {
                Debug.LogErrorFormat("Battle arenas not configured for level {0}", worldName);
                return null;
            }

            return battleArenas[Random.Range(0, battleArenas.Length)];
        }
    }
}
