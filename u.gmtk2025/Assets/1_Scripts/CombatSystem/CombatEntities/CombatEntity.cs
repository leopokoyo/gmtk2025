using System;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities.ScriptableObjects;
using _1_Scripts.CombatSystem.Effects.Enums;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatEntities
{
    public class CombatEntity : MonoBehaviour
    {
        
        public CombatEntityData _data;
        
        public int Strength => _data.Strength;
        public int Agility => _data.Agility;
        public int Cunning =>  _data.Cunning;
        public int Wisdom => _data.Wisdom;
        public int Willpower => _data.Willpower;
        public int Intelligence => _data.Intelligence;
        private int MaxHealth => _data.MaxHealth;
        
        public List<BaseCombatAction> CombatActions => _data.CombatActions;
        public int CurrentHealth;
        public List<CombatEffectTypes> CurrentEffects;
        
        public bool IsAlive => CurrentHealth > 0;

        public void Initialize(List<CombatEffectTypes> currentEffects = null)
        {
            CurrentEffects = currentEffects ?? new List<CombatEffectTypes>();
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }

        public void GainEffect(CombatEffectTypes effects)
        {
            if (!CurrentEffects.Contains(effects)) CurrentEffects.Add(effects);
        }

        /// <summary>
        /// Try to remove the desired effect (only occurs if effect is present)
        /// </summary>
        /// <param name="effects"></param>
        public void RemoveEffect(CombatEffectTypes effects)
        {
            if (CurrentEffects.Contains(effects)) CurrentEffects.Remove(effects);
        }
    }
}