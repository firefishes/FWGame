using System.Collections.Generic;
using ShipDock.ECS;
using ShipDock.Tools;

namespace FWGame
{
    public class RoleColliderComponent : ShipDockComponent
    {
        private IFWRole mRole;
        private IFWRole mRoleCollidingTarget;
        private List<int> mRoleColliding;
        private KeyValueList<int, IFWRole> mRoleColliderMapper;

        public override void Init()
        {
            base.Init();

            mRoleColliderMapper = new KeyValueList<int, IFWRole>();
        }

        public override int SetEntitas(IShipDockEntitas target)
        {
            int id = base.SetEntitas(target);
            if(id >= 0)
            {
                IFWRole role = target as IFWRole;
                int subgroupID = role.RoleMustSubgroup.roleColliderID;
                mRoleColliderMapper[subgroupID] = role;
            }
            return id;
        }

        protected override void FreeEntitas(int mid, ref IShipDockEntitas entitas, out int statu)
        {
            base.FreeEntitas(mid, ref entitas, out statu);

            if(statu == 0)
            {
                IFWRole role = entitas as IFWRole;
                int colliderID = role.RoleMustSubgroup.roleColliderID;
                mRoleColliderMapper.Remove(colliderID);
            }
        }

        public override void Execute(int time, ref IShipDockEntitas target)
        {
            base.Execute(time, ref target);

            int blockID;
            mRole = target as IFWRole;

            bool isGetEnemy = false;
            mRoleColliding = mRole.CollidingRoles;
            int max = mRoleColliding.Count;
            if (max > 0)
            {
                for (int i = 0; i < max; i++)
                {
                    blockID = mRoleColliding[i];
                    mRoleCollidingTarget = mRoleColliderMapper[blockID];
                    if (mRoleCollidingTarget != default)
                    {
                        if(mRoleCollidingTarget == mRole.EnemyMainLockDown)
                        {
                            isGetEnemy = true;
                        }
                    }
                }
                if(isGetEnemy)
                {
                    mRole.FindngPath = false;
                    mRole.SpeedCurrent = 0;
                }
                //else
                //{
                //    mRole.EnemyMainLockDown = default;
                //    mRole.FindngPath = true;//TODO 锁定最近的敌人
                //    mRole.SpeedCurrent = mRole.Speed;
                //}
            }
        }
    }
}

