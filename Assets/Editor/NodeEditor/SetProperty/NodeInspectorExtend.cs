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
            switch (property.name) {
                case "BranchParent":
                    break;
                case "ProcessItems":
                    GUIStyle style = NodeEditorResources.styles.subProcessBG;
                    EditorGUILayout.BeginVertical(style);
                    EditorGUILayout.PropertyField(property, label, includeChildren, GUILayout.MinWidth(40));
                    EditorGUILayout.EndVertical();
                    break;
                default:
                    EditorGUILayout.PropertyField(property, label, includeChildren, GUILayout.MinWidth(40));
                    break;
            }
        }
    }

}

