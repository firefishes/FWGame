﻿using ShipDock.Applications;
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

            CreateComponents();
        }

        private void CreateComponents()
        {
            ShipDockApp app = ShipDockApp.Instance;
            var components = app.Components;
            components.Create<RoleMustComponent>(FWConsts.COMPONENT_ROLE_MUST);
            components.Create<FWRoleCampComponent>(FWConsts.COMPONENT_ROLE_CAMP);
            components.Create<FWUserInputComponent>(FWConsts.COMPONENT_ROLE_INPUT);
            components.Create<PositionComponent>(FWConsts.COMPONENT_POSITION);
            components.Create<RoleNormalEnterSceneBehavior>(FWConsts.COMPONENT_ROLE_NORMAL_ENTER_SCENE);
            components.Create<RoleColliderComponent>(FWConsts.COMPONENT_ROLE_COLLIDER);
        }

        public override void ServerReady()
        {
            base.ServerReady();

        }

    }

}
