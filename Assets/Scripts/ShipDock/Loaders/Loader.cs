﻿
using System.Collections;
using ShipDock.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

namespace ShipDock.Loader
{
    public class Loader : IDispose
    {
        public const int LOADER_ASSETBUNDLE = 0;
        public const int LOADER_TEXT = 1;
        public const int LOADER_AUDIO_CLIP = 2;
        public const int LOADER_TEXTURE = 3;
        public const int LOADER_HTTP_GET = 4;
        public const int LOADER_HTTP_POST = 5;

        public static int retryMax = 5;

        private UnityWebRequest mRequester;
        private AsyncOperation mAsyncOperation;

        public Loader()
        {
            InitLoader(LOADER_ASSETBUNDLE);
        }

        public void Dispose()
        {
            Clear();
        }

        public void Clear()
        {
            CompletedEvent.RemoveAllListeners();

            SetUrl(string.Empty);

            mRequester = default;
            mAsyncOperation = default;
            ResultData = default;
            Assets = default;
        }

        public void InitLoader(int loadType)
        {
            Clear();
            SetLoadType(loadType);
        }

        public void Load(string url)
        {
            if (!string.IsNullOrEmpty(Url) && (Url != url))
            {
                SetUrl(url);
            }
            else
            {
                return;
            }

            CreateLoader();
            StartLoad();
        }

        private void Abort()
        {
            if (IsLoading)
            {
                mRequester.Abort();
                mAsyncOperation = default;
            }
        }

        private void CreateLoader()
        {
            if (IsLoading)
            {
                return;
            }
            switch (LoadType)
            {
                case LOADER_ASSETBUNDLE:
                    mRequester = UnityWebRequestAssetBundle.GetAssetBundle(Url);
                    break;
                case LOADER_AUDIO_CLIP:
                    break;
                case LOADER_TEXT:
                    break;
                case LOADER_TEXTURE:
                    break;
                default:
                    mRequester = new UnityWebRequest(Url)
                    {
                        downloadHandler = new DownloadHandlerBuffer()
                    };
                    break;
            }
        }

        private IEnumerator StartLoad()
        {
            if (mRequester != null)
            {
                IsLoading = true;
                SetAsync();
                yield return mAsyncOperation;

                CheckResult();
            }
            else
            {
                yield return 0;
            }
        }

        private void SetAsync()
        {
#if UNITY_5_6_5
            mAsyncOperation = mRequester.Send();
#elif UNITY_5_6_OR_NEWER
            mAsyncOperation = mRequester.SendWebRequest();
#endif
        }

        private void CheckResult()
        {
            IsLoading = false;
            if (IsError())
            {
                LoadFailed();
            }
            else
            {
                if ((mRequester.downloadHandler != default) && (mRequester.responseCode == 200))
                {
                    Loaded();
                }
                else
                {
                    RetryCount++;
                    if (RetryCount >= retryMax)
                    {
                        LoadFailed();
                    }
                    else
                    {
                        StartLoad();
                    }
                }
            }
        }

        protected virtual void Loaded()
        {
            CreateResult();
            CompletedEvent.Invoke(true, this);
        }

        private void CreateResult()
        {
            switch (LoadType)
            {
                case LOADER_ASSETBUNDLE:
                    Assets = DownloadHandlerAssetBundle.GetContent(mRequester);
                    break;
                case LOADER_TEXT:
                    break;
                case LOADER_AUDIO_CLIP:
                    break;
                case LOADER_TEXTURE:
                    break;
                default:
                    DownloadHandlerBuffer handler = mRequester.downloadHandler as DownloadHandlerBuffer;
                    ResultData = handler.data;
                    break;
            }
        }

        private bool IsError()
        {
#if UNITY_5_6_5
            return (mRequester.isError);
#elif UNITY_5_6_OR_NEWER
            return mRequester.isNetworkError || mRequester.isHttpError;
#else
            return false;
#endif
        }

        protected virtual void LoadFailed()
        {
            LoadError = mRequester.error;
            if (IsRetryAlways)
            {
                SetUrl(string.Empty);
                Load(Url);
            }
            else
            {
                CompletedEvent.Invoke(false, this);
            }
        }

        public void SetLoadType(int type)
        {
            LoadType = type;
        }

        public void SetUrl(string url)
        {
            Abort();
            Url = url;
        }

        protected int RetryCount { get; set; }

        public int LoadType { get; private set; }
        public bool IsLoading { get; private set; }
        public bool IsRetryAlways { get; set; }
        public string LoadError { get; private set; }
        public string Url { get; private set; }
        public byte[] ResultData { get; private set; }
        public AssetBundle Assets { get; private set; }
        public OnLoaderCompleted CompletedEvent { get; set; } = new OnLoaderCompleted();
    }

}