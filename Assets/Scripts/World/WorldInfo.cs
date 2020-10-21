using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.World
{
    /// <summary>
    /// Class containing information about current location.
    /// </summary>
    public class WorldInfo : MonoBehaviour
    {
        [SerializeField] private string worldName = "";
        [SerializeField] private Transform[] battleArenas = null;

        /// <summary>
        /// Get random battle arena to initialize a battle.
        /// </summary>
        /// <returns>Transofrm of the battle arena.</returns>
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
