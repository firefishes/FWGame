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
            //IParamNotice<IFWRole> notice = target as IParamNotice<IFWRole>;
            //IFWRole role = notice.ParamValue;
        }

        public override void ServerReady()
        {
            base.ServerReady();

            Add<IParamNotice<IFWRole>>(AddCampRole);
        }

        [Callable("AddCampRole", "CampRoleCreated")]
        private void AddCampRole(ref IParamNotice<IFWRole> target)
        {
            FWGameData data = ShipDockApp.AppInstance.Datas.GetData<FWGameData>(FWConsts.DATA_GAME);
            data.AddCampRole(target.ParamValue);
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