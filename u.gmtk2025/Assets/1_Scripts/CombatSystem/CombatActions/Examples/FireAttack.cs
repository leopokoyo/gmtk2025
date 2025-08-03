
using _1_Scripts.CombatSystem.DamageCalculators;
using _1_Scripts.CombatSystem.DamageCalculators.Examples;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatActions.Examples
{
    public class FireAttack : BaseCombatAttackAction
    {
        [SerializeField] private string actionName = "FireAttack";
        [SerializeField] private float accuracy = 0.7f;
        [SerializeField] private float critChance = 0.5f;
        [SerializeField] private int range = 4;

        public override string CombatActionName => actionName;
        public override float Accuracy => accuracy;
        public override float CriticalHitChance => critChance;
        public override int Range => range;

        public override DamageCalculationDelegate DamageCalculator =>
            DamageCalculatorSamples.FireballAttackDamageCalculator;
    }
}