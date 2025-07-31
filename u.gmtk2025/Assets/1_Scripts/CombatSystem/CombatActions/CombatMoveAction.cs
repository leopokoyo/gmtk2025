using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;

namespace _1_Scripts.CombatSystem.CombatActions
{
    public class CombatMoveAction : ICombatAction
    {
        public CombatActionType CombatActionType => CombatActionType.Move;
    }
}