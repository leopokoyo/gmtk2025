using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.DamageCalculators;
using _1_Scripts.CombatSystem.Events;
using _1_Scripts.CombatSystem.Managers;
using _1_Scripts.CombatSystem.Services.Events;
using UnityEngine;

namespace _1_Scripts.CombatSystem.Services.CombatService
{
    public class CombatService
    {
        public event EventHandler<DamageDealtEventArgs> OnDamageDealt;
        private readonly RowSystemService.RowSystemService _rowSystemService = new ();

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

                // Checks if targets are in range
                if (!_rowSystemService.IsInRange(caster, target, attackAction.Range))
                {
                    
                    Debug.LogWarning($"[CombatService] Target {target.name} is out of range for {caster.name}'s attack '{attackAction.CombatActionName}'.");
                    continue;
                }
                Debug.Log($"$[CombatService] Target {target.name} is in range for {caster.name}'s attack '{attackAction.CombatActionName}' with {_rowSystemService.DistanceBetweenEntities(caster, target)}");

                var damage = attackAction.DamageCalculator?.Invoke(context) ?? 0;
                
                target.TakeDamage(damage);
                Debug.Log($"[CombatService] {caster.name} uses {attackAction.CombatActionName} on {target.name} for {damage} damage.");
                
                OnDamageDealt?.Invoke(this, new DamageDealtEventArgs(caster, target, damage));

                if (attackAction.CombatEffects == null) continue;
                foreach (var effect in attackAction.CombatEffects)
                {
                    target.GainEffect(effect.EffectType);
                }
            }
        }

        /// <summary>
        /// For moving (rethink the name
        /// </summary>
        /// <param name="action"></param>
        /// <param name="caster"></param>
        public void ExecuteAction(BaseCombatAction action, CombatEntity caster)
        {
            if (action == null || caster == null) return;
            if (action is not BaseCombatMoveAction moveAction) return;
            var startingRow = caster.RowIndex;
            var targetRowIndex = CombatTools.ClampToValidRow(caster.RowIndex + (moveAction.Direction * moveAction.Distance));
            if (CombatManager.Instance.GetCombatantsInRow(targetRowIndex).Count >= 5) return;
            caster.SetRow(targetRowIndex);
            Debug.Log($"[CombatService] {caster.name} has moved to row {targetRowIndex} from {startingRow}.");
            
        }
    }

}