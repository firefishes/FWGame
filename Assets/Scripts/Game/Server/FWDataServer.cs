using ShipDock.Applications;
using ShipDock.Datas;
using ShipDock.ECS;
using ShipDock.Notices;
using ShipDock.Pooling;
using ShipDock.Server;
using ShipDock.Tools;
using UnityEngine;

namespace FWGame
{
    public class FWDataServer : Server
    {
        private ServerComponentDataRelater mRelater;

        public FWDataServer()
        {
            ServerName = FWConsts.SERVER_FW_DATAS;
            mRelater = new ServerComponentDataRelater
            {
                DataNames = new int[]
            {
                FWConsts.DATA_GAME
            },
                ComponentNames = new int[]
            {
                FWConsts.COMPONENT_ROLE_CAMP
            }
            };

        }

        public override void InitServer()
        {
            base.InitServer();
            
            ShipDockApp app = ShipDockApp.AppInstance;
            var datas = app.Datas;
            datas.AddData(new FWGameData());

            Register<IParamNotice<IFWRole>>(CampRoleCreated, Pooling<CampRoleNotice>.Instance);

        }

        public override void ServerReady()
        {
            base.ServerReady();
            
            mRelater.CommitCache();

            Add<IParamNotice<IFWRole>>(AddCampRole);
        }
        
        [Resolvable("CampRoleCreated")]
        private void CampRoleCreated(ref IParamNotice<IFWRole> target)
        {
            var component = mRelater.ComponentRef<RoleCampComponent>(FWConsts.COMPONENT_ROLE_CAMP);
            target.ParamValue = component.RoleCreated;
        }

        [Callable("AddCampRole", "CampRoleCreated")]
        private void AddCampRole(ref IParamNotice<IFWRole> target)
        {
            Debug.Log(target.ParamValue);
            var data = mRelater.DataRef<FWGameData>(FWConsts.DATA_GAME);
            data.AddCampRole(target.ParamValue);
        }
    }

    public class ServerComponentDataRelater
    {
        private KeyValueList<int, IData> mDataCached;
        private KeyValueList<int, IShipDockComponent> mCompCached;

        public void CommitCache()
        {
            if (mCompCached == default)
            {
                mCompCached = new KeyValueList<int, IShipDockComponent>();
            }
            if(mDataCached == default)
            {
                mDataCached = new KeyValueList<int, IData>();
            }
            ShipDockApp app = ShipDockApp.AppInstance;
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