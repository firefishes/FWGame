using System;
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
        }

        [Resolvable("Notice")]
        private void NoticeResolver(ref INotice target)
        {
        }

        [Resolvable("GameNotice")]
        private void GameNoticeResolver(ref INotice target)
        {
        }

        public override void ServerReady()
        {
            base.ServerReady();

            //SetResolver<INotice>("TestResolver", OnTestResolver);
            //SetResolver<INotice>("TestResolver2", OnTestResolver2);
        }

    }

}