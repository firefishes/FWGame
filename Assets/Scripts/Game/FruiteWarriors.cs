
#define G_LOG

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

        private MethodUpdater updater;

        public void UIRootAwaked(IUIRoot root)
        {
            ShipDockApp.Instance.InitUIRoot(root);
        }

        void Start()
        {
            ShipDockApp.StartUp(120, OnShipDockStart);
        }

        private void OnShipDockStart()
        {
            Tester.Instance.SetDefaultTester(FWTester.Instance);
            Tester.Instance.Init(FWTester.Instance);
            Tester.Instance.Log(FWTester.LOG0, "ShipDock start up..");

            ShipDockConsts.NOTICE_SCENE_UPDATE_READY.Add(OnSceneUpdateReady);

            ShipDockApp app = ShipDockApp.Instance;
            app.AddStart(OnAppStarted);

            Servers servers = app.Servers;
            servers.OnInit += OnServersInit;
            servers.Add(new FWServer());
            servers.Add(new FWDataServer());
            servers.Add(new FWComponentServer());
            servers.Add(new FWCamerasServer());
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

        private void CheckServerInited(int obj)
        {
            if(ShipDockApp.Instance.Servers.IsServersReady)
            {
                UpdaterNotice.RemoveSceneUpdater(updater);
                
                AssetsLoader assetsLoader = new AssetsLoader();
                assetsLoader.CompleteEvent.AddListener(OnPreloadComplete);
                assetsLoader
                    .Add(AppPaths.StreamingResDataRoot.Append(AppPaths.resData), FWConsts.ASSET_RES_DATA)
                    .Add(FWConsts.ASSET_UI_MAIN)
                    .Add(FWConsts.ASSET_UI_ROLE_CHOOSER)
                    .Add(FWConsts.ASSET_RES_BRIGEDS)
                    .Add(FWConsts.ASSET_BANANA_ROLE)
                    .Load(out _);
            }
        }

        private void OnPreloadComplete(bool successed, Loader target)
        {
            AssetBundles ABs = ShipDockApp.Instance.ABs;
            GameObject prefab = ABs.Get(FWConsts.ASSET_RES_BRIGEDS, "BananaRoleRes");
            GameObject role;
            int max = 10;
            for (int i = 0; i < max; i++)
            {
                role = Instantiate(prefab);
            }

            UIManager uis = ShipDockApp.Instance.UIs;
            uis.Open<RoleChooser>(FWConsts.UI_NAME_ROLE_CHOOSER);
        }

        private void OnServersInit()
        {
            ShipDockApp app = ShipDockApp.Instance;
            app.Servers.AddResolvableConfig(FWConsts.ServerConfigs);
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

        private void OnDestroy()
        {
            ShipDockApp.Close();
        }
    }

}
