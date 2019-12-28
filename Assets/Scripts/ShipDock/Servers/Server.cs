using ShipDock.Pooling;

namespace ShipDock.Server
{
    public delegate void ResolveDelgate<T>(ref T target);

    public class Server : IServer
    {
        public Server(string serverName = "")
        {
            if(!string.IsNullOrEmpty(ServerName))
            {
                ServerName = serverName;
            }
        }

        public virtual void Dispose()
        {
            Purge();
        }

        protected virtual void Purge()
        {

        }

        public virtual void InitServer()
        {
        }

        public virtual void ServerReady()
        {
        }

        public int Register<InterfaceT>(ResolveDelgate<InterfaceT> target, params IPoolBase[] factory)
        {
            IResolvable[] list = ServersHolder.SetResolvable(target, out int statu);
            int max = list != default ? list.Length : 0;
            if(max > 0)
            {
                IResolvable resolvable;
                IPoolBase factoryItem;
                int factoryCount = factory != default ? factory.Length : 0;
                for (int i = 0; i < max; i++)
                {
                    factoryItem = factoryCount > i ? factory[i] : default;
                    resolvable = list[i];
                    resolvable.InitResolver<InterfaceT>(ServersHolder, factoryItem);
                }
            }
            return statu;
        }

        public void Unregister<InterfaceT>(string alias)
        {
            //TODO 注销容器方法
        }

        public InterfaceT Resolve<InterfaceT>(string alias, string resolverName = "")
        {
            int resultError;
            InterfaceT result = default;
            IResolvable resolvable = ServersHolder.GetResolvable(ref alias, out resultError);
            if (resultError == 0)
            {
                IResolverHandler resolverHandler = resolvable.GetResolver<InterfaceT>(Resolvable.RESOLVER_DEF, out _);
                resolverHandler.InvokeResolver();
                result = (InterfaceT)resolverHandler.ResolverParam;

                resolverHandler = resolvable.GetResolver<InterfaceT>(Resolvable.RESOLVER_CRT, out _);
                resolverHandler.SetParam(ref result);
                resolverHandler.InvokeResolver();

                result = (InterfaceT)resolverHandler.ResolverParam;

                if ((result != default) && !string.IsNullOrEmpty(resolverName))
                {
                    resolverHandler = resolvable.GetResolver<InterfaceT>(resolverName, out _);
                    resolverHandler.SetParam(ref result);
                    resolverHandler.InvokeResolver();
                }
            }
            return result;
        }

        public void SetServerHolder(IServersHolder servers)
        {
            ServersHolder = servers;
        }

        public int Prioriity { get; set; }
        public IServersHolder ServersHolder { get; private set; }
        public virtual string ServerName { get; protected set; }
    }

}
