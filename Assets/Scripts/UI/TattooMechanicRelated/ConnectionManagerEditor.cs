using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ConnectionManager))]
public class ConnectionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ConnectionManager cm = (ConnectionManager)target;
        if(GUILayout.Button("Preview Lines"))
        {
            cm.LinesEditorPreview();
        }
    }
}
#endif