using UnityEditor;
using UnityEngine;
/*
[CustomEditor(typeof(EnemyMovement))]
[CanEditMultipleObjects]
public class EnemyInspectorEditor : Editor {
    SerializedProperty patrolPointA;
    SerializedProperty patrolPointB;

    private void OnEnable() {
        patrolPointA = serializedObject.FindProperty("patrolPointA");
        patrolPointB = serializedObject.FindProperty("patrolPointB");
    }

    public override void OnInspectorGUI() {
        EnemyMovement myScript = (EnemyMovement)target;
        
        EditorGUILayout.PropertyField(patrolPointA, new GUIContent("Patrol Point A"));

        if(GUILayout.Button("Set Patrol Point A")) {
            patrolPointA.vector2Value = myScript.transform.position;
        }
        
        EditorGUILayout.PropertyField(patrolPointB, new GUIContent("Patrol Point B"));
        
        if(GUILayout.Button("Set Patrol Point B")) {
            patrolPointB.vector2Value = myScript.transform.position;
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}*/