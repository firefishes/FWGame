using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ShipDock.Applications;
using ShipDock.Tools;
using UnityEditor;
using UnityEngine;

namespace ShipDock.Editors
{
    public class AssetBundleInfoPopupEditor : ShipDockEditor
    {
        public static AssetBundleInfoPopupEditor Popup()
        {
            InitEditorWindow<AssetBundleInfoPopupEditor>("资源包信息");//, new Rect(0, 0, 400, 400));
            return focusedWindow as AssetBundleInfoPopupEditor;
        }

        protected override void InitConfigFlagAndValues()
        {
            base.InitConfigFlagAndValues();

            SetValueItem("abName", string.Empty);
            SetValueItem("ab_item_name", string.Empty);
            SetValueItem("abPath", string.Empty);
        }

        protected override void ReadyClientValues()
        {
        }

        protected override void UpdateClientValues()
        {
        }

        protected override void CheckGUI()
        {
            base.CheckGUI();

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("资源包名称：");
            ValueItemTextField("abName");
            ValueItemTextField("abPath");
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            ResList = ShipDockEditorData.Instance.selections;

            CreateAssetItemWithButton();

            string abName = GetValueItem("abName").Value;
            string abPath = GetValueItem("abPath").Value;
            if (!string.IsNullOrEmpty(abName))
            {
                if(GUILayout.Button("Build"))
                {
                    List<AssetImporter> importers = new List<AssetImporter>();
                    CreateAssetImporters(ref abName, ref importers);

                    ShipDockEditorData editorData = ShipDockEditorData.Instance;
                    string output = editorData.outputRoot.Append(abPath);
                    if (!Directory.Exists(output))
                    {
                        Directory.CreateDirectory(output);
                    }
                    BuildPipeline.BuildAssetBundles(output, BuildAssetBundleOptions.None, editorData.buildPlatform);

                    if (EditorUtility.DisplayDialog("提示", string.Format("资源打包完成!!!"), "OK"))
                    {
                        foreach (var impt in importers)
                        {
                            impt.assetBundleName = string.Empty;
                            impt.assetBundleName = abName;
                        }
                        AssetDatabase.Refresh();
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        private void CreateAssetImporters(ref string abName, ref List<AssetImporter> importers)
        {
            string path;
            string assetItemName;
            string relativeName;
            FileInfo fileInfo;
            AssetImporter importer;
            int max = ResList.Length;
            for (int i = 0; i < max; i++)
            {
                assetItemName = GetValueItem("res_" + i).Value;
                relativeName = assetItemName.Replace("Assets/".Append(AppPaths.resDataRoot), string.Empty);
                path = AppPaths.DataPathResDataRoot.Append(relativeName);

                fileInfo = new FileInfo(path);
                if (fileInfo.Extension == ".cs")
                {
                    continue;
                }
                Debug.Log(fileInfo.Extension);
                importer = AssetImporter.GetAtPath(assetItemName);
                importer.assetBundleName = abName;
                importers.Add(importer);
            }
        }

        private void CreateAssetItemWithButton()
        {
            int max = ResList.Length;
            UnityEngine.Object item;
            string fieldValue;
            for (int i = 0; i < max; i++)
            {
                item = ResList[i];
                SetValueItem("res_" + i.ToString(), AssetDatabase.GetAssetPath(item));
                if (GUILayout.Button(ResList[i].name))
                {
                    fieldValue = GetValueItem("res_" + i).Value;
                    GetValueItem("ab_item_name")?.Change(fieldValue);
                }
            }
            ValueItemLabel("ab_item_name");
        }

        public UnityEngine.Object[] ResList { get; set; } = new UnityEngine.Object[0];
    }

}
