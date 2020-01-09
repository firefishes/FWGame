
using System.Collections.Generic;
using UnityEngine;

namespace ShipDock.Loader
{
    public class DemoAssetComponent : MonoBehaviour
    {
        [SerializeField]
        private bool m_Valid;
        [SerializeField]
        private string m_ABName;
        [SerializeField]
        private List<DemoAsset> m_Assets;

        private void Awake()
        {
            if(!m_Valid)
            {
                return;
            }
            DemoAssetCoordinator comp = GetComponent<DemoAssetCoordinator>();
            comp.Add(this);
        }

        public List<DemoAsset> Assets
        {
            get
            {
                return m_Assets;
            }
        }
    }

}