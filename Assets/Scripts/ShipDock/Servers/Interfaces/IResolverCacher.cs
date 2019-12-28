namespace ShipDock.Server
{
    public interface IResolverCacher<InterfaceT>
    {
        void SetDelegate(ResolveDelgate<InterfaceT> target);
        ResolveDelgate<InterfaceT> DelegateTarget { get; }
    }
}