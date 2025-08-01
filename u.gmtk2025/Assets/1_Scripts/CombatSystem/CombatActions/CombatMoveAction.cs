using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;

namespace _1_Scripts.CombatSystem.CombatActions
{
    public class CombatMoveAction : ICombatAction
    {
        public CombatActionType CombatActionType => CombatActionType.Move;
        
        public CombatEntity Caster { get; set; }
        public List<CombatEntity> Targets { get; set; }
    }
}