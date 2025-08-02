using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.Events;
using _1_Scripts.CombatSystem.Managers;

namespace _1_Scripts.Bootstrap
{
    using UnityEngine;
    using System.Collections.Generic;

    public class TestCombatBootstrap : MonoBehaviour
    {
        public List<CombatEntity> players;
        public List<CombatEntity> enemies;

        private void Start()
        {
            // Raise start combat event
            CombatEvents.RaiseCombatStart(this, new StartCombatEventArgs(players, enemies));

            // Example: assign actions and targets for each player
            foreach (var player in players)
            {
                var action = player.CombatActions[0];
                var target = enemies.Find(e => e.IsAlive);
                CombatManager.Instance.StorePlayerAction(player, action, new List<CombatEntity> { target });
            }

            // For enemies: pick random action and target (for now)
            foreach (var enemy in enemies)
            {
                var action = enemy.CombatActions[UnityEngine.Random.Range(0, enemy.CombatActions.Count)];
                var target = players.Find(p => p.IsAlive);
                CombatManager.Instance.StorePlayerAction(enemy, action, new List<CombatEntity> { target });
            }

            // Now resolve the round once all actions are stored
            CombatManager.Instance.ResolveRound();
        }
    }
}