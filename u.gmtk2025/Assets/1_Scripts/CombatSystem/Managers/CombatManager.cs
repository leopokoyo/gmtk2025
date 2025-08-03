using System;
using System.Collections.Generic;
using System.Linq;
using _1_Scripts.CombatSystem.CombatActions.Enums;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.Events;
using _1_Scripts.CombatSystem.Managers.Enums;
using _1_Scripts.CombatSystem.Managers.Turns;
using _1_Scripts.CombatSystem.Services.CombatService;
using _1_Scripts.CombatSystem.Services.RowSystemService;
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

        private CombatState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                Debug.Log($"[CombatManager] State changed to: {_state}");
                CombatEvents.RaiseCombatStateChanged(_state);
            }
        }

        private Dictionary<CombatEntity, CombatTurn> _turnQueue = new();
        
        public void SetCombatants(List<CombatEntity> entities)
        {
            _allCombatants = entities;
        }

        /// <summary>
        /// Returns a dictionary with row index as key and list of entities in that row as value.
        /// </summary>
        public Dictionary<int, List<CombatEntity>> GetCombatantsByRow()
        {
            return _allCombatants
                .Where(e => e.IsAlive)
                .GroupBy(e => e.RowIndex)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Gets all combatants in a specific row.
        /// </summary>
        public List<CombatEntity> GetCombatantsInRow(int rowIndex)
        {
            return _allCombatants
                .Where(e => e.RowIndex == rowIndex && e.IsAlive)
                .ToList();
        }

        public List<CombatEntity> GetEnemies()
        {
            return _enemies;
        }

        /// <summary>
        /// Gets the row of a specific combatant.
        /// </summary>
        public int GetCombatantRow(CombatEntity entity) => entity.RowIndex;

        /// <summary>
        /// Debug tool: prints all combatants by row.
        /// </summary>
        public void DebugPrintCombatantsByRow()
        {
            var byRow = GetCombatantsByRow();
            Debug.Log($"[CombatManager] Total Combatants: {byRow.Count}, in the follow rows: ");
            for (int i = CombatConstants.StartingRowIndex; i <= CombatConstants.EndingRowIndex; i++)
            {
                var rowList = byRow.ContainsKey(i) ? byRow[i] : new List<CombatEntity>();
                Debug.Log($"Row {i}: {string.Join(", ", rowList.Select(e => e.name))}");
            }
        }
        
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
                if (args.Target.IsAlive) return;
                Debug.Log($"[CombatManager] {args.Target.name} has died.");
                CombatEvents.RaiseCombatEntityDied(args.Target, 
                    _players.Count(p => p.IsAlive), 
                    _enemies.Count(e => e.IsAlive));
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

        private void ProceedToNextPhase()
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
                    if (_players.All(p => !p.IsAlive) || _enemies.All(e => !e.IsAlive))
                    {
                        State = CombatState.EndOfCombat;
                    }
                    else
                    {
                        ClearTurnQueue();
                        State = CombatState.Planning;
                        // Important: Let bootstrap or UI handle starting next planning phase.
                    }
                    break;

                case CombatState.EndOfCombat:
                    HandleCombatEnd();
                    break;

                case CombatState.OutOfCombat:
                    // idle state, waiting for combat start
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void HandleCombatEnd()
        {
            Debug.Log("[CombatManager] Combat has ended.");
            CombatEvents.RaiseCombatEnd(_players.Count(p => p.IsAlive), _enemies.Count(e => e.IsAlive));

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
            DebugPrintCombatantsByRow();
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

            if (!AllAliveCombatantsSubmitted()) return;
            Debug.Log("[CombatManager] All actions received, proceeding to next phase.");
            ProceedToNextPhase();
        }
        
        private bool AllAliveCombatantsSubmitted()
        {
            return _allCombatants
                .Where(entity => entity.IsAlive)
                .All(entity => _turnQueue[entity].SelectedAction != null);
        }
        
        private void ResolveRound()
        {
            Debug.Log("[CombatManager] Resolving round...");

            var orderedTurns = _turnQueue.Values
                .Where(t => t.SelectedAction != null && t.Caster.IsAlive)
                .OrderByDescending(t => t.Caster.Agility)
                .ToList();

            foreach (var turn in orderedTurns)
            {
                if (!turn.Caster.IsAlive)
                {
                    Debug.Log($"[CombatManager] Entity: {turn.Caster.name} is dead and cannot resolve its action.");
                    continue;
                }
                
                switch (turn.SelectedAction.CombatActionType)  
                {
                    case CombatActionType.Move:
                        _combatService.ExecuteAction( turn.SelectedAction, turn.Caster);
                        break;
                    case CombatActionType.Attack:
                        _combatService.ExecuteAction(turn.SelectedAction, turn.SelectedTargets, turn.Caster);
                        break;
                    case CombatActionType.UseItem:
                        break;
                    default:
                        Debug.Log($"[CombatManager] Entity: {turn.SelectedAction} is not a valid combat action (invalid CombatActionType).");
                        throw new ArgumentOutOfRangeException();
                }
            }

            Debug.Log("[CombatManager] Round complete.");
            ProceedToNextPhase();
        }
        
        public bool IsEntityAlive(CombatEntity entity) => entity.IsAlive;
    }
    
}
