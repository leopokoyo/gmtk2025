using _1_Scripts.CombatSystem.DamageCalculators;
using _1_Scripts.CombatSystem.DamageCalculators.Examples;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatActions.Examples
{
    public class BasicAttack : BaseCombatAttackAction
    {
        [SerializeField] private string _actionName = "Basic Attack";
        [SerializeField] private float _accuracy = 1;
        [SerializeField] private float _critChance = 0.1f;
        [SerializeField] private int _range = 1;

        public override string CombatActionName => _actionName;
        public override float Accuracy => _accuracy;
        public override float CriticalHitChance => _critChance;
        public override int Range => _range;

        public override DamageCalculationDelegate DamageCalculator =>
            DamageCalculatorSamples.SimpleAttackDamageCalculator;
    }
}