using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.CombatEntities;

namespace _1_Scripts.CombatSystem.CombatActions.Interfaces
{
    [Serializable]
    public abstract class BaseCombatAction
    {
        public CombatActionType CombatActionType { get; }
        public  string CombatActionName { get; }
    }
}