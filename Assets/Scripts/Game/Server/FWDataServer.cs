using ShipDock.Applications;
using ShipDock.Datas;
using ShipDock.Notices;
using ShipDock.Pooling;
using ShipDock.Server;
using UnityEngine;

namespace FWGame
{
    public class FWDataServer : DataServer
    {
        private int mRoleIndex;

        public FWDataServer() : base()
        {
        }

        public override void InitServer()
        {
            base.InitServer();
            
            Register<IParamNotice<ICommonRole>>(CampRoleCreated, Pooling<CampRoleNotice>.Instance);
            Register<IParamNotice<ICommonRole>>(SetUserFWRoleResolver, Pooling<ParamNotice<ICommonRole>>.Instance);

        }

        public override void ServerReady()
        {
            base.ServerReady();
            
            Add<IParamNotice<ICommonRole>>(AddCampRole);
            Add<IParamNotice<ICommonRole>>(SetUserFWRole);
        }

        [Resolvable("SetUserFWRole")]
        private void SetUserFWRoleResolver<I>(ref I target) { }

        [Resolvable("CampRoleCreated")]
        private void CampRoleCreated(ref IParamNotice<ICommonRole> target)
        {
            var component = mRelater.ComponentRef<FWRoleCampComponent>(FWConsts.COMPONENT_ROLE_CAMP);
            target.ParamValue = component.RoleCreated;
        }

        [Callable("AddCampRole", "CampRoleCreated")]
        private void AddCampRole(ref IParamNotice<ICommonRole> target)
        {
            Debug.Log(target.ParamValue);
            var data = mRelater.DataRef<FWGameData>(FWConsts.DATA_GAME);
            data.AddCampRole(target.ParamValue as IFWRole);
            target.ParamValue.Name = "Role_" + mRoleIndex;
            mRoleIndex++;
        }

        [Callable("SetUserFWRole", "SetUserFWRole")]
        private void SetUserFWRole(ref IParamNotice<ICommonRole> target)
        {
            IParamNotice<ICommonRole> notice = target as IParamNotice<ICommonRole>;
            IFWRole role = notice.ParamValue as IFWRole;

            FWPlayerData data =  mRelater.DataRef<FWPlayerData>(FWConsts.DATA_PLAYER);
            data.SetCurrentRole(role);
        }

        public override int[] RelatedDataNames { get; } = new int[]
        {
            FWConsts.DATA_GAME,
            FWConsts.DATA_PLAYER
        };

        public override int[] RelatedComponentNames { get; } = new int[]
        {
            FWConsts.COMPONENT_ROLE_CAMP
        };

        public override IData[] DataList { get; } = new IData[]
        {
            new FWGameData(),
            new FWPlayerData()
        };

        public override string DataServerName { get; } = FWConsts.SERVER_FW_DATAS;
    }

}