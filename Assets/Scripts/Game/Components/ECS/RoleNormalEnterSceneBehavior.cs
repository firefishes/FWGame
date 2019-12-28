using ShipDock.ECS;

namespace FWGame
{
    public class RoleBehavior : ShipDockComponent
    {
            
    }

    public class RoleNormalEnterSceneBehavior : RoleBehavior
    {
        private FWRole mRole;

        public override void Execute(int time, ref IShipDockEntitas target)
        {
            base.Execute(time, ref target);

            mRole = target as FWRole;
            if(mRole.Gravity)
            {
                mRole.Gravity = true;
            }
        }
    }
}
