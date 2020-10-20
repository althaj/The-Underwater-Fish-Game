using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TUFG.Battle
{
    /// <summary>
    /// A class containing information about a battle.
    /// </summary>
    public class Battle
    {
        public List<Unit> allies;
        public List<Unit> enemies;
        public bool playerTurn;
        public int currentUnitTurn;
    }
}
