using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEffects.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.DamageCalculators;

namespace _1_Scripts.CombatSystem.CombatActions
{
    [Serializable]
    public class BaseCombatAttackAction : BaseCombatAction
    {
        /// <summary>
        /// Context Needed to Complete Combat Attack Actions
        /// </summary>
        public virtual DamageCalculationDelegate DamageCalculator { get; set; }
        public virtual float Accuracy { get; set; }
        public virtual float CriticalHitChance { get; set; }
        public virtual int Range { get; set; }
        public List<ICombatEffects> CombatEffects { get; set; }
        public override CombatActionType CombatActionType => CombatActionType.Attack;
    }
}