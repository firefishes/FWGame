using UnityEngine;

namespace ShipDock.Applications
{
    public class ResPrefabBridge : ResBridge, IResPrefabBridge
    {
        protected override void Awake()
        {
            base.Awake();

            GameObject source = Assets.Get(m_ABName, m_AssetName);
            Prefab = Instantiate(source);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Prefab = default;
        }

        public GameObject Prefab { get; private set; }
    }

}