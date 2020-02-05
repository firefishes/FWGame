using UnityEngine;

namespace ShipDock.Applications
{
    public class ResTextureBridge : ResBridge, IResTextureBridge
    {
        protected override void Awake()
        {
            base.Awake();

            Texture = Assets.Get<Texture>(m_ABName, m_AssetName);
        }

        public Texture Texture { get; private set; }
    }
}

