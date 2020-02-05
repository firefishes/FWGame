using UnityEngine;

namespace ShipDock.Applications
{
    public class ResSpriteBridge : ResBridge, IResSpriteBridge
    {
        protected override void Awake()
        {
            base.Awake();

            Sprite = Assets.Get<Sprite>(m_ABName, m_AssetName);
            Texture = Sprite.texture;
        }

        public Sprite Sprite { get; private set; }
        public Texture Texture { get; private set; }
    }
}

