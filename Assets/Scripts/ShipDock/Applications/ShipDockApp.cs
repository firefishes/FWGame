using ShipDock.ECS;
using ShipDock.Notices;
using ShipDock.Server;
using ShipDock.Testers;
using ShipDock.Tools;
using System;

namespace ShipDock.Applications
{
    public static class ShipDockAppExtension
    {
        public static void Add(this int target, Action<INoticeBase<int>> handler)
        {
            ShipDockApp.Instance.Notificater?.Add(target, handler);
        }

        public static void Add(this INotificationSender target, Action<INoticeBase<int>> handler)
        {
            ShipDockApp.Instance.Notificater?.Add(target, handler);
        }

        public static void Remove(this int target, Action<INoticeBase<int>> handler)
        {
            ShipDockApp.Instance.Notificater?.Remove(target, handler);
        }

        public static void Remove(this INotificationSender target, Action<INoticeBase<int>> handler)
        {
            ShipDockApp.Instance.Notificater?.Remove(target, handler);
        }

        public static void Dispatch(this int noticeName, INoticeBase<int> notice = default)
        {
            if(notice == default)
            {
                notice = new Notice();
            }
            notice.SetNoticeName(noticeName);
            ShipDockApp.Instance.Notificater?.SendNotice(notice);
            notice.Dispose();
        }

        public static T GetServer<T>(this string serverName) where T : IServer 
        {
            return ShipDockApp.Instance.Servers.GetServer<T>(serverName);
        }
    }

    public class ShipDockApp : Singletons<ShipDockApp>
    {

        public static ShipDockApp AppInstance
        {
            get
            {
                return Instance;
            }
        }

        public static void StartUp(int ticks, Action onStartUp = default)
        {
            if(onStartUp != default)
            {
                Instance.AddStart(onStartUp);
            }
            Instance.Start(ticks);
        }

        public static void CallLater(Action<int> method)
        {
            Instance.TicksUpdater?.CallLater(method);
        }

        public static void Close()
        {
            Instance.Clean();
        }

        private Action mAppStarted;

        public void Start(int ticks)
        {
            Tester.Instance.Log(TesterBaseApp.LOG, IsStarted, "warning: ShipDockApplication has started");
            
            if (IsStarted)
            {
                return;
            }

            Notificater = new Notifications<int>();
            Servers = new Servers();
            Servers.OnInit += OnCreateComponentManager;

            if (ticks > 0)
            {
                TicksUpdater = new TicksUpdater(ticks);
            }

            ShipDockConsts.NOTICE_APPLICATION_STARTUP.Dispatch();

            IsStarted = true;
            mAppStarted?.Invoke();
            mAppStarted = null;
        }

        private void OnCreateComponentManager()
        {
            Components = new ShipDockComponentManager();

            MethodUpdater updater = new MethodUpdater
            {
                Update = ComponentUpdateByTicks
            };
            UpdaterNotice notice = new UpdaterNotice();
            notice.ParamValue = updater;
            ShipDockConsts.NOTICE_ADD_UPDATE.Dispatch(notice);
            notice.Dispose();
        }

        private int mFrameSign;
        private void ComponentUpdateByTicks(int time)
        {
            Components.UpdateComponentUnit(ComponentUnitUpdate);
            if(mFrameSign > 0)
            {
                Components.FreeComponentUnit(ComponentUnitUpdate);
            }
            mFrameSign++;
            if(mFrameSign > 1)
            {
                mFrameSign = 0;
            }
        }

        private void ComponentUnitUpdate(Action<int> method)
        {
            TicksUpdater.CallLater(method);
        }

        public void Clean()
        {
            IsStarted = false;

            Utils.Reclaim(Notificater);
            TicksUpdater?.Dispose();

            Notificater = null;
            TicksUpdater = null;
        }

        public void AddStart(Action method)
        {
            if (IsStarted)
            {
                method();
            }
            else
            {
                mAppStarted += method;
            }
        }

        public bool IsStarted { get; private set; }
        public TicksUpdater TicksUpdater { get; private set; }
        public Notifications<int> Notificater { get; private set; }
        public ShipDockComponentManager Components { get; private set; }
        public Servers Servers { get; private set; }
    }
}
