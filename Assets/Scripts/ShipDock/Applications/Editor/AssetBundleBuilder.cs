#define SHOW_MENU_IN_EDITOR

using ShipDock.Applications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ShipDock.Editors
{
    public class AssetBundleBuilder
    {
        public Action OnABBuilt;

        /// <summary>
        /// 批量删除AB包文件
        /// </summary>
        [MenuItem("ShipDock/Delete Asset Bundles")]
        public static void DelAssetBundle()
        {

            DeleteABRes(AppPaths.DataPathResDataRoot);

            string resRoot = AppPaths.StreamingResDataRoot;
            string platformRoot = GetSuffix(BuildTarget.Android);
            DeleteABRes(resRoot + platformRoot);
            platformRoot = GetSuffix(BuildTarget.iOS);
            DeleteABRes(resRoot + platformRoot);
            platformRoot = GetSuffix(BuildTarget.StandaloneOSX);
            DeleteABRes(resRoot + platformRoot);
            platformRoot = GetSuffix(BuildTarget.StandaloneWindows);
            DeleteABRes(resRoot + platformRoot);
            platformRoot = GetSuffix(BuildTarget.StandaloneWindows64);
            DeleteABRes(resRoot + platformRoot);
        }

        #region 资源打包相关
#if SHOW_MENU_IN_EDITOR
        [MenuItem("ShipDock/Build Asset Bundle/IOS")]
#endif
        public static void BuildIOSAB()
        {
            Debug.Log("Start build IOS asset bundles");
            AssetBundleBuilder builder = new AssetBundleBuilder();
            builder.BuildAssetBundle(BuildTarget.iOS);
        }

#if SHOW_MENU_IN_EDITOR
        [MenuItem("ShipDock/Build Asset Bundle/ANDROID")]
#endif
        public static void BuildAndroidAB()
        {
            Debug.Log("Start build Android asset bundles");
            AssetBundleBuilder builder = new AssetBundleBuilder();
            builder.BuildAssetBundle(BuildTarget.Android);
        }

#if SHOW_MENU_IN_EDITOR
        [MenuItem("ShipDock/Build Asset Bundle/OSX")]
#endif
        public static void BuildOSXAB()
        {
            Debug.Log("Start build OSX asset bundles");
            AssetBundleBuilder builder = new AssetBundleBuilder();
            builder.BuildAssetBundle(BuildTarget.StandaloneOSX);
        }

#if SHOW_MENU_IN_EDITOR
        [MenuItem("ShipDock/Build Asset Bundle/WIN")]
#endif
        public static void BuildWinAB()
        {
            Debug.Log("Start build Win asset bundles");
            AssetBundleBuilder builder = new AssetBundleBuilder();
            builder.BuildAssetBundle(BuildTarget.StandaloneWindows);
        }

#if SHOW_MENU_IN_EDITOR
        [MenuItem("ShipDock/Build Asset Bundle/WIN64")]
#endif
        public static void BuildWin64AB()
        {
            Debug.Log("Start build win64 asset bundles");
            AssetBundleBuilder builder = new AssetBundleBuilder();
            builder.BuildAssetBundle(BuildTarget.StandaloneWindows64);
        }
        #endregion

        public static string GetSuffix(BuildTarget buildPlatform)
        {
            string result = string.Empty;
            switch (buildPlatform)
            {
                case BuildTarget.Android:
                    result = "_ANDROID/";
                    break;
                case BuildTarget.iOS:
                    result = "_IOS/";
                    break;
                case BuildTarget.StandaloneWindows:
                    result = "_WIN/";
                    break;
                case BuildTarget.StandaloneWindows64:
                    result = "_WIN64/";
                    break;
                case BuildTarget.StandaloneOSX:
                    result = "_OSX/";
                    break;
                default:
                    result = "_UNKNOWN";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 资源打包
        /// </summary>
        public void BuildAssetBundle(BuildTarget buildPlatform)
        {
            DirectoryInfo[] dirScenesDIRArray = null;
            AssetDatabase.RemoveUnusedAssetBundleNames();

            string rootPath = AppPaths.DataPathResDataRoot;
            if (!Directory.Exists(rootPath))
            {
                EditorUtility.DisplayDialog("警告！", "所需资源目录不存在：" + rootPath, "确定");
                return;
            }
            string[] assetLabelRoots = Directory.GetDirectories(rootPath);
            if (assetLabelRoots.Length == 0)
            {
                EditorUtility.DisplayDialog("提示！", "没有需要打包的资源！！！", "确定");
                return;
            }

            string outputRoot = AppPaths.StreamingResDataRoot;
            string platformPath = GetSuffix(buildPlatform);

            string tmpScenesDIR = string.Empty;
            string tmpScenesName = string.Empty;
            string abOutputPath = string.Empty;

            AssetBundleInfoPopupEditor.Popup();
            AssetBundleInfoPopupEditor infoPopup = EditorWindow.focusedWindow as AssetBundleInfoPopupEditor;
            
            UnityEngine.Object[] selections = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
            int max = selections.Length;
            for (int i = 0; i < max; i++)
            {
                Debug.Log(AssetDatabase.GetAssetPath(selections[i]));

                infoPopup.SetValueItem("res_" + i.ToString(), AssetDatabase.GetAssetPath(selections[i]));
            }
            infoPopup.ResList = selections;

            return;
            FileSystemInfo dirInfo = null;

            List<AssetImporter> fileList = new List<AssetImporter>();
            for (int i = 0; i < assetLabelRoots.Length; i++)
            {
                DirectoryInfo dirTempInfo = new DirectoryInfo(assetLabelRoots[i]);
                dirScenesDIRArray = dirTempInfo.GetDirectories();

                for (int j = 0; j < dirScenesDIRArray.Length; j++)
                {
                    tmpScenesDIR = rootPath + dirScenesDIRArray[j].Name;
                    int tmpIndex = tmpScenesDIR.LastIndexOf("/", StringComparison.Ordinal);
                    tmpScenesName = tmpScenesDIR.Substring(tmpIndex + 1);
                    dirInfo = dirScenesDIRArray[j];
                    JudgeDIRorFileByRecursive(ref dirInfo, tmpScenesName, ref fileList);
                }
                abOutputPath = outputRoot + platformPath + dirTempInfo.Name;
                if (!Directory.Exists(abOutputPath))
                {
                    Directory.CreateDirectory(abOutputPath);
                }

                BuildPipeline.BuildAssetBundles(abOutputPath, BuildAssetBundleOptions.None, buildPlatform);

                fileList.ForEach(info => info.assetBundleName = string.Empty);
                fileList.Clear();
            }

            //提示信息
            if (EditorUtility.DisplayDialog("提示", string.Format("资源打包完成!!!"), "OK"))
            {
                AssetDatabase.Refresh();
            }

            if (OnABBuilt != null)
            {
                OnABBuilt.Invoke();
                OnABBuilt = null;
            }
        }

        private static void DeleteABRes(string strNeedDeleteDIR)
        {
            if (!string.IsNullOrEmpty(strNeedDeleteDIR))
            {
                if (!Directory.Exists(strNeedDeleteDIR))
                {
                    return;
                }

                if (File.Exists(strNeedDeleteDIR + ".meta"))
                {
                    File.Delete(strNeedDeleteDIR + ".meta");
                }
                //注意： 这里参数"true"表示可以删除非空目录
                Directory.Delete(strNeedDeleteDIR, true);
                //刷新
                AssetDatabase.Refresh();
            }
        }

        /// <summary>递归判断是否为目录与文件，修改AssetBundle 的标记(lable)</summary>
        private static void JudgeDIRorFileByRecursive(ref FileSystemInfo fileSysInfo, string scenesName, ref List<AssetImporter> assetList)
        {
            if (!fileSysInfo.Exists)
            {
                Debug.LogError("文件或者目录名称： " + fileSysInfo + " 不存在，请检查");
                return;
            }

            //得到当前目录下一级的文件信息集合
            DirectoryInfo dirInfoObj = fileSysInfo as DirectoryInfo;//文件信息转换为目录信息
            FileSystemInfo[] fileSysArray = dirInfoObj.GetFileSystemInfos();
            FileSystemInfo item = null;
            foreach (FileSystemInfo fileInfo in fileSysArray)
            {
                FileInfo fileinfoObj = fileInfo as FileInfo;
                //文件类型
                if (fileinfoObj != null)
                {
                    //修改此文件的AssetBundle标签
                    SetFileABLabel(fileinfoObj, scenesName, assetList);
                }
                //目录类型
                else
                {
                    item = fileInfo;
                    //如果是目录则递归调用
                    JudgeDIRorFileByRecursive(ref item, scenesName, ref assetList);
                }
            }
        }

        /// <summary>
        /// 对指定的文件设置“AB包名称”
        /// </summary>
        private static void SetFileABLabel(FileInfo fileinfoObj, string scenesName, List<AssetImporter> assetList)
        {
            //参数检查（*.meta 文件不做处理）
            if ((fileinfoObj.Extension == ".meta") || (fileinfoObj.Extension == ".DS_Store"))
            {
                return;
            }

            //得到AB包名称
            string strABName = GetABName(fileinfoObj, scenesName);
            //获取资源文件的相对路径
            int tmpIndex = fileinfoObj.FullName.IndexOf("Assets", System.StringComparison.Ordinal);
            string strAssetFilePath = fileinfoObj.FullName.Substring(tmpIndex);//得到文件相对路径
            //给资源文件设置AB名称以及后缀
            AssetImporter tmpImporterObj = AssetImporter.GetAtPath(strAssetFilePath);
            tmpImporterObj.assetBundleName = string.Empty;
            if (assetList != null)
            {
                assetList.Add(tmpImporterObj);
            }
            tmpImporterObj.assetBundleName = strABName;//这里的字符串需要替换
            if (fileinfoObj.Extension == ".unity")
            {
                tmpImporterObj.assetBundleVariant = "u3d";
            }
            else
            {
                tmpImporterObj.assetBundleVariant = "ab";
            }
            //tmpImporterObj.SaveAndReimport();
        }

        /// <summary>
        /// 获取AB包的名称
        /// </summary>
        /// <param name="fileinfoObj">文件信息</param>
        /// <param name="scenesName">场景名称</param>
        /// AB 包形成规则：
        ///     文件AB包名称=“所在二级目录名称”（场景名称）+“三级目录名称”（下一级的“类型名称”）
        /// <returns>
        /// 返回一个合法的“AB包名称”
        /// </returns>
        private static string GetABName(FileInfo fileinfoObj, string scenesName)
        {
            //返回AB包名称
            string strABName = string.Empty;
            //Win路径
            string tmpWinPath = fileinfoObj.FullName;//文件信息的全路径（Win格式）
            //Unity路径
            string tmpUnityPath = tmpWinPath.Replace("\\", "/");//替换为Unity字符串分割符
            int tmpSceneNamePostion = tmpUnityPath.IndexOf(scenesName, System.StringComparison.Ordinal) + scenesName.Length;//定位“场景名称”后面字符位置
            //AB包中“类型名称”所在区域
            string strABFileNameArea = tmpUnityPath.Substring(tmpSceneNamePostion + 1);
            if (strABFileNameArea.Contains("/"))
            {
                string[] tempStrArray = strABFileNameArea.Split('/');
                //AB包名称
                strABName = scenesName + "/" + tempStrArray[0];
            }
            else
            {
                //定义*.Unity 文件形成的特殊AB包名称
                strABName = scenesName + "/" + scenesName;
            }
            return strABName;
        }
    }

    public class ABWindow : EditorWindow
    {
        string myString = "Hello";
        bool groupEndable;
        bool myBool = true;
        float myfloat = 1.3f;

        //[MenuItem("Tools/MyWindow")]
        static void Init()
        {
            ABWindow window = EditorWindow.GetWindow(typeof(ABWindow)) as ABWindow;
            window.titleContent.text = "AssetBundleSetting";
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Base Setting", EditorStyles.boldLabel);
            myString = EditorGUILayout.TextField("textfiled", myString);

            groupEndable = EditorGUILayout.BeginToggleGroup("Option", groupEndable);
            myBool = EditorGUILayout.Toggle("Toggle", myBool);
            myfloat = EditorGUILayout.Slider("Slider", myfloat, -3, 3);
            EditorGUILayout.EndToggleGroup();
            if (GUILayout.Button("Select", EditorStyles.label))
            {
                EditorUtility.DisplayDialog("删除所有AB包文件吗？", "是否删除文件？", "是", "否");
            }
        }
    }
}
