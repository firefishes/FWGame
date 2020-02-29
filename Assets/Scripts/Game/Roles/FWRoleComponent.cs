
using ShipDock.Applications;
using ShipDock.Notices;
using ShipDock.Pooling;
using UnityEngine;

namespace FWGame
{
    public class FWRoleComponent : RoleComponent
    {
        [SerializeField]
        private int m_Camp;

        override protected void InitRoleData()
        {
            mRoleData = new FWRoleData();
            m_Camp = UnityEngine.Random.Range(0, 2);
        }

        protected override void SetRoleEntitas()
        {
            mRole = new BananaRole();
        }

        override protected void OnInited()
        {
            base.OnInited();

            SetRoleCamp();
        }

        private void SetRoleCamp()
        {
            (mRole as FWRole).Camp = m_Camp;
            if (m_Camp == 0)
            {
                ParamNotice<RoleEntitas> notice = Pooling<ParamNotice<RoleEntitas>>.From();
                notice.ParamValue = mRole;
                FWConsts.COMPONENT_ROLE_CONTROLLABLE.Dispatch(notice);
                Pooling<ParamNotice<RoleEntitas>>.To(notice);
            }
        }

        override protected void OnRoleNotices(INoticeBase<int> obj)
        {
            IParamNotice<int> notice = obj as IParamNotice<int>;
            switch(notice.ParamValue)
            {
                case FWConsts.NOTICE_PLAYER_ROLE_CHOOSEN:
                    FWConsts.SERVER_FW_LENS.DeliveParam<FWCamerasServer, FWRoleComponent>("SetChoosenPlayer", "PlayerRoleChoosen", OnRoleChoosen);
                    break;
            }
        }

        private void OnRoleChoosen(ref IParamNotice<FWRoleComponent> target)
        {
            (target as IParamNotice<FWRoleComponent>).ParamValue = this;
        }
    }
}

