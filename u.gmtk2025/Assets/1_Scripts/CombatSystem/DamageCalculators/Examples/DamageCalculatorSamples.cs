using System;

namespace _1_Scripts.CombatSystem.DamageCalculators.Examples
{
    public static class DamageCalculatorSamples
    {
        public static int SimpleAttackDamageCalculator(DamageCalculationEventArgs context)
        {
            var baseDamage = context.Caster.Strength;
            var defense = context.Target.Willpower;
            
            return Math.Max(0, baseDamage - defense);
        }
        
        public static int FireballAttackDamageCalculator(DamageCalculationEventArgs context)
        {
            var baseDamage = context.Caster.Wisdom + context.Caster.Intelligence;
            var defense = context.Target.Willpower;
            
            return Math.Max(0, baseDamage - defense);
        }
    }
}