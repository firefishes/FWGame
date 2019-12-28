using ShipDock.Interfaces;
using System;

namespace ShipDock.Applications
{
    public class ComponentBridge : IDispose
    {
        private Action mOnStarted;

        public void Dispose()
        {
            mOnStarted = default;
        }

        public ComponentBridge(Action callback = null)
        {
            mOnStarted = callback;
        }

        public void Start()
        {
            ShipDockApp.AppInstance.AddStart(OnAppStart);
        }

        private void OnAppStart()
        {
            ShipDockApp.AppInstance.Servers.AddOnServerFinished(mOnStarted);
        }
    }

}
