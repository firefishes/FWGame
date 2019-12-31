using ShipDock.Applications;
using ShipDock.Datas;
using ShipDock.Notices;
using ShipDock.Pooling;
using ShipDock.Server;
using ShipDock.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWGame
{
    public class FWDataServer : Server
    {
        private FWGameData mGameData;
        private RoleCampComponent mRoleCampComponent;

        public FWDataServer()
        {
            ServerName = FWConsts.SERVER_FW_DATAS;
        }

        public override void InitServer()
        {
            base.InitServer();
            
            ShipDockApp app = ShipDockApp.AppInstance;
            var datas = app.Datas;
            datas.AddData(new FWGameData());

            Register<IParamNotice<IFWRole>>(CampRoleCreated, Pooling<CampRoleNotice>.Instance);
            
        }
        
        [Resolvable("CampRoleCreated")]
        private void CampRoleCreated(ref IParamNotice<IFWRole> target)
        {
            target.ParamValue = mRoleCampComponent.RoleCreated;
        }

        public override void ServerReady()
        {
            base.ServerReady();

            Add<IParamNotice<IFWRole>>(AddCampRole);

            mGameData = ShipDockApp.AppInstance.Datas.GetData<FWGameData>(FWConsts.DATA_GAME);

            ShipDockApp app = ShipDockApp.AppInstance;
            var components = app.Components;
            mRoleCampComponent = components.GetComponentByAID(FWConsts.COMPONENT_ROLE_CAMP) as RoleCampComponent;
        }

        [Callable("AddCampRole", "CampRoleCreated")]
        private void AddCampRole(ref IParamNotice<IFWRole> target)
        {
            Debug.Log(target.ParamValue);
            mGameData.AddCampRole(target.ParamValue);
        }
    }

    public class FWGameData : Data
    {
        private KeyValueList<int, CampRoleModel> mCampRoleMapper;

        public FWGameData() : base(FWConsts.DATA_GAME)
        {
            mCampRoleMapper = new KeyValueList<int, CampRoleModel>();
        }

        public void AddCampRole(IFWRole role)
        {
            int key = mCampRoleMapper.Size;
            CampRoleModel model = new CampRoleModel
            {
                role = role,
                controllIndex = key
            };
            mCampRoleMapper[key] = model;
        }
    }

    public class CampRoleModel
    {
        public int controllIndex;
        public IFWRole role;
    }

    public class CampRoleNotice : ParamNotice<IFWRole>
    {

    }

}