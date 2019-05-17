using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using THEDARKKNIGHT.ProcessCore.Graph;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace THEDARKKNIGHT.ProcessCore.Graph.NodeEditor {
    //[CustomPropertyDrawer(typeof(ClassType))]
    public class SubProcessDrawer : PropertyDrawer
    {
        private float SubProcessItemHeight = 60;

        GUIStyle style = NodeEditorResources.styles.nodeBody;

        private string OutputPath = null;
        private string FileName = null;
        private Dictionary<string, ScriptInfo> ScirptInfoDic = new Dictionary<string, ScriptInfo>();

        GUIStyle ButtonStyle = new GUIStyle()
        {
            margin = new RectOffset(1,1,1,1),
        };



        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Rect openFolder      = new   Rect(position.x+235  , position.y + 15, 25, 15);
            Rect filePathLabel   = new   Rect(position.x      , position.y + 15, 75, 25);
            Rect filePath        = new   Rect(position.x+80   , position.y + 15, 150, 18);
            Rect nameSpaceLabel  = new   Rect(position.x      , position.y + 45, 75, 25);
            Rect nameSpace       = new   Rect(position.x+80  , position.y + 45, 150, 18);
            if (GUI.Button(openFolder, NodeEditorResources.OpenFile,ButtonStyle))
            {

                OutputPath = EditorUtility.OpenFilePanel("Choose your Config", "", "");
                if (!string.IsNullOrEmpty(OutputPath))
                {
                    FileName = Path.GetFileNameWithoutExtension(OutputPath);
                }
            }
            EditorGUI.LabelField(filePathLabel, "ClassName",NodeEditorResources.styles.SubFontSytle);
            EditorGUI.DelayedTextField(filePath,  FileName);
            EditorGUI.LabelField(nameSpaceLabel, "Namespace", NodeEditorResources.styles.SubFontSytle);
            EditorGUI.DelayedTextField(nameSpace, "");

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //ProcessItem target = property.serializedObject.targetObject as ProcessItem;
            //for (int i=0 ; i < target.ProcessItems.Length ; i++) {
            //    if(target.ProcessItems[i] !=null)
            //        ScirptInfoDic.Add(target.ProcessItems[i]);
            //}
            
            return SubProcessItemHeight;
        }

    }

    public struct ScriptInfo {
        public string  FileName;
        public string OutputPath;
    }
}
