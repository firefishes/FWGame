using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShipDock.Editors
{
    public class AssetBundleInfoPopupEditor : ShipDockEditor
    {
        public static void Popup()
        {
            InitEditorWindow<AssetBundleInfoPopupEditor>("资源包信息");//, new Rect(0, 0, 400, 400));
        }

        protected override void InitConfigFlagAndValues()
        {
            base.InitConfigFlagAndValues();

            SetValueItem("abName", string.Empty);
            SetValueItem("abName_set", string.Empty);
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
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            int max = ResList.Length;
            string fieldValue;
            for (int i = 0; i < max; i++)
            {
                if(GUILayout.Button(ResList[i].name))
                {
                    fieldValue = GetValueItem("res_" + i).Value;
                    GetValueItem("abName_set").Change(fieldValue);
                    //Debug.Log(GetValueItem("abName_set").Value);
                }
            }
            ValueItemLabel("abName_set");
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        public UnityEngine.Object[] ResList { get; set; } = new UnityEngine.Object[0];
    }

}