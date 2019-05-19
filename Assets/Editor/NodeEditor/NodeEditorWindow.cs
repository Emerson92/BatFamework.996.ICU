using System;
using System.Collections.Generic;
using THEDARKKNIGHT.FileSystem;
using THEDARKKNIGHT.ProcessCore.Graph;
using THEDARKKNIGHT.ProcessCore.Graph.Json;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace XNodeEditor {
    [InitializeOnLoad]
    public partial class NodeEditorWindow : EditorWindow {
        public static NodeEditorWindow current;

        /// <summary> Stores node positions for all nodePorts. </summary>
        public Dictionary<XNode.NodePort, Rect> portConnectionPoints { get { return _portConnectionPoints; } }
        private Dictionary<XNode.NodePort, Rect> _portConnectionPoints = new Dictionary<XNode.NodePort, Rect>();
        public Dictionary<XNode.Node, Vector2> nodeSizes { get { return _nodeSizes; } }
        private Dictionary<XNode.Node, Vector2> _nodeSizes = new Dictionary<XNode.Node, Vector2>();
        public XNode.NodeGraph graph;
        public Vector2 panOffset { get { return _panOffset; } set { _panOffset = value; Repaint(); } }
        private Vector2 _panOffset;
        public float zoom { get { return _zoom; } set { _zoom = Mathf.Clamp(value, 1f, 5f); Repaint(); } }
        private float _zoom = 1;

        void OnFocus() {
            current = this;
            graphEditor = NodeGraphEditor.GetEditor(graph);
            if (graphEditor != null && NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
        }


        partial void OnEnable();

        /// <summary> Create editor window </summary>
        public static NodeEditorWindow Init() {
            NodeEditorWindow w = CreateInstance<NodeEditorWindow>();
            w.titleContent = new GUIContent("ProcessGraph");
            w.wantsMouseMove = true;
            w.Show();
            return w;
        }

        public void Save() {
            if (AssetDatabase.Contains(graph)) {
                EditorUtility.SetDirty(graph);
                if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
            } else SaveAs();
        }

        public string SaveAs() {
            string path = EditorUtility.SaveFilePanelInProject("Save NodeGraph", "NewNodeGraph", "asset", "");
            if (string.IsNullOrEmpty(path)) return null;
            else {
                XNode.NodeGraph existingGraph = AssetDatabase.LoadAssetAtPath<XNode.NodeGraph>(path);
                if (existingGraph != null) AssetDatabase.DeleteAsset(path);
                AssetDatabase.CreateAsset(graph, path);
                EditorUtility.SetDirty(graph);
                if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
            }
            return path;
        }

        private void DraggableWindow(int windowID) {
            GUI.DragWindow();
        }

        public Vector2 WindowToGridPosition(Vector2 windowPosition) {
            return (windowPosition - (position.size * 0.5f) - (panOffset / zoom)) * zoom;
        }

        public Vector2 GridToWindowPosition(Vector2 gridPosition) {
            return (position.size * 0.5f) + (panOffset / zoom) + (gridPosition / zoom);
        }

        public Rect GridToWindowRectNoClipped(Rect gridRect) {
            gridRect.position = GridToWindowPositionNoClipped(gridRect.position);
            return gridRect;
        }

        public Rect GridToWindowRect(Rect gridRect) {
            gridRect.position = GridToWindowPosition(gridRect.position);
            gridRect.size /= zoom;
            return gridRect;
        }

        public Vector2 GridToWindowPositionNoClipped(Vector2 gridPosition) {
            Vector2 center = position.size * 0.5f;
            float xOffset = (center.x * zoom + (panOffset.x + gridPosition.x));
            float yOffset = (center.y * zoom + (panOffset.y + gridPosition.y));
            return new Vector2(xOffset, yOffset);
        }

        public void SelectNode(XNode.Node node, bool add) {
            if (add) {
                List<UnityEngine.Object> selection = new List<UnityEngine.Object>(Selection.objects);
                selection.Add(node);
                Selection.objects = (UnityEngine.Object[])selection.ToArray();
            } else Selection.objects = new UnityEngine.Object[] { node };
        }

        public void DeselectNode(XNode.Node node) {
            List<UnityEngine.Object> selection = new List<UnityEngine.Object>(Selection.objects);
            selection.Remove((UnityEngine.Object)node);
            Selection.objects = (UnityEngine.Object[])selection.ToArray();
        }

        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line) {
            XNode.NodeGraph nodeGraph = EditorUtility.InstanceIDToObject(instanceID) as XNode.NodeGraph;
            if (nodeGraph != null)
            {
                NodeEditorWindow w = GetWindow(typeof(NodeEditorWindow), false, "ProcessGraph", true) as NodeEditorWindow;
                w.wantsMouseMove = true;
                w.graph = nodeGraph;
                return true;
            }
            return false;

        }

        /// <summary> Repaint all open NodeEditorWindows. </summary>
        public static void RepaintAll() {
            NodeEditorWindow[] windows = Resources.FindObjectsOfTypeAll<NodeEditorWindow>();
            for (int i = 0; i < windows.Length; i++) {
                windows[i].Repaint();
            }
        }

        /// <summary>
        /// Save File as config of json
        /// </summary>
        public void SaveAsJson() {
            string path = EditorUtility.OpenFolderPanel("Config","","");
            if (graph != null)
            {
                ProcessItem nextNode = null;
                ProcessJson json = new ProcessJson();
                List<ProcessUnit> dataArray = new List<ProcessUnit>();
                ///////////////////////////////
                //// Get the head process item
                //////////////////////////////
                for (int i = 0; i < graph.nodes.Count; i++)
                {
                    ProcessItem item = graph.nodes[i] as ProcessItem;
                    if (!CheckArrayIsNotNull(item.EnterProcess) && CheckArrayIsNotNull(item.OutPortProcess))
                    {
                        nextNode = item;
                        break;
                    }
                }
                if (nextNode != null)
                {
                    Debug.Log("nextNode :" + nextNode.name);
                    SearchProcessUnit(nextNode, dataArray);
                    json.ProcessList = dataArray;
                    Debug.Log("json.ProcessList :" + json.ProcessList.Count);
                    string jsonData = JsonUtility.ToJson(json);
                    byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(jsonData);
                    Debug.Log("jsonData :" + jsonData);
                    BFileSystem.Instance().WriteFileToDiskAsync(data, path, "ProcessConfig.Json", true);

                }
                else {
                    Debug.Log("Save File,can not find the first node!");
                }

            }
        }

        private bool CheckArrayIsNotNull(ProcessItem[] enterProcess)
        {
            bool flage = false;
            if (enterProcess != null)
            {

                for (int i = 0; i < enterProcess.Length ;i ++)
                {
                    if (enterProcess[i] != null)
                        return !flage;
                }
                return flage;
            }
            else
                return flage;
        }

        private void SearchProcessUnit(ProcessItem nextNode, List<ProcessUnit> dataArray)
        {
            if (nextNode != null)
            {
                for (int k = 0; k < dataArray.Count;k++){
                    if(dataArray[k].name == nextNode.name)                   
                        return ;
                }
                ProcessUnit unit = new ProcessUnit();
                List<SubProcess> subProcessList = new List<SubProcess>();
                for (int j = 0; j < nextNode.ProcessItems.Length; j++)
                {
                    SubProcess subClass = new SubProcess();
                    subClass.Namespace = nextNode.ProcessItems[j].Namespace;
                    subClass.ClassName = nextNode.ProcessItems[j].className;
                    subProcessList.Add(subClass);
                }
                unit.SubProcessList = subProcessList;
                unit.BranchID = nextNode.BranchID;
                unit.BranchParentName = nextNode.BranchParent;
                unit.SubBranchID = nextNode.SubBranchID;
                unit.name = nextNode.name;
                dataArray.Add(unit);
                if (nextNode.OutPortProcess != null)
                {
                    if(nextNode.OutPortProcess.Length > 0)
                    {
                        for (int i = 0; i < nextNode.OutPortProcess.Length; i++)
                        {
                            if( nextNode.OutPortProcess[i] != null )
                            {
                                nextNode = nextNode.OutPortProcess[i];
                                SearchProcessUnit(nextNode, dataArray);
                            }
                        }

                    }

                }
            }
        }
    }
}