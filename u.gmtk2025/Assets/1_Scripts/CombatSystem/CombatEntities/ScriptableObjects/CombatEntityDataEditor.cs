#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities.ScriptableObjects;

[CustomEditor(typeof(CombatEntityData))]
public class CombatEntityDataEditor : Editor
{
    private CombatEntityData data;
    private List<Type> allActionTypes;
    private List<Type> availableTypes;
    private string[] availableTypeNames;
    private int selectedTypeIndex = 0;

    private void OnEnable()
    {
        data = (CombatEntityData)target;
        RefreshAvailableTypes();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Combat Actions", EditorStyles.boldLabel);

        if (data.CombatActions == null)
            data.CombatActions = new List<BaseCombatAction>();

        for (int i = 0; i < data.CombatActions.Count; i++)
        {
            var action = data.CombatActions[i];
            if (action == null)
            {
                EditorGUILayout.LabelField($"Action {i}: null");
                continue;
            }

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(action.GetType().Name, EditorStyles.boldLabel);

            SerializedProperty listProp = serializedObject.FindProperty("CombatActions");
            EditorGUILayout.PropertyField(listProp.GetArrayElementAtIndex(i), true);

            if (GUILayout.Button("Remove"))
            {
                data.CombatActions.RemoveAt(i);
                RefreshAvailableTypes();
                break; // to avoid modifying list while drawing
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Unique Action", EditorStyles.boldLabel);

        if (availableTypes.Count == 0)
        {
            EditorGUILayout.HelpBox("All available action types are already added.", MessageType.Info);
        }
        else
        {
            selectedTypeIndex = EditorGUILayout.Popup(selectedTypeIndex, availableTypeNames);
            if (GUILayout.Button("Add Action"))
            {
                Type typeToAdd = availableTypes[selectedTypeIndex];
                var newAction = (BaseCombatAction)Activator.CreateInstance(typeToAdd);
                data.CombatActions.Add(newAction);
                RefreshAvailableTypes();
                EditorUtility.SetDirty(data);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void RefreshAvailableTypes()
    {
        if (data == null)
            return;

        if (data.CombatActions == null)
            data.CombatActions = new List<BaseCombatAction>();

        allActionTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(BaseCombatAction).IsAssignableFrom(t) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null)
            .ToList();

        var usedTypes = data.CombatActions
            .Where(a => a != null)
            .Select(a => a.GetType())
            .ToHashSet();

        availableTypes = allActionTypes
            .Where(t => !usedTypes.Contains(t))
            .ToList();

        availableTypeNames = availableTypes
            .Select(t => t.Name)
            .ToArray();

        selectedTypeIndex = 0;
    }
}
#endif
