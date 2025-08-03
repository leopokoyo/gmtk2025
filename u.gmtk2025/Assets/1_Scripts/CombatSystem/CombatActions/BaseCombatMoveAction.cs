using System;
using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;

namespace _1_Scripts.CombatSystem.CombatActions
{
    [Serializable]
    public class  BaseCombatMoveAction : BaseCombatAction
    {
        public override CombatActionType CombatActionType => CombatActionType.Move;
        public virtual int Distance { get; set; }
        public virtual int Direction { get; set; }
        public new virtual string CombatActionName { get; private set; }
    }
}