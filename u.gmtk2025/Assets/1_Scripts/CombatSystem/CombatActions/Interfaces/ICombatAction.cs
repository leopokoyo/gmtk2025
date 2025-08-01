using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.CombatEntities;

namespace _1_Scripts.CombatSystem.CombatActions.Interfaces
{
    public interface ICombatAction
    {
        public CombatActionType CombatActionType { get; }
        public CombatEntity Caster { get; set; }
        public List<CombatEntity> Targets { get; set; }
    }
}