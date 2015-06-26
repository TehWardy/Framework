using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 
/// By: Charles Humphrey @NemoKrad
/// On: 25/06/2015
/// 
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(NodeScript))]
public class NodeInspector : Editor
{
    NodeEditor editor = null;
    NodeScript myScript = null;

    SerializedObject script;
    SerializedProperty rootProperty;

    void OnEnable()
    {
        myScript = (NodeScript)target;
        script = new SerializedObject(myScript);
        rootProperty = script.FindProperty("root");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button(new GUIContent("Edit Node", "Click to edit the node stack")))
        {
            EditorUtility.SetDirty(myScript);

            editor = EditorWindow.GetWindow<NodeEditor>();

            editor.RootNode = myScript.root;
            editor.NodeTypes = myScript.AvailableNodeTypes;

            editor.Init();
        }

        if (editor != null)
        {
            myScript.root = editor.RootNode;
        }
            
        GUILayout.Label(new GUIContent(string.Format("Root Node: {0}", myScript.root == null ? "None Set" : myScript.root.GetType().Name), "Current root node"));

        script.ApplyModifiedProperties();

        Repaint();
    }


}