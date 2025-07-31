using System.Collections.Generic;
using _1_Scripts.Combat.Damage;
using _1_Scripts.Combat.Enums;

namespace _1_Scripts.Combat.Interfaces
{
    public interface ICombatAttack
    {
        string Name { get; }
        List<DamageResult> Execute(CombatEntity caster, List<CombatEntity> targets);
    }
}
