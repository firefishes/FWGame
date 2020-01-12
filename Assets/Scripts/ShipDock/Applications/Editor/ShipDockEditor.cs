using System;
using System.Collections.Generic;
using ShipDock.Tools;
using UnityEditor;
using UnityEngine;

namespace ShipDock.Editors
{
    public abstract class ShipDockEditor : EditorWindow
    {
        protected static void InitEditorWindow<T>(string title) where T : ShipDockEditor
        {
            ShipDockEditor window = GetWindow<T>(title);
            window.Preshow();
            window.Show();
        }

        protected KeyValueList<string, bool> mApplyValues;
        protected KeyValueList<string, string> mApplyStrings;

        private List<int> mGUIFlagKeys;
        private List<string> mFlagKeys;
        private List<string> mFlagLabels;
        private List<bool> mConfigFlagValues;

        public virtual void Preshow()
        {
            UpdateEditorAsset();
            InitConfigFlagAndValues();
        }

        protected virtual void UpdateEditorAsset()
        {
            UpdateEditorConfigAssets();
        }

        protected virtual void UpdateEditorConfigAssets()
        {
        }

        protected virtual void InitConfigFlagAndValues()
        {
            mGUIFlagKeys = new List<int>();
            mFlagKeys = new List<string>();
            mFlagLabels = new List<string>();
            mConfigFlagValues = new List<bool>();

            mApplyValues = new KeyValueList<string, bool>();

            bool value;
            int max = mFlagKeys.Count;
            for (int i = 0; i < max; i++)
            {
                value = mConfigFlagValues[i];
                CheckClientConfigApplyed(mFlagKeys[i], ref value);
            }
        }

        protected void CheckClientConfigApplyed(string flagKey, ref bool clientConfigFlag)
        {
            bool applyFlag = mApplyValues.IsContainsKey(flagKey) && mApplyValues[flagKey];
            if (applyFlag != clientConfigFlag)
            {
                mApplyValues[flagKey] = clientConfigFlag;
            }
        }

        protected void AddFlagKeyValue(string flag, ref bool configValue, string label)
        {
            mFlagKeys.Add(flag);
            mFlagLabels.Add(label);
            mConfigFlagValues.Add(configValue);
        }

        protected void NeedUpdateFlagKeyValue(string flag, ref bool configValue)
        {
            int index = mFlagKeys.IndexOf(flag);
            if (index >= 0)
            {
                mGUIFlagKeys.Add(index);
                mConfigFlagValues[index] = configValue;
            }
        }

        protected bool GetFlagKeyValue(string flag)
        {
            int index = mFlagKeys.IndexOf(flag);
            return (index >= 0) && mConfigFlagValues[index];
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            CheckGUI();
            EditorGUILayout.EndVertical();
        }

        protected virtual void CheckGUI()
        {
        }


        protected void ConfirmPopup(string titleValue, string message, Action action = null, string ok = "好的", string cancel = "取消", string log = "")
        {
            bool result;
            if (string.IsNullOrEmpty(cancel))
            {
                result = EditorUtility.DisplayDialog(titleValue, message, ok);
            }
            else
            {
                result = EditorUtility.DisplayDialog(titleValue, message, ok, cancel);
            }
            if (result)
            {
                if (action != null)
                {
                    action.Invoke();
                    if (!string.IsNullOrEmpty(log))
                    {
                        Debug.Log(log);
                    }
                }
            }
        }

        protected abstract void ReadyClientValues();
        protected abstract void UpdateClientValues();
    }
}
