using System;
using _1_Scripts.CombatSystem.CombatEntities;

namespace _1_Scripts.CombatSystem.DamageCalculators
{
    public class DamageCalculationEventArgs : EventArgs
    {
        public CombatEntity Caster;
        public CombatEntity Target;
    }
}