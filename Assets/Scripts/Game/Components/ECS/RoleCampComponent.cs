using System;
using System.Collections.Generic;
using ShipDock.Applications;
using ShipDock.ECS;
using ShipDock.Notices;
using ShipDock.Tools;

namespace FWGame
{
    public class RoleCampComponent : ShipDockComponent
    {
        private FWDataServer mDataServer;
        private IFWRole mRoleTarget;
        private IFWRole mRoleEntitas;
        private List<int> mAllRoles;
        private KeyValueList<int, List<int>> mCampRoles;

        public override void Init()
        {
            base.Init();

            mAllRoles = new List<int>();
            mCampRoles = new KeyValueList<int, List<int>>();
            mDataServer = FWConsts.SERVER_FW_DATAS.GetServer<FWDataServer>();
        }

        public override int SetEntitas(IShipDockEntitas target)
        {
            int id = base.SetEntitas(target);
            if(id >= 0)
            {
                RoleCreated = target as IFWRole;
                int campID = RoleCreated.Camp;
                List<int> list;
                if (mCampRoles.IsContainsKey(campID))
                {
                    list = mCampRoles[campID];
                }
                else
                {
                    list = new List<int>();
                    mCampRoles[campID] = list;
                }
                list.Add(id);
                if(!mAllRoles.Contains(id))
                {
                    mAllRoles.Add(id);
                }
                //IParamNotice<IFWRole> notice = mDataServer.Resolve<IParamNotice<IFWRole>>("CampRoleCreated");
                //notice.ParamValue = role;
                mDataServer.Delive<IParamNotice<IFWRole>>("AddCampRole", "CampRoleCreated");
                RoleCreated = default;
            }
            return id;
        }

        protected override void FreeEntitas(int mid, ref IShipDockEntitas entitas, out int statu)
        {
            base.FreeEntitas(mid, ref entitas, out statu);
            if(statu == 0)
            {
                IFWRole role = entitas as IFWRole;
                int campID = role.Camp;
                List<int> list;
                if (mCampRoles.IsContainsKey(campID))
                {
                    list = mCampRoles[campID];
                    list.Remove(mid);
                }
                if (mAllRoles.Contains(mid))
                {
                    mAllRoles.Remove(mid);
                }
            }
        }

        public override void Execute(int time, ref IShipDockEntitas target)
        {
            base.Execute(time, ref target);

            mRoleTarget = target as IFWRole;
            int id;
            int max = mAllRoles.Count;
            for (int i = 0; i < max; i++)
            {
                id = mAllRoles[i];
                mRoleEntitas = GetEntitas(id) as IFWRole;
                if ((mRoleEntitas != default) && (mRoleTarget != default))
                {
                    if ((mRoleTarget.EnemyMainLockDown == default) &&
                       (mRoleTarget != mRoleEntitas) &&
                       (mRoleEntitas.Camp != mRoleTarget.Camp))
                    {
                        mRoleTarget.FindngPath = true;
                        mRoleTarget.EnemyMainLockDown = mRoleEntitas;
                        break;
                    }
                }
            }
            //mRoleTarget = mRoleEntitas.EnemyMainLockDown;
            //if (mRoleTarget != default)
            //{
            //    mRoleEntitas.SetPahterTarget(mRoleTarget.Position);
            //}
        }

        public IFWRole RoleCreated { get; private set; }
    }

}
