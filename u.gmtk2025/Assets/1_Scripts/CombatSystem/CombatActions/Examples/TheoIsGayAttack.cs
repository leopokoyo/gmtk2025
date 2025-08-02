using _1_Scripts.CombatSystem.DamageCalculators.Examples;

namespace _1_Scripts.CombatSystem.CombatActions.Examples
{
    public class TheoIsGayAttack : BaseCombatAttackAction
    {
        public TheoIsGayAttack()
        {
            CombatActionName = "Theo Is Gay!";
            Accuracy = 1;
            Range = 4;
            // CombatEffects.Add( {CombatEffectTypes = CombatEffectTypes.Burning, CombatEffects});
            DamageCalculator = DamageCalculatorSamples.SimpleAttackDamageCalculator;
            
        }
    }
    
}