using ShipDock.ECS;
using ShipDock.Tools;
using UnityEngine;

namespace FWGame
{
    public class RoleMustComponent : ShipDockComponent
    {

        private IFWRole mRoleItem = default;
        private KeyValueList<IFWRole, int> mRigidbodySubgourp;
        private KeyValueList<IFWRole, int> mRoleColliderSubgourp;
        private KeyValueList<IFWRole, int> mAnimatorSubgourp;

        public override int SetEntitas(IShipDockEntitas target)
        {
            int id = base.SetEntitas(target);

            if (id >= 0)
            {
                mRoleItem = target as IFWRole;

                FWRoleMustSubgroup subgroup = mRoleItem.RoleMustSubgroup;
                SetSubgroupMap(ref mRigidbodySubgourp, subgroup.rigidbodyID);
                SetSubgroupMap(ref mRoleColliderSubgourp, subgroup.roleColliderID);
                SetSubgroupMap(ref mAnimatorSubgourp, subgroup.animatorID);
            }

            return id;
        }

        protected override void FreeEntitas(int mid, ref IShipDockEntitas entitas, out int statu)
        {
            base.FreeEntitas(mid, ref entitas, out statu);

            mRoleItem = entitas as IFWRole;

            RemoveSubgroupMap(ref mRigidbodySubgourp, ref mRoleItem);
            RemoveSubgroupMap(ref mRoleColliderSubgourp, ref mRoleItem);
            RemoveSubgroupMap(ref mAnimatorSubgourp, ref mRoleItem);
        }

        private void SetSubgroupMap(ref KeyValueList<IFWRole, int> mapper, int mid)
        {
            if(mapper == default)
            {
                mapper = new KeyValueList<IFWRole, int>();
            }
            if(!mapper.ContainsKey(mRoleItem))
            {
                Debug.Log(mid);
                mapper[mRoleItem] = mid;
            }
        }

        private void RemoveSubgroupMap(ref KeyValueList<IFWRole, int> mapper, ref IFWRole target)
        {
            if (mapper.ContainsKey(mRoleItem))
            {
                mapper.Remove(target);
            }
        }

        public int GetRigidbody(ref IFWRole target)
        {
            return mRigidbodySubgourp.ContainsKey(target) ? mRigidbodySubgourp[target] : default;
        }

        public int GetCollider(ref IFWRole target)
        {
            return mRoleColliderSubgourp.ContainsKey(target) ? mRoleColliderSubgourp[target] : default;
        }

        public int GetAnimator(ref IFWRole target)
        {
            return mAnimatorSubgourp.ContainsKey(target) ? mAnimatorSubgourp[target] : default;
        }
    }

}
