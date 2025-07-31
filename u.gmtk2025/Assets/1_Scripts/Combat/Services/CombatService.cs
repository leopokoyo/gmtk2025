using System;
using System.Collections.Generic;
using _1_Scripts.Combat.Damage;
using _1_Scripts.Combat.Damage.Events;
using _1_Scripts.Combat.Effects.Interface;
using _1_Scripts.Combat.Enums;
using _1_Scripts.ScriptableObjects;
using UnityEngine;

namespace _1_Scripts.Combat.Services
{
    public class CombatService
    {
        public event EventHandler<DamageDealtEventArgs> OnDamageDealt;

        public List<DamageResult> PerformAttack(
            CombatEntity caster,
            List<CombatEntity> targets,
            List<ICombatEffect> additionalEffects,
            DamageCalculationDelegateWrapper damageFunction)
        {
            foreach (var target in targets)
            {
                var context = new AttackContext
                {
                    Caster = caster,
                    Target = target,
                    AdditionalEffects = additionalEffects
                };

                var damage = damageFunction.CalculateDamage(context);
                target.CurrentHealth -= damage;

                Debug.Log(
                    $"{caster.Name} dealt {damage} damage to {target.Name}. {target.Name} has {target.CurrentHealth} HP left.");

                // 🔥 Dispatch event
                OnDamageDealt?.Invoke(this, new DamageDealtEventArgs
                {
                    Caster = caster,
                    Target = target,
                    DamageAmount = damage
                });
            }
        }
    }
}