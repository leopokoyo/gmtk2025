using _1_Scripts.CombatSystem.Events;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatActions.Examples
{
    [CreateAssetMenu(fileName = "Basic Move Backwards", menuName = "ScriptableObjects/Combat/Actions/BasicMoveBackward")]
    public class MoveBackwardsCombatAction : BaseCombatMoveAction
    {
        public override string CombatActionName => "Basic Move Backwards";
        public override int Distance => 1;
        public override int Direction => CombatConstants.Backward;
    }
}