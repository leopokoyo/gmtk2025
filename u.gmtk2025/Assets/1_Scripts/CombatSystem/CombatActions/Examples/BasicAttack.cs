using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.DamageCalculators;
using _1_Scripts.CombatSystem.DamageCalculators.Examples;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatActions.Examples
{

    [CreateAssetMenu(fileName = "Combat Action - Basic Attack", menuName = "ScriptableObjects/Combat/Actions/Attacks/BasicAttack")]
    public class BasicAttack : BaseCombatAttackAction
    {
        [SerializeField] private string actionName = "Basic Attack";
        [SerializeField] private float accuracy = 1.0f;
        [SerializeField] private float critChance = 0.1f;
        [SerializeField] private int range = 1;

        public override string CombatActionName => actionName;
        public override float Accuracy => accuracy;
        public override float CriticalHitChance => critChance;
        public override int Range => range;

        public override DamageCalculationDelegate DamageCalculator =>
            DamageCalculatorSamples.SimpleAttackDamageCalculator;
    }
}