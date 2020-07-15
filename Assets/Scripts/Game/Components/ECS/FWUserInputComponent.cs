using ShipDock.Applications;

namespace FWGame
{
    public class FWUserInputComponent : UserInputComponent<FWServer>
    {
        protected override void CheckUserInput()
        {
            base.CheckUserInput();

            mRoleItem.TargetTracking = default;
        }

        protected override string[] RelateServerNames { get; } = new string[]
        {
            FWConsts.SERVER_FW,
            FWConsts.SERVER_FW_LENS
        };

        protected override string MainServerName { get; } = FWConsts.SERVER_FW;
    }
}

