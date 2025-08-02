using System;
using _1_Scripts.CombatSystem.CombatEntities;
using UnityEngine;

namespace _1_Scripts.CombatSystem.Services.Events
{
    public class DamageDealtEventArgs : EventArgs
    {
        public CombatEntity Target;
        public CombatEntity Caster;
        public int Damage;

        public DamageDealtEventArgs(CombatEntity caster, CombatEntity entity, int damage)
        {
            Caster = caster;
            Target = entity;
            Damage = damage;
            
        }
    }
}