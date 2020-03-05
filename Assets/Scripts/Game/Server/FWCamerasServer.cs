using ShipDock.Applications;
using ShipDock.Datas;
using ShipDock.Notices;
using ShipDock.Pooling;
using ShipDock.Server;

namespace FWGame
{
    public class FWCamerasServer : CamerasServer<FWCamerLens, FWPlayerData>
    {
        public FWCamerasServer() : base()
        {
        }

        public override void InitServer()
        {
            base.InitServer();

            Register<IParamNotice<FWRoleComponent>>(PlayerRoleChoosenParamer, Pooling<ParamNotice<FWRoleComponent>>.Instance);

        }

        [Resolvable("PlayerRoleChoosen")]
        private void PlayerRoleChoosenParamer(ref IParamNotice<FWRoleComponent> target) { }

        public override void ServerReady()
        {
            base.ServerReady();

            Add<IParamNotice<FWRoleComponent>>(SetChoosenPlayer);
        }

        [Callable("SetChoosenPlayer", "PlayerRoleChoosen")]
        private void SetChoosenPlayer<I>(ref I target)
        {
            FWRoleComponent role = (target as IParamNotice<FWRoleComponent>).ParamValue;
            SetCameraParent(role.CameraNode);
        }

        override public void OnDataChanged(IData data, int keyName)
        {
            base.OnDataChanged(data, keyName);

            switch (keyName)
            {
                case FWConsts.DC_PLAYER_ROLE_CHOOSEN:
                    FWPlayerData playerData = data as FWPlayerData;
                    IParamNotice<int> paramNotice = Resolve<IParamNotice<int>>("IntParamer");
                    paramNotice.ParamValue = FWConsts.NOTICE_PLAYER_ROLE_CHOOSEN;
                    int msg = playerData.PlayerCurrentRole.SourceID;
                    msg.Dispatch(paramNotice);
                    break;
            }
        }

        protected override string LensServerName { get; } = FWConsts.SERVER_FW_LENS;
        protected override int DataName { get; } = FWConsts.DATA_PLAYER;
    }
}
