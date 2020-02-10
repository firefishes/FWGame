using ShipDock.Notices;
using ShipDock.Server;

public static class ServerExtension
{
    public static I Delive<S, I>(this string serverName, string resolverName, string alias) where S : IServer
    {
        S server = serverName.GetServer<S>();
        return server.Delive<I>(resolverName, alias);
    }

    public static P DeliveParam<S, P>(this string serverName, string resolverName, string alias, ResolveDelegate<IParamNotice<P>> resolver = default) where S : IServer
    {
        S server = serverName.GetServer<S>();
        if(resolver != default)
        {
            server.Add(resolver, true);
        }
        IParamNotice<P> notice = server.Delive<IParamNotice<P>>(resolverName, alias);
        return notice.ParamValue;
    }
}
