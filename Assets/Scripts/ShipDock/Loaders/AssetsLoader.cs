using ShipDock.Applications;
using ShipDock.Interfaces;
using ShipDock.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShipDock.Loader
{
    public class AssetsLoader : IDispose
    {

        private int mIndex;
        private Loader mLoader;
        private LoaderOpertion mCurrentOption;
        private Queue<LoaderOpertion> mOpertions;
        private List<string> mDependences;

        public AssetsLoader()
        {
            mOpertions = new Queue<LoaderOpertion>();
            mLoader = Loader.GetAssetBundleLoader();
            mLoader.CompletedEvent.AddListener(OnCompleted);

            if (ShipDockApp.AppInstance.IsStarted)
            {
                ABs = ShipDockApp.AppInstance.ABs;
            }
            else
            {
                ABs = new AssetBundles();
            }
            AessetManifest = ABs.GetManifest();
        }

        public void Dispose()
        {
            Utils.Reclaim(mLoader);

            mLoader = default;
        }

        public AssetsLoader LoadRemote(string url)
        {
            if (mLoader != default)
            {
                LoaderOpertion opertion = new LoaderOpertion()
                {
                    url = url
                };
                mOpertions.Enqueue(opertion);
            }
            return this;
        }

        public AssetsLoader Load(string relativeName, string manifest)
        {
            if (mLoader != default)
            {
                LoaderOpertion opertion = new LoaderOpertion()
                {
                    manifestName = manifest,
                    relativeName = relativeName,
                    isManifest = true,
                };
                mOpertions.Enqueue(opertion);
            }
            return this;
        }

        public AssetsLoader Load(string relativeName, bool isDependenciesLoader = true)
        {
            if(mLoader != default)
            {
                LoaderOpertion opertion = new LoaderOpertion()
                {
                    relativeName = relativeName,
                    isGetDependencies = isDependenciesLoader
                };
                mOpertions.Enqueue(opertion);
            }
            return this;
        }

        public void Load(out int statu)
        {
            statu = 0;
            if ((mOpertions != default) && (mOpertions.Count > 0) && (mCurrentOption == default))
            {
                mCurrentOption = mOpertions.Dequeue();

                string source = mCurrentOption.relativeName;
                if (mCurrentOption.isGetDependencies)
                {
                    string[] list = AessetManifest.GetDirectDependencies(source);
                    if(mDependences != default)
                    {
                        Utils.Reclaim(ref mDependences);
                    }
                    mIndex = 0;
                    mDependences = new List<string>(list)
                    {
                        source
                    };

                    source = mDependences[mIndex];
                    source = AppPaths.StreamingResDataRoot.Append(source);//TODO 根据版本号决定是缓存目录还是项目目录获取
                    mIndex++;
                }
                else if(!mCurrentOption.isManifest)
                {
                    source = mCurrentOption.url;
                }
                mLoader.Load(source);
            }
            else
            {
                statu = 1;
            }
        }

        private void OnCompleted(bool isSuccessd, Loader target)
        {
            if(isSuccessd)
            {
                LoadSuccessd(ref target);
            }
        }

        private void LoadSuccessd(ref Loader target)
        {
            if (mCurrentOption.isManifest)
            {
                GetAssetManifest(ref target);
            }
            else if(mCurrentOption.isGetDependencies)
            {
                GetNextDependencies(ref target);
            }
            else
            {
                GetRemote(ref target);
            }

            Load(out int statu);
            if(statu == 1)
            {
                CompleteEvent?.Invoke(true, mLoader);
            }
        }

        private void GetRemote(ref Loader target)
        {
            ABs.Add(target.Assets);
            //TODO 版本控制
            mCurrentOption = default;
        }

        private void GetNextDependencies(ref Loader target)
        {
            ABs.Add(target.Assets);
            if(mIndex < mDependences.Count)
            {
                string source = mDependences[mIndex];
                mLoader.Load(AppPaths.StreamingResDataRoot.Append(source));//TODO 根据版本号决定是缓存目录还是项目目录获取
                mIndex++;
            }
            else
            {
                mCurrentOption = default;
            }
        }

        private void GetAssetManifest(ref Loader target)
        {
            ABs.Add(mCurrentOption.manifestName, target.Assets);
            AessetManifest = ABs.GetManifest();
            mCurrentOption = default;
        }
        
        public string[] DirectDependencies { get; private set; }
        public OnAssetLoaderCompleted CompleteEvent { get; private set; } = new OnAssetLoaderCompleted();
        public AssetBundleManifest AessetManifest { get; private set; }
        public AssetBundles ABs { get; private set; }
    }
}
