using System.Collections.Generic;
using System.Linq;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.Events;
using _1_Scripts.CombatSystem.Managers.Enums;
using _1_Scripts.CombatSystem.Managers.Turns;
using _1_Scripts.CombatSystem.Services;
using UnityEngine;

namespace _1_Scripts.CombatSystem.Managers
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }

        private CombatService _combatService;

        private List<CombatEntity> _players = new();
        private List<CombatEntity> _enemies = new();
        private List<CombatEntity> _allCombatants = new();

        private CombatState _state = CombatState.OutOfCombat;

        public CombatState State
        {
            get => _state;
            private set
            {
                if (_state == value) return;
                _state = value;
                Debug.Log($"[CombatManager] State changed to: {_state}");
                CombatEvents.RaiseCombatStateChanged(_state);
            }
        }

        private Dictionary<CombatEntity, CombatTurn> _turnQueue = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _combatService = new CombatService();

            CombatEvents.OnCombatStart += OnCombatStartHandler;

            _combatService.OnDamageDealt += (sender, args) =>
            {
                Debug.Log($"Damage dealt: {args.Damage} from {args.Caster.name} to {args.Target.name}");

                if (!args.Target.IsAlive)
                {
                    Debug.Log($"[CombatManager] {args.Target.name} has died.");
                    CombatEvents.RaiseCombatEntityDied(args.Target, 
                        _players.Count(p => p.IsAlive), 
                        _enemies.Count(e => e.IsAlive));
                }
            };
        }

        private void OnDestroy()
        {
            CombatEvents.OnCombatStart -= OnCombatStartHandler;
        }

        private void OnCombatStartHandler(object sender, StartCombatEventArgs e)
        {
            Debug.Log("[CombatManager] Combat started");

            _players = e.PlayerParty;
            _enemies = e.EnemiesInCombat;

            _allCombatants.Clear();
            _allCombatants.AddRange(_players);
            _allCombatants.AddRange(_enemies);

            foreach (var entity in _allCombatants)
            {
                entity.Initialize();
            }

            _turnQueue.Clear();
            foreach (var entity in _allCombatants)
            {
                _turnQueue[entity] = new CombatTurn(entity);
            }

            State = CombatState.Planning;
        }

        public void ProceedToNextPhase()
        {
            switch (State)
            {
                case CombatState.Planning:
                    State = CombatState.Resolving;
                    ResolveRound();
                    break;

                case CombatState.Resolving:
                    State = CombatState.EndOfTurn;
                    EndTurn();
                    break;

                case CombatState.EndOfTurn:
                    bool playersAlive = _players.Any(p => p.IsAlive);
                    bool enemiesAlive = _enemies.Any(e => e.IsAlive);

                    if (!playersAlive || !enemiesAlive)
                    {
                        HandleCombatEnd();
                    }
                    else
                    {
                        ClearTurnQueue();
                        State = CombatState.Planning;
                    }
                    break;
            }
        }

        private void HandleCombatEnd()
        {
            Debug.Log("[CombatManager] Combat has ended.");
            CombatEvents.RaiseCombatEnd(_players.Count(p => p.IsAlive), _enemies.Count(e => e.IsAlive));

            // Optional cleanup logic here...
            State = CombatState.OutOfCombat;
        }

        private void ClearTurnQueue()
        {
            foreach (var turn in _turnQueue.Values)
            {
                turn.SelectedAction = null;
                turn.SelectedTargets = null;
            }
        }

        private void EndTurn()
        {
            Debug.Log("[CombatManager] Turn Ended.");
            ProceedToNextPhase();
        }

        public void StorePlayerAction(CombatEntity caster, BaseCombatAction action, List<CombatEntity> targets)
        {
            if (!_turnQueue.TryGetValue(caster, out var value))
            {
                Debug.LogWarning($"[CombatManager] StorePlayerAction: Caster {caster.name} not found in turn queue.");
                return;
            }

            value.SelectedAction = action;
            value.SelectedTargets = targets;
            Debug.Log($"[CombatManager] Stored action '{action.CombatActionName}' for {caster.name}");
        }

        public void ResolveRound()
        {
            Debug.Log("[CombatManager] Resolving round...");

            var orderedTurns = _turnQueue.Values
                .Where(t => t.SelectedAction != null && t.Caster.IsAlive)
                .OrderByDescending(t => t.Caster.Agility)
                .ToList();

            foreach (var turn in orderedTurns)
            {
                _combatService.ExecuteAction(turn.SelectedAction, turn.SelectedTargets, turn.Caster);
            }

            Debug.Log("[CombatManager] Round complete.");
            ProceedToNextPhase();
        }
    }
}
