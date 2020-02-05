using ShipDock.Datas;
using ShipDock.ECS;
using ShipDock.Tools;

namespace ShipDock.Applications
{
    public class ServerRelater
    {
        private KeyValueList<int, IData> mDataCached;
        private KeyValueList<int, IShipDockComponent> mCompCached;

        public void CommitCache()
        {
            if (mCompCached == default)
            {
                mCompCached = new KeyValueList<int, IShipDockComponent>();
            }
            if (mDataCached == default)
            {
                mDataCached = new KeyValueList<int, IData>();
            }
            ShipDockApp app = ShipDockApp.Instance;
            int max = ComponentNames.Length;
            int name;
            var components = app.Components;
            for (int i = 0; i < max; i++)
            {
                name = ComponentNames[i];
                mCompCached[name] = components.GetComponentByAID(name);
            }
            max = DataNames.Length;
            if (max > 0)
            {
                var datas = app.Datas;
                for (int i = 0; i < max; i++)
                {
                    name = DataNames[i];
                    mDataCached[name] = datas.GetData<IData>(name);
                }
            }
        }

        public T ComponentRef<T>(int componentName) where T : IShipDockComponent
        {
            return mCompCached != default ? (T)mCompCached[componentName] : default;
        }

        public T DataRef<T>(int dataName) where T : IData
        {
            return mDataCached != default ? (T)mDataCached[dataName] : default;
        }

        public int[] DataNames { get; set; }
        public int[] ComponentNames { get; set; }
    }
}