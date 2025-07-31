using System;

namespace _1_Scripts.Combat.Damage.Events
{
    public class DamageDealtEventArgs: EventArgs
    {
        public CombatEntity Caster;
        public CombatEntity Target;
        public int DamageAmount;
    }
}