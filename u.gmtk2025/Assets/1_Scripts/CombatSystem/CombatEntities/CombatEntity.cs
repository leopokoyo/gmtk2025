using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities.ScriptableObjects;
using _1_Scripts.CombatSystem.Effects.Enums;
using _1_Scripts.CombatSystem.Effects.Interfaces;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatEntities
{
    public class CombatEntity : MonoBehaviour
    {
        [SerializeField]
        private CombatEntityData _data { get; set; }
        
        public int Strength => _data.Strength;
        public int Agility => _data.Agility;
        public int Cunning =>  _data.Cunning;
        public int Wisdom => _data.Wisdom;
        public int Willpower => _data.Willpower;
        public int Intelligence => _data.Intelligence;
        private int MaxHealth => _data.MaxHealth;
        
        public List<ICombatAction> CombatActions => _data.CombatActions;
        public int CurrentHealth;
        public List<CombatEffectTypes> CurrentEffects;

        public void Initialize(List<CombatEffectTypes> currentEffects)
        {
            CurrentEffects = currentEffects ?? new List<CombatEffectTypes>();
            CurrentHealth = MaxHealth;
        }

    }
}