using ShipDock.Applications;
using ShipDock.Notices;
using ShipDock.Pooling;
using ShipDock.Server;
using UnityEngine;

namespace FWGame
{
    public class FWDataServer : Server
    {
        private ServerRelater mRelater;

        public FWDataServer()
        {
            ServerName = FWConsts.SERVER_FW_DATAS;
            mRelater = new ServerRelater
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

}