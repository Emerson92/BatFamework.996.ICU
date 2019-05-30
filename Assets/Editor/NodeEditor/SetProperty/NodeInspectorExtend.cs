using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace THEDARKKNIGHT.ProcessCore.Graph.NodeEditor {

    public class NodeInspectorExtend 
    {
        public static void InpspectorPropertyFieldOnGUI(SerializedProperty property, GUIContent label, bool includeChildren = true) {
            ProcessItem target = property.serializedObject.targetObject as ProcessItem;
            GUIStyle style = NodeEditorResources.styles.subProcessBG;
            switch (property.name) {
                case "IsLuaScript":
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(80));
                    EditorGUILayout.LabelField("LuaScript",GUILayout.Width(80));
                    target.IsLuaScript = EditorGUILayout.Toggle(target.IsLuaScript, GUILayout.Width(50));
                    EditorGUILayout.EndHorizontal();
                    break;
                case "BranchParent":////don't draw BranchParent;
                    break;
                case "ProcessItems":
                    if (!target.IsLuaScript) {
                        EditorGUILayout.BeginVertical(style);
                        EditorGUILayout.PropertyField(property, label, includeChildren, GUILayout.MinWidth(40));
                        EditorGUILayout.EndVertical();
                    }
                    break;
                case "LuaProcessItems":
                    if (target.IsLuaScript) {
                        EditorGUILayout.BeginVertical(style);
                        EditorGUILayout.PropertyField(property, label, includeChildren, GUILayout.MinWidth(40));
                        EditorGUILayout.EndVertical();
                    }
                    break;
                default:
                    EditorGUILayout.PropertyField(property, label, includeChildren, GUILayout.MinWidth(40));
                    break;
            }
        }
    }

}

