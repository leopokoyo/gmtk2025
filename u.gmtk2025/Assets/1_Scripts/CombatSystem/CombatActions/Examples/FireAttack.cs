
using _1_Scripts.CombatSystem.DamageCalculators;
using _1_Scripts.CombatSystem.DamageCalculators.Examples;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatActions.Examples
{
    public class FireAttack : BaseCombatAttackAction
    {
        [SerializeField] private string _actionName = "FireAttack";
        [SerializeField] private float _accuracy = 0.7f;
        [SerializeField] private float _critChance = 0.5f;
        [SerializeField] private int _range = 4;

        public override string CombatActionName => _actionName;
        public override float Accuracy => _accuracy;
        public override float CriticalHitChance => _critChance;
        public override int Range => _range;

        public override DamageCalculationDelegate DamageCalculator =>
            DamageCalculatorSamples.FireballAttackDamageCalculator;
    }
}