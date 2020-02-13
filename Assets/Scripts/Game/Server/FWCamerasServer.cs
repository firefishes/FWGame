using ShipDock.Applications;
using ShipDock.Datas;
using ShipDock.Notices;
using ShipDock.Pooling;
using ShipDock.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWGame
{
    public class FWCamerasServer : Server, IDataExtracter
    {
        private ServerRelater mRelater;
        private FWCamerLens mLens;

        public FWCamerasServer()
        {
            ServerName = FWConsts.SERVER_FW_LENS;

            mRelater = new ServerRelater()
            {
                DataNames = new int[]
                {
                    FWConsts.DATA_PLAYER
                }
            };
        }

        public override void InitServer()
        {
            base.InitServer();

            Register<IParamNotice<FWCamerLens>>(SetLensParamer, Pooling<ParamNotice<FWCamerLens>>.Instance);
            Register<IParamNotice<Role>>(PlayerRoleChoosenParamer, Pooling<ParamNotice<Role>>.Instance);

        }

        [Resolvable("PlayerRoleChoosen")]
        private void PlayerRoleChoosenParamer(ref IParamNotice<Role> target) { }

        [Resolvable("SetLensParamer")]
        private void SetLensParamer(ref IParamNotice<FWCamerLens> target) { }

        public override void ServerReady()
        {
            base.ServerReady();

            mRelater.CommitRelate();
            FWPlayerData playerData = mRelater.DataRef<FWPlayerData>(FWConsts.DATA_PLAYER);
            playerData.Register(this);

            Add<IParamNotice<FWCamerLens>>(SetLens);
            Add<IParamNotice<Role>>(SetChoosenPlayer);
        }

        [Callable("SetChoosenPlayer", "PlayerRoleChoosen")]
        private void SetChoosenPlayer<I>(ref I target)
        {
            Role role = (target as IParamNotice<Role>).ParamValue;
            Transform tf = mLens.CameraFollower.transform;
            tf.SetParent(role.CameraNode);
            tf.localPosition = Vector3.zero;
            tf.localRotation = role.CameraNode.localRotation;
        }

        [Callable("SetLens", "SetLensParamer")]
        private void SetLens<I>(ref I target)
        {
            mLens = (target as IParamNotice<FWCamerLens>).ParamValue;
        }

        public void OnDataChanged(IData data, int keyName)
        {
            switch(keyName)
            {
                case FWConsts.DC_PLAYER_ROLE_CHOOSEN:
                    FWPlayerData playerData = data as FWPlayerData;
                    IParamNotice<int> paramNotice = Resolve<IParamNotice<int>>("IntParamer");
                    paramNotice.ParamValue = FWConsts.NOTICE_PLAYER_ROLE_CHOOSEN;
                    playerData.PlayerCurrentRole.SourceID.Dispatch(paramNotice);
                    break;
            }
        }
    }
}
