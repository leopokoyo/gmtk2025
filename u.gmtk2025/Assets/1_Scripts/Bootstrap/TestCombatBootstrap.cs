using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.Events;
using _1_Scripts.CombatSystem.Managers;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using _1_Scripts.CombatSystem.Managers.Enums;

namespace _1_Scripts.Bootstrap
{
    public class TestCombatBootstrap : MonoBehaviour
    {
        public List<CombatEntity> players;
        public List<CombatEntity> enemies;

        private void OnEnable()
        {
            CombatEvents.OnCombatStateChanged += OnCombatStateChanged;
        }

        private void OnDisable()
        {
            CombatEvents.OnCombatStateChanged -= OnCombatStateChanged;
        }

        private void Start()
        {
            CombatEvents.RaiseCombatStart(this, new StartCombatEventArgs(players, enemies));
        }

        private void OnCombatStateChanged(CombatState state)
        {
            switch (state)
            {
                case CombatState.Planning:
                    SimulateActionsForAll();
                    CombatManager.Instance.ProceedToNextPhase();
                    break;

                case CombatState.EndOfCombat:
                    Debug.Log("[TestCombatBootstrap] Combat ended.");
                    break;
            }
        }

        private void SimulateActionsForAll()
        {
            // Players
            foreach (var player in players.Where(p => p.IsAlive))
            {
                var action = player.CombatActions.FirstOrDefault();
                var target = enemies.FirstOrDefault(e => e.IsAlive);
                if (action != null && target != null)
                {
                    CombatManager.Instance.StorePlayerAction(player, action, new List<CombatEntity> { target });
                }
            }

            // Enemies
            foreach (var enemy in enemies.Where(e => e.IsAlive))
            {
                var action = enemy.CombatActions.FirstOrDefault();
                var target = players.FirstOrDefault(p => p.IsAlive);
                if (action != null && target != null)
                {
                    CombatManager.Instance.StorePlayerAction(enemy, action, new List<CombatEntity> { target });
                }
            }
        }
    }
}
