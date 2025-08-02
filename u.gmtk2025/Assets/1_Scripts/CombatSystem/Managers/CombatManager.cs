using System.Collections.Generic;
using System.Linq;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.Events;
using _1_Scripts.CombatSystem.Managers.Turns;
using _1_Scripts.CombatSystem.Services;
using UnityEngine;

namespace _1_Scripts.CombatSystem.Managers
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }

        private CombatService combatService;

        private List<CombatEntity> players = new();
        private List<CombatEntity> enemies = new();
        private List<CombatEntity> allCombatants = new();

        // Store selected actions and targets per combatant
        private Dictionary<CombatEntity, CombatTurn> turnQueue = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            combatService = new CombatService();

            CombatEvents.OnCombatStart += OnCombatStartHandler;

            combatService.OnDamageDealt += (sender, args) =>
            {
                Debug.Log($"Damage dealt: {args.Damage} from {args.Caster.name} to {args.Target.name}");
            };
        }

        private void OnDestroy()
        {
            CombatEvents.OnCombatStart -= OnCombatStartHandler;
        }

        private void OnCombatStartHandler(object sender, StartCombatEventArgs e)
        {
            Debug.Log("[CombatManager] Combat started");

            players = e.PlayerParty;
            enemies = e.EnemiesInCombat;

            allCombatants.Clear();
            allCombatants.AddRange(players);
            allCombatants.AddRange(enemies);

            foreach (var entity in allCombatants)
            {
                entity.Initialize();
            }

            turnQueue.Clear();

            // Initialize turnQueue with empty turns for each combatant
            foreach (var entity in allCombatants)
            {
                turnQueue[entity] = new CombatTurn(entity);
            }
        }

        /// <summary>
        /// Called externally to assign a chosen action and targets for a given combatant.
        /// </summary>
        public void StorePlayerAction(CombatEntity caster, BaseCombatAction action, List<CombatEntity> targets)
        {
            if (!turnQueue.ContainsKey(caster))
            {
                Debug.LogWarning($"[CombatManager] StorePlayerAction: Caster {caster.name} not found in turn queue.");
                return;
            }

            turnQueue[caster].SelectedAction = action;
            turnQueue[caster].SelectedTargets = targets;
            Debug.Log($"[CombatManager] Stored action '{action.CombatActionName}' for {caster.name}");
        }

        /// <summary>
        /// After all players and enemies have stored their actions, call this to resolve the round.
        /// </summary>
        public void ResolveRound()
        {
            Debug.Log("[CombatManager] Resolving round...");

            // Sort combatants by agility descending to determine action order
            var orderedTurns = turnQueue.Values
                .Where(t => t.SelectedAction != null && t.Caster.IsAlive)
                .OrderByDescending(t => t.Caster.Agility)
                .ToList();

            foreach (var turn in orderedTurns)
            {
                combatService.ExecuteAction(turn.SelectedAction, turn.SelectedTargets, turn.Caster);
            }

            Debug.Log("[CombatManager] Round complete.");
        }
    }
}
