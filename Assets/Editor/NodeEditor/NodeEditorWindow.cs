using System;
using System.Collections.Generic;
using System.Text;
using THEDARKKNIGHT.FileSystem;
using THEDARKKNIGHT.ProcessCore.Graph;
using THEDARKKNIGHT.ProcessCore.Graph.Json;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace XNodeEditor
{
    [InitializeOnLoad]
    public partial class NodeEditorWindow : EditorWindow
    {
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

        void OnFocus()
        {
            current = this;
            graphEditor = NodeGraphEditor.GetEditor(graph);
            if (graphEditor != null && NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
        }


        partial void OnEnable();

        /// <summary> Create editor window </summary>
        public static NodeEditorWindow Init()
        {
            NodeEditorWindow w = CreateInstance<NodeEditorWindow>();
            w.titleContent = new GUIContent("ProcessGraph");
            w.wantsMouseMove = true;
            w.Show();
            return w;
        }

        public void Save()
        {
            if (AssetDatabase.Contains(graph))
            {
                EditorUtility.SetDirty(graph);
                if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
            }
            else SaveAs();
        }

        public string SaveAs()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save NodeGraph", "NewNodeGraph", "asset", "");
            if (string.IsNullOrEmpty(path)) return null;
            else
            {
                XNode.NodeGraph existingGraph = AssetDatabase.LoadAssetAtPath<XNode.NodeGraph>(path);
                if (existingGraph != null) AssetDatabase.DeleteAsset(path);
                AssetDatabase.CreateAsset(graph, path);
                EditorUtility.SetDirty(graph);
                if (NodeEditorPreferences.GetSettings().autoSave) AssetDatabase.SaveAssets();
            }
            return path;
        }

        /// <summary>
        /// reload the config,refresh the new node
        /// </summary>
        /// <param name="path"></param>
        public void ReLoadConfig(string path)
        {
            byte[] data = BFileSystem.Instance().ReadFileFromDisk(path);
            string config = Encoding.UTF8.GetString(data);
            Debug.Log("config :" + config);
            XNode.NodeGraph nodeGraph = new ProcessGraph();
            List<ProcessItem> nodes = new List<ProcessItem>();
            ProcessJson configData = JsonUtility.FromJson<ProcessJson>(config);
            /////Start to create new nodes
            for (int i = 0; i < configData.ProcessList.Count; i++)
            {
                ProcessUnit unit = configData.ProcessList[i];
                ProcessItem node = new ProcessItem();
                node.name = unit.Name;
                node.position = unit.Pos;
                node.BranchID = unit.BranchID;
                node.BranchParent = unit.BranchParentName;
                node.SubBranchID = unit.SubBranchID;
                node.ParBranchID = unit.ParBranchID;
                node.graph = nodeGraph;
                if (unit.IsLuaScript)
                {
                    node.LuaProcessItems = new LuaClassType[unit.SubLuaProcessList.Count];
                    ///// Lua Script
                    for (int k = 0; k < unit.SubLuaProcessList.Count; k++)
                    {
                        LuaClassType LuaItem = new LuaClassType();
                        LuaItem.Nickname = unit.SubLuaProcessList[k].Nickname;
                        LuaItem.LuaPath = unit.SubLuaProcessList[k].UrlPath;
                        node.LuaProcessItems[k] = LuaItem;
                    }
                }
                else
                {
                    node.ProcessItems = new ClassType[unit.SubProcessList.Count];
                    ///// C# Script
                    for (int j = 0; j < unit.SubProcessList.Count; j++)
                    {
                        ClassType item = new ClassType();
                        item.Nickname = unit.SubProcessList[j].Nickname;
                        item.Namespace = unit.SubProcessList[j].Namespace;
                        item.Classname = unit.SubProcessList[j].ClassName;
                        node.ProcessItems[j] = item;
                    }
                }
                nodes.Add(node);
            }


            ///// reconnect the link from node to node
            for (int t = 0; t < nodes.Count;t++) {
                ProcessItem node = nodes[t];

                /////EnterProcess
                if (node.ParBranchID.Length > 0)
                {
                    ProcessItem[] lastUnits = FindLastNodes(nodes, node.ParBranchID);
                    node.EnterProcess = lastUnits;
                }
                else
                {
                    ProcessItem lastNode = FindLastNode(nodes, t);
                    node.EnterProcess = new ProcessItem[] { lastNode };
                }

                /////OutterProcess
                if (node.SubBranchID.Length > 0)
                {
                    ProcessItem[] nextUnits = FindNextNodes(nodes, node.ParBranchID);
                    node.EnterProcess = nextUnits;
                }
                else {
                    ProcessItem nextNode = FindNextNode(nodes, t);
                    node.OutPortProcess = new ProcessItem[] { nextNode };
                }
            }
            nodeGraph.nodes = nodes;
            ////TODO 刷新图像
            this.graph = nodeGraph;
        }

        private ProcessItem[] FindNextNodes(List<ProcessItem> nodes, string[] parBranchID)
        {
            ProcessItem[] startNodes = new ProcessItem[parBranchID.Length];
            for (int i = 0; i < parBranchID.Length; i++)
            {
                string branchID = parBranchID[i];
                for (int j =0; j < nodes.Count; j++)
                {
                    if (nodes[j].BranchID == branchID)
                    {
                        startNodes[i] = nodes[j];
                    }
                }
            }
            return startNodes;
        }

        private ProcessItem FindNextNode(List<ProcessItem> nodes, int index)
        {
            int tempIndex = index;
            while (tempIndex > 0 && !string.IsNullOrEmpty(nodes[tempIndex].BranchID))
            {
                tempIndex += 1;
            }
            if (index != tempIndex)
                return nodes[tempIndex];
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processList"></param>
        /// <param name="parBranchID"></param>
        /// <returns></returns>
        private ProcessItem[] FindLastNodes(List<ProcessItem> processList,string[] parBranchID)
        {
            ProcessItem[] endNodes = new ProcessItem[parBranchID.Length];
            for (int i = 0; i < parBranchID.Length;i++) {
                string branchID = parBranchID[i];
                for (int j = processList.Count; j > 0; j--) {
                    if (processList[j].BranchID == branchID) {
                        endNodes[i] = processList[j];
                    }
                }
            }
            return endNodes;
        }

        private ProcessItem FindLastNode(List<ProcessItem> processList, int index)
        {
            int tempIndex = index;
            while (tempIndex > 0 && string.IsNullOrEmpty(processList[tempIndex].BranchID))
            {
                tempIndex -= 1;
            }
            if (index != tempIndex)
                return processList[tempIndex];
            else
                return null;
        }

        private void DraggableWindow(int windowID)
        {
            GUI.DragWindow();
        }

        public Vector2 WindowToGridPosition(Vector2 windowPosition)
        {
            return (windowPosition - (position.size * 0.5f) - (panOffset / zoom)) * zoom;
        }

        public Vector2 GridToWindowPosition(Vector2 gridPosition)
        {
            return (position.size * 0.5f) + (panOffset / zoom) + (gridPosition / zoom);
        }

        public Rect GridToWindowRectNoClipped(Rect gridRect)
        {
            gridRect.position = GridToWindowPositionNoClipped(gridRect.position);
            return gridRect;
        }

        public Rect GridToWindowRect(Rect gridRect)
        {
            gridRect.position = GridToWindowPosition(gridRect.position);
            gridRect.size /= zoom;
            return gridRect;
        }

        public Vector2 GridToWindowPositionNoClipped(Vector2 gridPosition)
        {
            Vector2 center = position.size * 0.5f;
            float xOffset = (center.x * zoom + (panOffset.x + gridPosition.x));
            float yOffset = (center.y * zoom + (panOffset.y + gridPosition.y));
            return new Vector2(xOffset, yOffset);
        }

        public void SelectNode(XNode.Node node, bool add)
        {
            if (add)
            {
                List<UnityEngine.Object> selection = new List<UnityEngine.Object>(Selection.objects);
                selection.Add(node);
                Selection.objects = (UnityEngine.Object[])selection.ToArray();
            }
            else Selection.objects = new UnityEngine.Object[] { node };
        }

        public void DeselectNode(XNode.Node node)
        {
            List<UnityEngine.Object> selection = new List<UnityEngine.Object>(Selection.objects);
            selection.Remove((UnityEngine.Object)node);
            Selection.objects = (UnityEngine.Object[])selection.ToArray();
        }

        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line)
        {
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
        public static void RepaintAll()
        {
            NodeEditorWindow[] windows = Resources.FindObjectsOfTypeAll<NodeEditorWindow>();
            for (int i = 0; i < windows.Length; i++)
            {
                windows[i].Repaint();
            }
        }

        /// <summary>
        /// Save File as config of json
        /// </summary>
        public void SaveAsJson()
        {
            string path = EditorUtility.OpenFolderPanel("Config", "", "");
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
                else
                {
                    Debug.Log("Save File,can not find the first node!");
                }

            }
        }

        private bool CheckArrayIsNotNull(ProcessItem[] enterProcess)
        {
            bool flage = false;
            if (enterProcess != null)
            {

                for (int i = 0; i < enterProcess.Length; i++)
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
                for (int k = 0; k < dataArray.Count; k++)
                {
                    if (dataArray[k].Name == nextNode.name)
                        return;
                }
                ProcessUnit unit = new ProcessUnit();
                unit.IsLuaScript = nextNode.IsLuaScript;
                if (nextNode.IsLuaScript)
                {
                    List<SubLuaProcess> subLuaProcessList = new List<SubLuaProcess>();
                    for (int j = 0; j < nextNode.LuaProcessItems.Length; j++)
                    {
                        SubLuaProcess subClass = new SubLuaProcess();
                        subClass.Nickname = nextNode.LuaProcessItems[j].Nickname;
                        subClass.UrlPath = nextNode.LuaProcessItems[j].LuaPath;
                        subLuaProcessList.Add(subClass);
                    }
                    unit.SubLuaProcessList = subLuaProcessList;
                }
                else
                {

                    List<SubProcess> subProcessList = new List<SubProcess>();
                    for (int j = 0; j < nextNode.ProcessItems.Length; j++)
                    {
                        SubProcess subClass = new SubProcess();
                        subClass.Nickname = nextNode.ProcessItems[j].Nickname;
                        subClass.Namespace = nextNode.ProcessItems[j].Namespace;
                        subClass.ClassName = nextNode.ProcessItems[j].Classname;
                        subProcessList.Add(subClass);
                    }
                    unit.SubProcessList = subProcessList;
                }
                unit.BranchID = nextNode.BranchID;
                unit.BranchParentName = nextNode.BranchParent;
                unit.SubBranchID = nextNode.SubBranchID;
                unit.Name = nextNode.name;
                dataArray.Add(unit);
                if (nextNode.OutPortProcess != null)
                {
                    if (nextNode.OutPortProcess.Length > 0)
                    {
                        for (int i = 0; i < nextNode.OutPortProcess.Length; i++)
                        {
                            if (nextNode.OutPortProcess[i] != null)
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