using ShipDock.Applications;
using ShipDock.Notices;
using ShipDock.Pooling;
using ShipDock.Server;

namespace FWGame
{
    public class FWServer : MainServer
    {
        
        public FWServer() : base(FWConsts.SERVER_FW)
        {
        }

        protected override void Purge()
        {
            base.Purge();
        }

        public override void InitServer()
        {
            base.InitServer();
            
            Register<INotice>(GameNoticeResolver, Pooling<GameNotice>.Instance);
            
        }
        
        [Resolvable("GameNotice")]
        private void GameNoticeResolver(ref INotice target) { }
        
        public override void ServerReady()
        {
            base.ServerReady();

            //SetResolver<INotice>("TestResolver", OnTestResolver);
            //SetResolver<INotice>("TestResolver2", OnTestResolver2);
        }
    }

}