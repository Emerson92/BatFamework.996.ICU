using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace THEDARKKNIGHT.ProcessCore.Graph.NodeEditor {
    [CustomPropertyDrawer(typeof(LuaClassType))]
    public class SubProcessDrawer : PropertyDrawer
    {
        private float SubProcessItemHeight = 60;

        GUIStyle style = NodeEditorResources.styles.nodeBody;

        GUIStyle ButtonStyle = new GUIStyle()
        {
            margin = new RectOffset(1,1,1,1),
        };



        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Element 1
            SerializedProperty nickProperty = property.FindPropertyRelative("Nickname");
            SerializedProperty luaPathProperty = property.FindPropertyRelative("LuaPath");

            LuaClassType[] item = (property.serializedObject.targetObject as ProcessItem).LuaProcessItems;
            EditorGUI.BeginProperty(position, label, property);
            Rect openFolder      = new   Rect(position.x+235  , position.y + 15, 25, 15);
            Rect filePathLabel   = new   Rect(position.x      , position.y + 15, 75, 25);
            Rect filePath        = new   Rect(position.x+80   , position.y + 15, 150, 18);
            Rect nameSpaceLabel  = new   Rect(position.x      , position.y + 45, 75, 25);
            Rect nameSpace       = new   Rect(position.x+80  , position.y + 45, 150, 18);
            EditorGUI.LabelField(filePathLabel, "Nickname", NodeEditorResources.styles.SubFontSytle);
            if (GUI.Button(openFolder, NodeEditorResources.OpenFile,ButtonStyle))
            {

                string outputPath = EditorUtility.OpenFilePanel("Choose your Config", "", "");
                if (!string.IsNullOrEmpty(outputPath))
                {
                    string fileName = Path.GetFileName(outputPath);
                    nickProperty.stringValue = fileName;
                    
                }
                luaPathProperty.stringValue = outputPath;
            }
            EditorGUI.LabelField(nameSpaceLabel, "LuaPath", NodeEditorResources.styles.SubFontSytle);
            luaPathProperty.stringValue = EditorGUI.DelayedTextField(nameSpace, luaPathProperty.stringValue);
            EditorGUI.LabelField(filePath, nickProperty.stringValue);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            
            return SubProcessItemHeight;
        }

    }

    public struct ScriptInfo {
        public string  FileName;
        public string OutputPath;
    }
}
