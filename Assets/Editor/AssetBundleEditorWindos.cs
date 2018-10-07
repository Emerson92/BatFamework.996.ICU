/**********************************************************
 *                   资源打包工具
 *    鼠标选中物体，根据在project中选择的物体，进行物体打包
 *     
 * 
 * 
 * ********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleEditorWindos : EditorWindow
{

    static AssetBundleEditorWindos EditorWindos;

    public string OutputPath = "";

    public string[] ob;

    //测试数据
    /////////////////////////////////////////
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    /////////////////////////////////////////


    private GUIStyle LableSytle;
    private string assetName;
    private string WarrnString;
    private GUIStyle WarrnStyle;
    // Use this for initialization
    void Start()
    {
        assetName = "";
        WarrnString = "";
        CreateStyle();
    }

    private void CreateStyle()
    {

        if (LableSytle == null) {
            Debug.Log("LableSytle");
            LableSytle = new GUIStyle();
            LableSytle.alignment = TextAnchor.UpperCenter;
            LableSytle.normal.background = null; //这是设置背景填充的  
            LableSytle.normal.textColor = new Color(1, 0, 0);   //设置字体颜色的  
            LableSytle.fontSize = 20; //当然，这是字体颜色  
        }
        if (WarrnStyle == null) {
            Debug.Log("WarrnStyle");
            WarrnStyle = new GUIStyle();
            WarrnStyle.alignment = TextAnchor.UpperCenter;
            WarrnStyle.normal.background = null; //这是设置背景填充的  
            WarrnStyle.normal.textColor = Color.blue;   //设置字体颜色的  
            WarrnStyle.fontSize = 15; //当然，这是字体颜色  
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 打包工具界面
    /// </summary>
    [MenuItem("评测系统打包工具/评测打包界面")]
    public static void StartOpenBundleUI()
    {
        Debug.Log("StartOpenBundleUI");
        CreateAssetBundleUI();
    }

    private static void CreateAssetBundleUI()
    {
        EditorWindos = (AssetBundleEditorWindos)EditorWindow.GetWindow(typeof(AssetBundleEditorWindos));
        EditorWindos.titleContent = new GUIContent("打包工具");
        EditorWindos.position = new Rect(100, 100, 450, 450);
        EditorWindos.Show();

    }


    /// <summary>
    /// 绘制相关界面元素
    /// </summary>
    void OnGUI()
    {
        CreateStyle();
        GUILayout.Label("打包输出路径：", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("路径:", OutputPath, GUILayout.MaxWidth(450));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("选择路径", GUILayout.Width(150f), GUILayout.Height(20f), GUILayout.MinWidth(150f)))
        {
            Debug.Log("打包输出路径");
            OutputPath = EditorUtility.OpenFolderPanel("输出路径", "", "");
            Debug.Log("OutputPath :" + OutputPath);
        }
        if (GUILayout.Button("重置路径", GUILayout.Width(150f), GUILayout.Height(20f)))
        {
            OutputPath = "";
        }
        EditorGUILayout.EndHorizontal();
     
        GUILayout.Label("包名: 为当前文件名称", LableSytle, GUILayout.MaxWidth(450));
        if (OutputPath != "")
        {
          
            GUILayout.Label("请用鼠标在Project窗口视图中框选你需要打包的文件夹与资源文件", EditorStyles.boldLabel);
            if (GUILayout.Button("开始打包", GUILayout.Width(100f), GUILayout.Height(50f)))
            {
                if (ob != null)
                {
                    if (ob.Length > 0)
                    {
                        Debug.Log("开始打包");
                        ClearAssetBundlesName();
                        PerpareToBuild();
                        StartToBuild();
                    }
                    else
                    {
                        WarrnString = "无效选中文件";
                    }
                }
                else {
                    WarrnString = "无效选中文件";
                }
            }
            GUILayout.Label(WarrnString, WarrnStyle);
        }

        if (Event.current.type == EventType.MouseMove)//当事件为移动鼠标  
        {
            Debug.Log("EventType.MouseMove");
            Repaint(); //重新绘制  
        }
    }

    /// <summary>
    /// 开始进行打包
    /// </summary>
    private void StartToBuild()
    {
        EditorUtility.ClearProgressBar();
        BuildPipeline.BuildAssetBundles(OutputPath, BuildAssetBundleOptions.None,BuildTarget.Android);
        AssetDatabase.Refresh();
        WarrnString = "打包成功，请在输出路径中查看你的AssertBundle文件";
        try
        {
            System.Diagnostics.Process.Start(@OutputPath);
        }
        catch
        {
            Debug.LogError(" 打开路径错误 " + @OutputPath);
        }

    }

    /// <summary>

    /// 打包前的准备工作
    /// </summary>
    private void PerpareToBuild()
    {
        if (ob == null || ob.Length < 0) {
            WarrnString = "请在project视窗中选择你的打包资源";
            return;
        }
        WarrnString = "";
        for (int i = 0; i < ob.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(ob[i]);
            string[] SplitString = assetPath.Split('/');
            string AssertName = SplitString[SplitString.Length - 1];
            string[] name = AssertName.Split('.');
            //在代码中给资源设置AssetBundleName  
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            if(name.Length >0)
                assetImporter.assetBundleName = name[0];
            else
                assetImporter.assetBundleName = AssertName;
            assetImporter.assetBundleVariant = "sdf";
            EditorUtility.DisplayProgressBar("自动化命名","正在处理 :"+ assetPath, i * 1.0f / ob.Length);
            
        }
    }

    /// <summary>
    /// 选中资源的鼠标回调函数
    /// </summary>
    private void OnSelectionChange()
    {
        CheckGameObjectChoose();
    }

    private void CheckGameObjectChoose()
    {
        ob = Selection.assetGUIDs;
        //GetAssetPath();
    }

    /// <summary>
    /// 获取鼠标选中文件与资源的路径
    /// </summary>
    private void GetAssetPath()
    {
        for (int i = 0; i < ob.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(ob[i]);
            string[] SplitString = path.Split('/');
            string AssertName = SplitString[SplitString.Length - 1];
            string[] name = AssertName.Split('.');
            Debug.Log(path);
            Debug.Log(SplitString[ SplitString.Length - 1 ]);
            Debug.Log(name[0]);
        }
    }

    /// <summary>
    /// 重新刷新面板
    /// </summary>
    void OnInspectorUpdate()
    {
        this.Repaint();
    }

    /// <summary>  
    /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包  
    /// 之前说过，只要设置了AssetBundleName的，都会进行打包，不论在什么目录下  
    /// </summary>  
    void ClearAssetBundlesName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        //Debug.Log("GetAllAssetBundleNames :"+length);
        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }

        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
        length = AssetDatabase.GetAllAssetBundleNames().Length;
        //Debug.Log("ClearAssetBundlesName" + length);
    }
}
