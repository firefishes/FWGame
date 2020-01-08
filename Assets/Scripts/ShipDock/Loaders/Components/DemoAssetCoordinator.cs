
using ShipDock.Applications;
using System.Collections.Generic;
using UnityEngine;

namespace ShipDock.Loader
{
    public class DemoAssetCoordinator : MonoBehaviour
    {
        [SerializeField]
        private List<DemoAssetComponent> m_DemoAssets;

        private void Awake()
        {
            ComponentBridge bridge = new ComponentBridge(OnAppReady);
            bridge.Start();
        }

        private void OnDestroy()
        {
            m_DemoAssets.Clear();
        }

        public void Add(DemoAssetComponent component)
        {
            if(!m_DemoAssets.Contains(component))
            {
                m_DemoAssets.Add(component);
            }
        }

        private void OnAppReady()
        {

        }
    }

}