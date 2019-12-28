using ShipDock.ECS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWGame
{
    public class RoleStateIdleComponent : ShipDockComponent
    {
        private IFWRole mRoleItem;

        public override void Execute(int time, ref IShipDockEntitas target)
        {
            base.Execute(time, ref target);

            mRoleItem = target as IFWRole;
            if(mRoleItem != default)
            {

            }
        }
    }
}
