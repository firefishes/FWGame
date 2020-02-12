using ShipDock.Applications;
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
            Register<IParamNotice<FWInputer>>(SetFWInputerParamer, Pooling<ParamNotice<FWInputer>>.Instance);
            Register<IParamNotice<FWInputer>>(GetFWInputerParamer, Pooling<ParamNotice<FWInputer>>.Instance);
        }

        [Resolvable("Notice")]
        private void NoticeResolver(ref INotice target) { }

        [Resolvable("GameNotice")]
        private void GameNoticeResolver(ref INotice target) { }

        [Resolvable("SetFWInputerParamer")]
        private void SetFWInputerParamer(ref IParamNotice<FWInputer> target) { }

        [Resolvable("FWInputerParamer")]
        private void GetFWInputerParamer(ref IParamNotice<FWInputer> target)
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

        [Callable("SetFWInputer", "SetFWInputerParamer")]
        private void SetFWInputer<I>(ref I target)
        {
            IParamNotice<FWInputer> notice = target as IParamNotice<FWInputer>;
            MainInputer = notice.ParamValue;
            Debug.Log("Main inputer is set.");
            
        }

        public FWInputer MainInputer { get; private set; }
    }

}