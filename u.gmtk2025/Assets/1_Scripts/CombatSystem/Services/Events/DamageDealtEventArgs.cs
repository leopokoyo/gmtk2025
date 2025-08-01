using System;
using _1_Scripts.CombatSystem.CombatEntities;

namespace _1_Scripts.CombatSystem.Services.Events
{
    public class DamageDealtEventArgs : EventArgs
    {
        public CombatEntity affectedEntity;
        public int damageDealt;
    }
}