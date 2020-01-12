
#define G_LOG

using System;
using ShipDock.Applications;
using ShipDock.Loader;
using ShipDock.Notices;
using ShipDock.Server;
using ShipDock.Testers;
using ShipDock.UI;
using UnityEngine;

namespace FWGame
{
    public class FruiteWarriors : MonoBehaviour
    {
        void Start()
        {
            ShipDockApp.StartUp(120, OnShipDockStart);
        }

        public void UIRootAwaked(IUIRoot root)
        {
            ShipDockApp.Instance.InitUIRoot(root);
        }

        private void OnShipDockStart()
        {
            Tester.Instance.SetDefaultTester(FWTester.Instance);
            Tester.Instance.Init();
            Tester.Instance.Log(FWTester.LOG0, "ShipDock start up..");

            ShipDockConsts.NOTICE_SCENE_UPDATE_READY.Add(OnSceneUpdateReady);

            ShipDockApp app = ShipDockApp.Instance;
            app.AddStart(OnAppStarted);

            Servers servers = app.Servers;
            servers.OnInit += OnServersInit;
            servers.Add(new FWServer());
            servers.Add(new FWDataServer());
            servers.Add(new FWComponentServer());
            servers.AddOnServerFinished(OnFinished);
        }

        private void OnAppStarted()
        {
        }

        private void OnSceneUpdateReady(INoticeBase<int> obj)
        {
            updater = new MethodUpdater();
            updater.Update += CheckServerInited;
            UpdaterNotice.AddSceneUpdater(updater);
        }

        private MethodUpdater updater;

        private void CheckServerInited(int obj)
        {
            if(ShipDockApp.Instance.Servers.IsServersReady)
            {
                UpdaterNotice.RemoveSceneUpdater(updater);

                Loader loader = new Loader();
                loader.CompletedEvent.AddListener(OnComplete);
                loader.Load("https://gc-game-test-ufile.greencheng.com/20180731/AB_Res/res/map_5_6/main_thread_game_5_6.ab/f51f7c92c50c0b49e6a213b21a5e79dc.ab");
            }
        }

        private void OnServersInit()
        {
            ShipDockApp app = ShipDockApp.Instance;

            IResolvableConfig[] configs = {
                new ResolvableConfigItem<INotice, Notice>("Notice"),
                new ResolvableConfigItem<INotice, GameNotice>("GameNotice"),
                new ResolvableConfigItem<IParamNotice<IFWRole>, CampRoleNotice>("CampRoleCreated")
            };
            app.Servers.AddResolvableConfig(configs);
        }

        private void OnFinished()
        {
            #region 测试服务容器
            FWServer server = FWConsts.SERVER_FW.GetServer<FWServer>();
            INotice notice = server.Resolve<INotice>("Notice") as INotice;
            Debug.Log(notice.Name);
            notice = server.Resolve<INotice>("GameNotice") as INotice;
            Debug.Log(notice.Name);
            #endregion

        }

        private void OnComplete(bool arg0, Loader arg1)
        {
            Debug.Log(arg0 + " " + arg1.Assets);
        }

        private void OnDestroy()
        {
            ShipDockApp.Close();
        }
    }

}
