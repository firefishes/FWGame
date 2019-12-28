using System;

namespace ShipDock.Applications
{
    public class MethodUpdater : IUpdate
    {

        public void AddUpdate()
        {
        }

        public void OnFixedUpdate(int dTime)
        {
        }

        public void OnLateUpdate()
        {
            Asynced = true;
        }

        public void OnUpdate(int dTime)
        {
            Update?.Invoke(dTime);
        }

        public void RemoveUpdate()
        {
        }

        public Action<int> Update { get; set; }
        public bool IsUpdate { get; set; } = true;
        public bool IsFixedUpdate { get; set; } = true;
        public bool IsLateUpdate { get; set; } = true;
        public bool Asynced { get; set; }
    }

}
