﻿
using ShipDock.Interfaces;
using ShipDock.Pooling;

namespace ShipDock.Server
{
    public interface IServer : IDispose
    {
        void InitServer();
        void ServerReady();
        void SetServerHolder(IServersHolder servers);
        int Register<InterfaceT>(ResolveDelegate<InterfaceT> target, params IPoolBase[] factory);
        void Unregister<InterfaceT>(string alias);
        InterfaceT Resolve<InterfaceT>(string alias,  string resolverName = "");
        void Add<InterfaceT>(ResolveDelegate<InterfaceT> target);
        InterfaceT Delive<InterfaceT>(string resolverName, string alias);
        IServersHolder ServersHolder { get; }
        int Prioriity { get; set; }
        string ServerName { get; }
    }
}