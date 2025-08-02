using _1_Scripts.CombatSystem.Effects.Enums;

namespace _1_Scripts.CombatSystem.CombatEffects.Interfaces
{
    public interface ICombatEffects
    {
        public CombatEffectTypes EffectType { get; }
        public CombatEffectAffectTypes AffectType { get; }
    }
}