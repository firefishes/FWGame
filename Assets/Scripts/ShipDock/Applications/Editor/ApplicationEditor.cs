using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShipDock.Editors
{
    public class ApplicationEditor : ShipDockEditor
    {
        [MenuItem("ShipDock/Applicaton Editor")]
        public static void Init()
        {
            InitEditorWindow<ApplicationEditor>("游戏客户端设置");
        }

        private BuildTarget mABBuildTarget;

        public override void Preshow()
        {
            base.Preshow();
        }

        protected override void InitConfigFlagAndValues()
        {
            base.InitConfigFlagAndValues();


        }

        protected override void CheckGUI()
        {
            base.CheckGUI();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("AB资源包");
            EditorGUILayout.Space();

            UpdateABBuildPanelUI();

            EditorGUILayout.EndHorizontal();
        }

        private void UpdateABBuildPanelUI()
        {
            //Func<BuildTarget, bool, string> buildABFunc = GetABBuildOutputFunc();
            //if (GUILayout.Button("IOS AB"))
            //{
            //    mApplyStrings["build_ab_output"] = buildABFunc(BuildTarget.iOS, false);
            //    AutoSetAssetBundle.OnABBuilt = OnBuildABFinished;
            //    ConfirmPopup("IOS AB", "你确定？", AutoSetAssetBundle.BuildIOSAB, "就是干");
            //}
            //if (GUILayout.Button("OSX AB"))
            //{
            //    mApplyStrings["build_ab_output"] = buildABFunc(BuildTarget.StandaloneOSX, false);
            //    AutoSetAssetBundle.OnABBuilt = OnBuildABFinished;
            //    ConfirmPopup("OSX AB", "你确定？", AutoSetAssetBundle.BuildOSXAB, "就是干");
            //}
            //if (GUILayout.Button("Android AB"))
            //{
            //    mApplyStrings["build_ab_output"] = buildABFunc(BuildTarget.Android, false);
            //    AutoSetAssetBundle.OnABBuilt = OnBuildABFinished;
            //    ConfirmPopup("Android AB", "你确定？", AutoSetAssetBundle.BuildAndroidAB, "就是干");
            //}
            //if (GUILayout.Button("Win AB"))
            //{
            //    mApplyStrings["build_ab_output"] = buildABFunc(BuildTarget.StandaloneWindows, false);
            //    AutoSetAssetBundle.OnABBuilt = OnBuildABFinished;
            //    ConfirmPopup("Win AB", "你确定？", AutoSetAssetBundle.BuildWinAB, "就是干");
            //}
            //if (GUILayout.Button("Win64 AB"))
            //{
            //    mApplyStrings["build_ab_output"] = buildABFunc(BuildTarget.StandaloneWindows64, false);
            //    AutoSetAssetBundle.OnABBuilt = OnBuildABFinished;
            //    ConfirmPopup("Win64 AB", "你确定？", AutoSetAssetBundle.BuildWin64AB, "就是干");
            //}
            //if (GUILayout.Button("Delete all AB"))
            //{
            //    ConfirmPopup("Delete all AB", "你确定？", AutoSetAssetBundle.DelAssetBundle, "绝不反悔", logWhenInvoked: "资源已删除");
            //}
        }

        protected Func<BuildTarget, bool, string> GetABBuildOutputFunc()
        {
            Func<BuildTarget, bool, string> buildABFunc = (BuildTarget buildTarget, bool isFinish) =>
            {
                //HasBuildAbOutput = true;
                mABBuildTarget = buildTarget;
                return (isFinish) ? mABBuildTarget.ToString().Append("资源打包完成...(*^o^*)") : "即将开始资源打包，平台：".Append(mABBuildTarget.ToString(), "\n");
            };
            return buildABFunc;
        }

        protected override void ReadyClientValues()
        {
        }

        protected override void UpdateClientValues()
        {
        }
    }

}
