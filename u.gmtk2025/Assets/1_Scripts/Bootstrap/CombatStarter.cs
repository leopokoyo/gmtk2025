using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.Events;
using _1_Scripts.CombatSystem.Managers;
using UnityEngine;

namespace _1_Scripts.Bootstrap
{
    public class CombatStarter : MonoBehaviour
    {
        [SerializeField] private CombatEntity playerPrefab;
        [SerializeField] private CombatEntity enemyPrefab;

        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform enemySpawnPoint;

        private List<CombatEntity> _players = new();
        private List<CombatEntity> _enemies = new();

        private void Start()
        {
            StartTestCombat();
        }

        private void StartTestCombat()
        {
            _players.Clear();
            _enemies.Clear();

            // Spawn 2 test players
            for (var i = 0; i < 2; i++)
            {
                var player = Instantiate(playerPrefab, playerSpawnPoint.position + Vector3.right * i * 2, Quaternion.identity);
                player.name = $"Player_{i}";
                player.SetRow(0);
                _players.Add(player);
            }

            // Spawn 2 test enemies
            for (var i = 0; i < 2; i++)
            {
                var enemy = Instantiate(enemyPrefab, enemySpawnPoint.position + Vector3.right * i * 2, Quaternion.identity);
                enemy.name = $"Enemy_{i}";
                enemy.SetRow(2);
                _enemies.Add(enemy);
            }

            // Start combat via GameManager
            CombatEvents.RaiseCombatStart(this, new StartCombatEventArgs(_players, _enemies));
        }
    }
}