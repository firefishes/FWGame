
#define G_LOG

using ShipDock.Applications;
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

            ShipDockApp app = ShipDockApp.Instance;
            Servers servers = app.Servers;
            servers.OnInit += OnServersInit;
            servers.AddOnServerFinished(OnFinished);
            servers.Add(new FWServer());
            servers.Add(new FWDataServer());
            servers.Add(new FWComponentServer());
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

        private void OnDestroy()
        {
            ShipDockApp.Close();
        }
    }

}
