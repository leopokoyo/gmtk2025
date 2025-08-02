using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.DamageCalculators;
using _1_Scripts.CombatSystem.Services.Events;
using UnityEngine;

namespace _1_Scripts.CombatSystem.Services
{
    public class CombatService
    {
        public event EventHandler<DamageDealtEventArgs> OnDamageDealt;

        public void ExecuteAction(BaseCombatAction action, List<CombatEntity> targets, CombatEntity caster)
        {
            if (action == null || caster == null || targets == null || targets.Count == 0) return;

            if (action is not BaseCombatAttackAction attackAction) return;

            foreach (var target in targets)
            {
                var context = new DamageCalculationEventArgs
                {
                    Caster = caster,
                    Target = target
                };

                var damage = attackAction.DamageCalculator?.Invoke(context) ?? 0;
                target.TakeDamage(damage);

                OnDamageDealt?.Invoke(this, new DamageDealtEventArgs(caster, target, damage));

                if (attackAction.CombatEffects != null)
                {
                    foreach (var effect in attackAction.CombatEffects)
                    {
                        target.GainEffect(effect.EffectType);
                    }
                }

                Debug.Log($"[CombatService] {caster.name} uses {attackAction.CombatActionName} on {target.name} for {damage} damage.");
            }
        }
    }

}