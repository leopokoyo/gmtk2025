using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using _1_Scripts.CombatSystem.CombatEntities;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.Services.ActionSelectorService;
using _1_Scripts.CombatSystem.Events;
using _1_Scripts.CombatSystem.Managers;

public class CombatUIController : MonoBehaviour
{
    private VisualElement _root;
    private Label _characterNameLabel;
    private VisualElement _actionButtonsContainer;
    private VisualElement _targetButtonsContainer;
    private Button _confirmButton;

    private CombatEntity _currentEntity;
    private BaseCombatAction _selectedAction;
    private CombatEntity _selectedTarget;

    private ActionSelectorService actionSelector;

    private void OnEnable()
    {
        actionSelector = new ActionSelectorService();
        var uiDocument = GetComponent<UIDocument>();
        _root = uiDocument.rootVisualElement;
        
        _characterNameLabel = _root.Q<Label>("CharacterNameLabel");
        _actionButtonsContainer = _root.Q<VisualElement>("ActionButtonsContainer");
        _targetButtonsContainer = _root.Q<VisualElement>("TargetButtonsContainer");
        _confirmButton = _root.Q<Button>("ConfirmButton");

        _confirmButton.clicked += OnConfirmClicked;

        CombatEvents.OnPlayerActionRequested += OnPlayerActionRequested;
    }

    private void OnDisable()
    {
        _confirmButton.clicked -= OnConfirmClicked;
        CombatEvents.OnPlayerActionRequested -= OnPlayerActionRequested;
    }

    private void OnPlayerActionRequested(CombatEntity entity)
    {
        _currentEntity = entity;
        _selectedAction = null;
        _selectedTarget = null;

        _characterNameLabel.text = $"Choose Action for {entity.name}";
        RenderActions(entity);
        RenderTargets();
    }

    private void RenderActions(CombatEntity entity)
    {
        _actionButtonsContainer.Clear();

        foreach (var action in entity.CombatActions)
        {
            var button = new Button(() =>
            {
                _selectedAction = action;
                Debug.Log($"Selected action: {action.CombatActionName}");
            })
            {
                text = action.CombatActionName
            };

            _actionButtonsContainer.Add(button);
        }
    }

    private void RenderTargets()
    {
        _targetButtonsContainer.Clear();

        var allEnemies = CombatManager.Instance.GetEnemies();

        foreach (var enemy in allEnemies)
        {
            var button = new Button(() =>
            {
                _selectedTarget = enemy;
                Debug.Log($"Selected target: {enemy.name}");
            })
            {
                text = enemy.name
            };

            _targetButtonsContainer.Add(button);
        }
    }

    private void OnConfirmClicked()
    {
        if (_selectedAction == null || _selectedTarget == null) return;

        actionSelector.SubmitPlayerAction(_selectedAction, new List<CombatEntity> { _selectedTarget });
        ClearUI();
    }

    private void ClearUI()
    {
        _actionButtonsContainer.Clear();
        _targetButtonsContainer.Clear();
        _characterNameLabel.text = string.Empty;
    }
}
