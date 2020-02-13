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
                    FWConsts.DATA_GAME,
                    FWConsts.DATA_PLAYER
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
            
            ShipDockApp app = ShipDockApp.Instance;
            var datas = app.Datas;
            datas.AddData(new FWGameData());
            datas.AddData(new FWPlayerData());

            Register<IParamNotice<IFWRole>>(CampRoleCreated, Pooling<CampRoleNotice>.Instance);
            Register<IParamNotice<IFWRole>>(SetUserFWRoleResolver, Pooling<ParamNotice<IFWRole>>.Instance);

        }

        public override void ServerReady()
        {
            base.ServerReady();
            
            mRelater.CommitRelate();

            Add<IParamNotice<IFWRole>>(AddCampRole);
            Add<IParamNotice<IFWRole>>(SetUserFWRole);
        }

        [Resolvable("SetUserFWRole")]
        private void SetUserFWRoleResolver<I>(ref I target) { }

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

        [Callable("SetUserFWRole", "SetUserFWRole")]
        private void SetUserFWRole(ref IParamNotice<IFWRole> target)
        {
            IParamNotice<IFWRole> notice = target as IParamNotice<IFWRole>;
            IFWRole role = notice.ParamValue;

            FWPlayerData data =  mRelater.DataRef<FWPlayerData>(FWConsts.DATA_PLAYER);
            data.SetCurrentRole(role);
        }
    }

}