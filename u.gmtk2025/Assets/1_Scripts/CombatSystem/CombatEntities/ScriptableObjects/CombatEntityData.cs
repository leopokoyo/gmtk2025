using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatEntities.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CombatEntities", menuName = "ScriptableObjects/CombatEntities/CombatData", order = 0)]
    public class CombatEntityData : ScriptableObject
    {
        public int Strength;
        public int Agility;
        public int Cunning;
        public int Wisdom;
        public int Willpower;
        public int Intelligence;
        public int MaxHealth;
        
        [SerializeReference] public List<BaseCombatAction> CombatActions;
    }
}