using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatEntities;

namespace _1_Scripts.CombatSystem.Events
{
    public class StartCombatEventArgs : EventArgs
    {
        public List<CombatEntity> PlayerParty;
        public List<CombatEntity> EnemiesInCombat;
        
        public StartCombatEventArgs(List<CombatEntity> playerParty, List<CombatEntity> enemies)
        {
            PlayerParty = playerParty ?? new List<CombatEntity>();
            EnemiesInCombat = enemies ?? new List<CombatEntity>();
        }
    }
}