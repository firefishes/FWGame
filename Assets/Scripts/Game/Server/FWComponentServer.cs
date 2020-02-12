using System;
using ShipDock.Applications;
using ShipDock.Notices;
using ShipDock.Pooling;
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

            Register<IParamNotice<IFWRole>>(SetUserFWRoleResolver, Pooling<ParamNotice<IFWRole>>.Instance);

            CreateComponents();
        }

        private void CreateComponents()
        {
            ShipDockApp app = ShipDockApp.Instance;
            var components = app.Components;
            components.CreateComponent<RoleMustComponent>(FWConsts.COMPONENT_ROLE_MUST);
            components.CreateComponent<RoleCampComponent>(FWConsts.COMPONENT_ROLE_CAMP);
            components.CreateComponent<UserInputComponent>(FWConsts.COMPONENT_ROLE_INPUT);
            components.CreateComponent<FWPositionComponent>(FWConsts.COMPONENT_POSITION);
            components.CreateComponent<RoleNormalEnterSceneBehavior>(FWConsts.COMPONENT_ROLE_NORMAL_ENTER_SCENE);
            components.CreateComponent<RoleColliderComponent>(FWConsts.COMPONENT_ROLE_COLLIDER);
        }

        public override void ServerReady()
        {
            base.ServerReady();

            Add<IParamNotice<IFWRole>>(SetUserFWRole);
        }

        [Callable("SetUserFWRole", "SetUserFWRole")]
        private void SetUserFWRole(ref IParamNotice<IFWRole> target)
        {
            IParamNotice<IFWRole> notice = target as IParamNotice<IFWRole>;
        }

        [Resolvable("SetUserFWRole")]
        private void SetUserFWRoleResolver<I>(ref I target) { }

    }

}
