using ShipDock.Applications;
using ShipDock.Notices;
using ShipDock.Server;
using ShipDock.Tools;
using ShipDock.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FWGame
{
    public class UIRoleChooser : UI
    {
        [SerializeField]
        private UIRoleCard m_UIRoleCardRaw;
        [SerializeField]
        private Transform m_UIRoleCardList;
        
        public void UpdateRolesUI(ref List<CampRoleModel> roles)
        {
            UIRoleCard roleCard;
            CampRoleModel item;
            int max = roles.Count;
            for (int i = 0; i < max; i++)
            {
                item = roles[i];
                if(item.role.Camp == 0)
                {
                    int id = item.controllIndex;
                    if(SelectedRoleModel == default)
                    {
                        OnRoleCardSelectedHandler(item);
                    }
                    if (RoleCardMapper.ContainsKey(id))
                    {
                        RoleCardMapper[id].RoleCardData = item;
                    }
                    else
                    {
                        roleCard = UIPooling.FromComponentPool(FWConsts.POOL_UI_ROLE_CARD, ref m_UIRoleCardRaw);
                        roleCard.RoleCardData = item;
                        roleCard.SetParent(m_UIRoleCardList);
                        roleCard.RoleCardSelectedEvent.AddListener(OnRoleCardSelectedHandler);
                    }
                }
            }
        }

        private void OnRoleCardSelectedHandler(CampRoleModel target)
        {
            if (SelectedRoleModel != default)
            {
                SelectedRoleModel.SetUserControll(false);
            }
            SelectedRoleModel = target;
            SelectedRoleModel.SetUserControll(true);

            FWConsts.SERVER_FW_DATAS.DeliveParam<FWDataServer, ICommonRole>("SetUserFWRole", "SetUserFWRole", OnSetUserFWRole, true);
        }

        [Resolvable("SetUserFWRole")]
        private void OnSetUserFWRole(ref IParamNotice<ICommonRole> target)
        {
            (target as IParamNotice<ICommonRole>).ParamValue = SelectedRoleModel.role;
        }

        public KeyValueList<int, UIRoleCard> RoleCardMapper { get; private set; } = new KeyValueList<int, UIRoleCard>();
        public CampRoleModel SelectedRoleModel { get; private set; }
    }
}
