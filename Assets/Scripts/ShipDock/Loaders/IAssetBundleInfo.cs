using ShipDock.Interfaces;
using UnityEngine;

namespace ShipDock.Loader
{
    public interface IAssetBundleInfo : IDispose
    {
        GameObject GetAsset(string path);
        T GetAsset<T>(string path) where T : Object;
        AssetBundle Asset { get; }
        string Name { get; }
    }
}

