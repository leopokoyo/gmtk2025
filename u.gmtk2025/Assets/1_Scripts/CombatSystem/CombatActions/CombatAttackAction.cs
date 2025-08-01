using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.DamageCalculators;
using _1_Scripts.CombatSystem.Effects.Interfaces;

namespace _1_Scripts.CombatSystem.CombatActions
{
    public class CombatAttackAction : ICombatAction
    {
        /// <summary>
        /// Context Needed to Complete Combat Attack Actions
        /// </summary>
        public DamageCalculationDelegate DamageCalculator { get; set; }
        public float Accuracy { get; set; }
        public float CriticalHitChance { get; set; }
        public int Range { get; set; }
        public List<ICombatEffects> CombatEffects { get; set; }

        public CombatActionType CombatActionType => CombatActionType.Attack;

        public CombatEntity Caster { get; set; }
        public List<CombatEntity> Targets { get; set; }
    }
}