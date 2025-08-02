using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;

namespace _1_Scripts.CombatSystem.Managers.Turns
{
    public class CombatTurn
    {
        public CombatEntity Caster;
        public BaseCombatAction SelectedAction;
        public List<CombatEntity> SelectedTargets;

        public CombatTurn(CombatEntity caster)
        {
            Caster = caster;
            SelectedTargets = new List<CombatEntity>();
        }
    }
}