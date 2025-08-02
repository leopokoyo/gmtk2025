using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;

namespace _1_Scripts.CombatSystem.Managers.Turns
{
    public class CombatTurnCommand
    {
        public CombatEntity Caster;
        public BaseCombatAction Action;
        public List<CombatEntity> Targets;
    }
}