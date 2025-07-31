using _1_Scripts.CombatSystem.CombatActions.Enums;

namespace _1_Scripts.CombatSystem.CombatActions.Interfaces
{
    public interface ICombatAction
    {
        public CombatActionType CombatActionType { get; }
    }
}