using ShipDock.Datas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWGame
{
    public class FWPlayerData : Data
    {
        public FWPlayerData() : base(FWConsts.DATA_PLAYER)
        {
        }

        public void SetCurrentRole(IFWRole role)
        {
            PlayerCurrentRole = role;
            DataChanged(FWConsts.DC_PLAYER_ROLE_CHOOSEN);
        }

        public IFWRole PlayerCurrentRole { get; private set; }
    }

}