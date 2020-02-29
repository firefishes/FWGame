using ShipDock.Applications;
using ShipDock.ECS;

namespace FWGame
{
    public class RoleBehavior : ShipDockComponent
    {
            
    }

    public class RoleNormalEnterSceneBehavior : RoleBehavior
    {
        private RoleEntitas mRole;

        public override void Execute(int time, ref IShipDockEntitas target)
        {
            base.Execute(time, ref target);

            mRole = target as RoleEntitas;
            if(mRole.Gravity)
            {
                mRole.Gravity = true;
            }
        }
    }
}
