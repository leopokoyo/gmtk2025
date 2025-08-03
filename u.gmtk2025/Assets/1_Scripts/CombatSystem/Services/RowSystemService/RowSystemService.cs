using _1_Scripts.CombatSystem.CombatEntities;
using UnityEngine;

namespace _1_Scripts.CombatSystem.Services.RowSystemService
{
    public class RowSystemService
    {
        public bool IsInRange(CombatEntity caster, CombatEntity target, int allowedRange)
        {
            var distance = Mathf.Abs(caster.RowIndex - target.RowIndex);
            return distance <= allowedRange;
        }

        public int DistanceBetweenEntities(CombatEntity caster, CombatEntity target) => Mathf.Abs(caster.RowIndex - target.RowIndex);
    }
}