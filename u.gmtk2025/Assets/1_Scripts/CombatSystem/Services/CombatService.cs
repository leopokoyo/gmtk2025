using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.DamageCalculators;
using _1_Scripts.CombatSystem.Services.Events;

namespace _1_Scripts.CombatSystem.Services
{
    public class CombatService
    {
        public event EventHandler<DamageDealtEventArgs> OnDamageDealt;

        public CombatEntity Player { get; private set; }
        public List<CombatEntity> Enemies { get; private set; }

        public CombatService(CombatEntity player, List<CombatEntity> enemies)
        {
            Player = player;
            Enemies = enemies;
        }

        public void ExecuteAction(ICombatAction action)
        {
            foreach (var target in action.Targets)
            {
                // TODO: ADD checks for range, add checks for critical hits, add checks for accuracy
                
                if (action is not CombatAttackAction attackAction) continue;
                var context = new DamageCalculationEventArgs
                {
                    Caster = action.Caster,
                    Target = target
                };

                var damage = attackAction.DamageCalculator?.Invoke(context) ?? 0;
                target.TakeDamage(damage);

                // Apply effects if any
                if (attackAction.CombatEffects == null) continue;
                foreach (var effect in attackAction.CombatEffects)
                {
                    target.GainEffect(effect.EffectType);
                }
            }
        }
    }
}