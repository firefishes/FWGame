﻿using System;
using ShipDock.Notices;
using ShipDock.Pooling;
using ShipDock.Server;
using UnityEngine;

namespace FWGame
{
    public class FWServer : Server
    {
        public FWServer()
        {
            ServerName = FWConsts.SERVER_FW;
        }

        protected override void Purge()
        {
            base.Purge();
        }

        public override void InitServer()
        {
            base.InitServer();
            
            Register<INotice>(NoticeResolver, Pooling<Notice>.Instance);
            Register<INotice>(GameNoticeResolver, Pooling<GameNotice>.Instance);
            Register<IParamNotice<FWInputer>>(FWInputerParamerResolver, Pooling<ParamNotice<FWInputer>>.Instance);
        }

        [Resolvable("Notice")]
        private void NoticeResolver(ref INotice target)
        {
        }

        [Resolvable("GameNotice")]
        private void GameNoticeResolver(ref INotice target)
        {
        }

        [Resolvable("FWInputerParamer")]
        private void FWInputerParamerResolver(ref IParamNotice<FWInputer> target)
        {
            target.ParamValue = MainInputer;
        }

        public override void ServerReady()
        {
            base.ServerReady();

            Add<IParamNotice<FWInputer>>(SetFWInputer);
            //SetResolver<INotice>("TestResolver", OnTestResolver);
            //SetResolver<INotice>("TestResolver2", OnTestResolver2);
        }

        [Callable("SetFWInputer", "FWInputerParamer")]
        private void SetFWInputer<I>(ref I target)
        {
            IParamNotice<FWInputer> notice = target as IParamNotice<FWInputer>;
            MainInputer = notice.ParamValue;
        }

        private FWInputer MainInputer { get; set; }
    }

}