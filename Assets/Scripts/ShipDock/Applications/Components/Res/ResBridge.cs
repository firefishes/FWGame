using ShipDock.Loader;
using UnityEngine;

namespace ShipDock.Applications
{
    public class ResBridge : MonoBehaviour, IResBridge
    {
        [SerializeField]
        protected string m_ABName;
        [SerializeField]
        protected string m_AssetName;

        protected virtual void Awake()
        {
            Assets = ShipDockApp.Instance.ABs;
        }

        protected virtual void OnDestroy()
        {
            Assets = default;
        }

        protected IAssetBundles Assets { get; set; }
    }

}
