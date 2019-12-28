using ShipDock.Applications;
using ShipDock.Server;

namespace FWGame
{
    public class FWComponentServer : Server
    {
        public FWComponentServer()
        {
            ServerName = FWConsts.SERVER_FW_COMPONENTS;
        }

        public override void InitServer()
        {
            base.InitServer();

            ShipDockApp app = ShipDockApp.AppInstance;
            var components = app.Components;
            components.CreateComponent<RoleMustComponent>(FWConsts.COMPONENT_ROLE_MUST);
            components.CreateComponent<RoleCampComponent>(FWConsts.COMPONENT_ROLE_CAMP);
            components.CreateComponent<UserInputComponent>(FWConsts.COMPONENT_ROLE_INPUT);
            components.CreateComponent<FWPositionComponent>(FWConsts.COMPONENT_POSITION);
            components.CreateComponent<RoleNormalEnterSceneBehavior>(FWConsts.COMPONENT_ROLE_NORMAL_ENTER_SCENE);
            components.CreateComponent<RoleColliderComponent>(FWConsts.COMPONENT_ROLE_COLLIDER);
        }
    }

}
