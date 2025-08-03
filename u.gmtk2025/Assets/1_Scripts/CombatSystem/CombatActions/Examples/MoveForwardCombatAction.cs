using _1_Scripts.CombatSystem.Events;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatActions.Examples
{
    [CreateAssetMenu(fileName = "Basic Move Forward", menuName = "ScriptableObjects/Combat/Actions/BasicMoveForward")]
    public class MoveForwardCombatAction : BaseCombatMoveAction
    {
        public override string CombatActionName => "Basic Move Forward";
        public override int Distance => 1;
        public override int Direction => CombatConstants.Forward;
    }
}