using UnityEngine;

namespace ShipDock.Applications
{
    public class ResPrefabBridge : ResBridge, IResPrefabBridge
    {
        protected override void Awake()
        {
            base.Awake();

            Prefab = Assets.Get(m_ABName, m_AssetName);
        }

        public GameObject Prefab { get; private set; }
    }

}