
using ShipDock.Interfaces;
using ShipDock.Pooling;

namespace ShipDock.Server
{
    public interface IServer : IDispose
    {
        void InitServer();
        void ServerReady();
        void SetServerHolder(IServersHolder servers);
        int Register<InterfaceT>(ResolveDelgate<InterfaceT> target, params IPoolBase[] factory);
        void Unregister<InterfaceT>(string alias);
        InterfaceT Resolve<InterfaceT>(string alias,  string resolverName = "");
        IServersHolder ServersHolder { get; }
        int Prioriity { get; set; }
        string ServerName { get; }
    }
}