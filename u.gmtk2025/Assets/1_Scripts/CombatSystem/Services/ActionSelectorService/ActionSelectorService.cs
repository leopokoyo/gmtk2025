using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.Events;
using _1_Scripts.CombatSystem.Managers;

namespace _1_Scripts.CombatSystem.Services.ActionSelectorService
{
    public class ActionSelectorService
    {
        private readonly Queue<CombatEntity> _pendingPlayers = new();
        private CombatEntity _currentPlayer;

        public CombatEntity CurrentPlayer => _currentPlayer;

        public void StartPlayerSelection(List<CombatEntity> players)
        {
            _pendingPlayers.Clear();

            foreach (var player in players)
            {
                if (player.IsAlive)
                {
                    _pendingPlayers.Enqueue(player);
                }
            }

            SelectNextPlayer();
        }

        /// <summary>
        /// Should be called (by input/UI or test logic) when the player submits an action and target(s).
        /// </summary>
        public void SubmitPlayerAction(BaseCombatAction action, List<CombatEntity> targets)
        {
            if (_currentPlayer == null || action == null || targets == null || targets.Count == 0)
                return;

            CombatManager.Instance.StorePlayerAction(_currentPlayer, action, targets);
            SelectNextPlayer();
        }

        private void SelectNextPlayer()
        {
            if (_pendingPlayers.Count == 0)
            {
                _currentPlayer = null;

                // All players have submitted actions
                // Let the CombatManager decide when to move on
                return;
            }

            _currentPlayer = _pendingPlayers.Dequeue();

            // Notify systems (e.g. UI or AI) that this player needs an action
            CombatEvents.RaisePlayerActionRequested(_currentPlayer);
        }
    }
}