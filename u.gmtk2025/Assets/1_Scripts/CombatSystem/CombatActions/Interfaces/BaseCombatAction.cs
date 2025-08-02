using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.CombatEntities;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatActions.Interfaces
{
    [Serializable]
    public abstract class BaseCombatAction
    {
        [SerializeField]
        public CombatActionType CombatActionType { get; }
        
        [SerializeField]
        public  string CombatActionName { get; }
    }
}