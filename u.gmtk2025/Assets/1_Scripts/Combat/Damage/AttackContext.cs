using System.Collections.Generic;
using _1_Scripts.Combat.Effects.Interface;
using _1_Scripts.Combat.ScriptableObjects;
using _1_Scripts.ScriptableObjects;

namespace _1_Scripts.Combat.Damage
{
    public class AttackContext
    {
        public CombatEntity Caster;
        public CombatEntity Target;
        
        public List<ICombatEffect> AdditionalEffects;
    }
}