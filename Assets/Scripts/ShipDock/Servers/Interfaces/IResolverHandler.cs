using ShipDock.Interfaces;

namespace ShipDock.Server
{
    public interface IResolverHandler : IDispose
    {
        void InvokeResolver();
        void SetParam<T>(ref T param);
        object ResolverParam { get; }
        bool OnlyOnce { get; set; }
    }
}