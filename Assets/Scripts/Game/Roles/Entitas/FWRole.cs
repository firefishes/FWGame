using System;
using ShipDock.Applications;
using ShipDock.Notices;

namespace FWGame
{
    public class FWRole : RoleEntitas, IFWRole
    {

        public override void CollidingChanged(int colliderID, bool isTrigger, bool isCollided)
        {
        }

        protected override int[] ComponentNames { get; } = new int[]
        {
            FWConsts.COMPONENT_ROLE_INPUT,
            FWConsts.COMPONENT_ROLE_CAMP,
            FWConsts.COMPONENT_POSITION,
            FWConsts.COMPONENT_ROLE_COLLIDER,
            FWConsts.COMPONENT_ROLE_MUST
        };
    }
}
