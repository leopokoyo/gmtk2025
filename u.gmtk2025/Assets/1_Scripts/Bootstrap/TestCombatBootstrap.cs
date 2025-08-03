using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.Events;
using _1_Scripts.CombatSystem.Managers;
using _1_Scripts.CombatSystem.Managers.Enums;
using _1_Scripts.CombatSystem.Services.ActionSelectorService;
using UnityEngine;

public class TestCombatBootstrapper : MonoBehaviour
{
    private ActionSelectorService _actionSelectorService;

    [SerializeField]
    private List<CombatEntity> _players = new();
    
    [SerializeField]
    private List<CombatEntity> _enemies = new();

    private void Awake()
    {
        _actionSelectorService = new ActionSelectorService();

        CombatEvents.OnCombatStart += OnCombatStartHandler;
        CombatEvents.OnCombatStateChanged += OnCombatStateChangedHandler;
    }

    private void OnDestroy()
    {
        CombatEvents.OnCombatStart -= OnCombatStartHandler;
        CombatEvents.OnCombatStateChanged -= OnCombatStateChangedHandler;
    }
    
    private void Start()
    {
        foreach (var player in _players) player.SetRow(Random.Range(0, 1));
        foreach (var enemy in _enemies) enemy.SetRow(Random.Range(2, 3));
        // // Player rows: 0 (front), 1 (back)
        // for (var i = 0; i < _players.Count; i++)
        // {
        //     var row = i < 2 ? 0 : 1;
        //     _players[i].SetRow(1);
        // }
        //
        // // Enemy rows: 2 (front), 3 (back)
        // _enemies[0].SetRow(2);
        // _enemies[1].SetRow(3);
        
        // Raise the combat start event to kick off the process
        CombatEvents.RaiseCombatStart(this, new StartCombatEventArgs(_players, _enemies));
    }

    private void OnCombatStartHandler(object sender, StartCombatEventArgs e)
    {
        _players = e.PlayerParty;
        _enemies = e.EnemiesInCombat;
    }

    private void OnCombatStateChangedHandler(CombatState state)
    {
        if (state == CombatState.Planning)
        {
            StartCoroutine(AutoSelectPlayerActions());
        }
    }

    private IEnumerator AutoSelectPlayerActions()
    {
        _actionSelectorService.StartPlayerSelection(_players);

        while (_actionSelectorService.CurrentPlayer != null)
        {
            var player = _actionSelectorService.CurrentPlayer;

            // Pick the first available action
            var action = player.CombatActions.FirstOrDefault();
            if (action == null)
            {
                Debug.LogWarning($"No actions found for player {player.name}. Skipping.");
                _actionSelectorService.SubmitPlayerAction(null, null);
                yield return null;
                continue;
            }

            // Pick first alive enemy as target
            var target = _enemies.FirstOrDefault(e => e.IsAlive);
            if (target == null)
            {
                Debug.LogWarning("No alive enemies found for targeting.");
                _actionSelectorService.SubmitPlayerAction(action, new List<CombatEntity>());
            }
            else
            {
                _actionSelectorService.SubmitPlayerAction(action, new List<CombatEntity> { target });
            }

            // Wait 0.2 seconds before next player action
            yield return new WaitForSeconds(0.4f);
        }

        // All player actions selected; assign enemy actions immediately
        AssignEnemyActions();
    }

    private void AssignEnemyActions()
    {
        foreach (var enemy in _enemies.Where(e => e.IsAlive))
        {
            // Choose enemy action (pick first action)
            var actionList = enemy.CombatActions;
            var action = actionList.Count > 0 ? actionList[Random.Range(0, actionList.Count)] : actionList.FirstOrDefault();

            // Choose first alive player as target
            var target = _players.FirstOrDefault(p => p.IsAlive);

            if (action != null && target != null)
            {
                CombatManager.Instance.StorePlayerAction(enemy, action, new List<CombatEntity> { target });
            }
        }
    }
}
