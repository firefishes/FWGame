﻿using ShipDock.Tools;
using ShipDock.UI;
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
                    if (RoleCardMapper.ContainsKey(id))
                    {
                        RoleCardMapper[id].RoleCardData = item;
                    }
                    else
                    {
                        roleCard = UIPooling.FromComponentPool(FWConsts.POOL_UI_ROLE_CARD, ref m_UIRoleCardRaw);
                        roleCard.RoleCardData = item;
                        roleCard.RoleCardSelectedEvent.AddListener(OnRoleCardSelectedHandler);
                        roleCard.SetParent(m_UIRoleCardList);
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
        }

        public KeyValueList<int, UIRoleCard> RoleCardMapper { get; private set; } = new KeyValueList<int, UIRoleCard>();
        public CampRoleModel SelectedRoleModel { get; private set; }
    }
}