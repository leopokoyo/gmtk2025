using System;
using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;

namespace _1_Scripts.CombatSystem.CombatActions
{
    [Serializable]
    public class BaseCombatMoveAction : BaseCombatAction
    {
        public CombatActionType CombatActionType => CombatActionType.Move;
        public string CombatActionName { get; private set; }
    }
}