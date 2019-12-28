namespace ShipDock.Server
{
    public class ResolverHandler<InterfaceT> : IResolverHandler, 
                                               IResolverCacher<InterfaceT>, 
                                               IResolverParamer<InterfaceT>
    {
        private InterfaceT mParamTemp;

        public void Dispose()
        {
            mParamTemp = default;
            ResolverParam = default;
            DelegateTarget = default;
        }

        public void SetDelegate(ResolveDelgate<InterfaceT> target)
        {
            DelegateTarget = target;
        }

        public void InvokeResolver()
        {
            mParamTemp = (InterfaceT)ResolverParam;
            DelegateTarget(ref mParamTemp);
            ResolverParam = mParamTemp;
        }

        public void SetParam<T>(ref T param)
        {
            ResolverParam = param;
        }

        public ResolveDelgate<InterfaceT> DelegateTarget { get; private set; }
        public object ResolverParam { get; set; }
    }

}
