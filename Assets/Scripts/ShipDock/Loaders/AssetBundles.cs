﻿using ShipDock.Interfaces;
using ShipDock.Tools;
using UnityEngine;

namespace ShipDock.Loader
{
    public class AssetBundles : IDispose
    {
        public const string ASSET_BUNDLE_MANIFEST = "AssetBundleManifest";

        private KeyValueList<string, AssetBundleManifest> mABManifests;
        private KeyValueList<string, IAssetBundleInfo> mCaches;

        public AssetBundles()
        {
            mABManifests = new KeyValueList<string, AssetBundleManifest>();
            mCaches = new KeyValueList<string, IAssetBundleInfo>();
        }

        public void Dispose()
        {
            Utils.Reclaim(ref mCaches, false, true);
            Utils.Reclaim(ref mABManifests, false, true);
        }

        public bool HasBundel(string name)
        {
            return mCaches != default && mCaches.ContainsKey(name);
        }

        public T Get<T>(string name, string path) where T : Object
        {
            T result = default;
            if(HasBundel(name))
            {
                IAssetBundleInfo assetBundleInfo = mCaches[name];
                result = assetBundleInfo.GetAsset<T>(path);
            }
            return result;
        }

        public GameObject Get(string name, string path)
        {
            GameObject result = default;
            if (HasBundel(name))
            {
                IAssetBundleInfo assetBundleInfo = mCaches[name];
                result = assetBundleInfo.GetAsset(path);
            }
            return result;
        }

        public void Add(AssetBundle bundle)
        {
            if(bundle == default)
            {
                return;
            }
            IAssetBundleInfo info;
            string name = bundle.name;
            if (!mCaches.ContainsKey(name))
            {
                info = new AssetBundleInfo(bundle);
                mCaches[name] = info;
                mABManifests[name] = bundle.LoadAsset<AssetBundleManifest>(ASSET_BUNDLE_MANIFEST);
            }
        }

        public void Remove(string name)
        {
            RemoveBundle(ref name);
        }

        public void Remove(AssetBundle bundle)
        {
            if (bundle == default)
            {
                return;
            }
            string name = bundle.name;
            RemoveBundle(ref name);
        }

        private void RemoveBundle(ref string name)
        {
            if (mCaches.ContainsKey(name))
            {
                mABManifests.Remove(name);

                IAssetBundleInfo info = mCaches[name];
                info.Dispose();
            }
        }

        public AssetBundleManifest GetManifest(string name)
        {
            return HasBundel(name) ? mABManifests[name] : default;
        }
    }
}

