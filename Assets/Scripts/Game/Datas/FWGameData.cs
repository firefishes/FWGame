using System;
using System.Collections.Generic;
using ShipDock.Datas;
using ShipDock.Tools;

namespace FWGame
{

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

            DataChanged(FWConsts.DC_CAMP_ROLE_CREATED);
        }

        public List<CampRoleModel> GetCampRoleModels()
        {
            return (mCampRoleMapper != default) ? mCampRoleMapper.Values : default;
        }
    }
}

