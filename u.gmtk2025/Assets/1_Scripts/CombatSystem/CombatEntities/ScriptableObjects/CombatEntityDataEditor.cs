#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using _1_Scripts.CombatSystem.CombatActions.Interfaces;
using _1_Scripts.CombatSystem.CombatEntities.ScriptableObjects;

[CustomEditor(typeof(CombatEntityData))]
public class CombatEntityDataEditor : Editor
{
    private CombatEntityData data;

    private void OnEnable()
    {
        data = (CombatEntityData)target;
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
            EditorGUILayout.BeginHorizontal();
            data.CombatActions[i] = (BaseCombatAction)EditorGUILayout.ObjectField(data.CombatActions[i], typeof(BaseCombatAction), false);
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                data.CombatActions.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Add New Action", EditorStyles.boldLabel);
        BaseCombatAction newAction = (BaseCombatAction)EditorGUILayout.ObjectField(null, typeof(BaseCombatAction), false);
        if (newAction != null && !data.CombatActions.Contains(newAction))
        {
            data.CombatActions.Add(newAction);
            EditorUtility.SetDirty(data);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif