
using UnityEngine;

namespace ShipDock.Loader
{
    public class DemoAssetComponent : MonoBehaviour
    {
        public bool valid = true;
        public string ABName;
        public string subAssetName;
        public GameObject asset;
        public Texture2D tex2D;
        public Sprite sprite;
        public AudioClip audioClip;
        public TextAsset textData;

        private void Awake()
        {
            if(!valid)
            {
                return;
            }
            DemoAssetCoordinator comp = GetComponent<DemoAssetCoordinator>();
            comp.Add(this);
        }

        public T GetAsset<T>() where T : Object
        {
            T result = default(T);
            if (typeof(T) == typeof(GameObject))
            {
                result = asset as T;
            }
            else if (typeof(T) == typeof(Texture2D))
            {
                result = tex2D as T;
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                result = audioClip as T;
            }
            else if (typeof(T) == typeof(Sprite))
            {
                result = sprite as T;
            }
            else if(typeof(T) == typeof(TextAsset))
            {
                result = textData as T;
            }
            else
            {
                //Tester.Instance.Log(TesterAssets.Instance, TesterAssets.LOG17, subAssetName);
            }
            return result;
        }
    }

}