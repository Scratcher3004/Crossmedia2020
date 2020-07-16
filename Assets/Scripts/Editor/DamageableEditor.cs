using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Damageable), true)]
public class DamageableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!EditorApplication.isPlaying && !EditorApplication.isPaused)
            return;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Health: " + ((Damageable) serializedObject.targetObject).GetCurrentHealth);
    }
}
