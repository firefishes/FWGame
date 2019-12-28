using System;

namespace ShipDock.ECS
{
    public interface IShipDockComponentManager
    {
        T GetEntitasWithComponents<T>(params int[] aidArgs) where T : IShipDockEntitas, new();
        void UpdateComponentUnit(Action<Action<int>> method);
        void FreeComponentUnit(Action<Action<int>> method);
        void UpdateAndFreeComponents(int time);
        int CreateComponent<T>(int aid) where T : IShipDockComponent, new();
        void RemoveComponent(IShipDockComponent target);
    }
}