using ShipDock.Applications;

namespace FWGame
{
    public class FWRole : RoleEntitas, IFWRole
    {
        public int Camp { get; set; }

        protected override int[] ComponentIDs { get; } = new int[]
        {
            FWConsts.COMPONENT_ROLE_INPUT,
            FWConsts.COMPONENT_ROLE_CAMP,
            FWConsts.COMPONENT_POSITION,
            FWConsts.COMPONENT_ROLE_COLLIDER,
            FWConsts.COMPONENT_ROLE_MUST
        };
    }
}
