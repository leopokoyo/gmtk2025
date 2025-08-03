using _1_Scripts.CombatSystem.CombatActions.Enums;
using UnityEngine;

namespace _1_Scripts.CombatSystem.CombatActions.Examples
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Combat/Actions/Move")]
    public class BasicMoveCombatAction : BaseCombatMoveAction
    {
        [SerializeField] private string actionName = "Move";
        [SerializeField] private int distance = 1;

        public override string CombatActionName => actionName;
        public new int Distance => distance;

        public override CombatActionType CombatActionType => CombatActionType.Move;
    }
}