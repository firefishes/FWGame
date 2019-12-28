using ShipDock.Pooling;
using ShipDock.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ShipDock.Server
{
    public class Resolvable : IResolvable
    {
        public const string RESOLVER_DEF = "def";
        public const string RESOLVER_CRT = "crt";

        private static readonly Type[] defaultGenericType = new Type[0];
        private static readonly object[] defaultGenericParam = new object[0];

        private ConstructorInfo mDefaultConstructorInfo;
        private Func<Type[], ConstructorInfo> mConstructor;
        private List<IResolverHandler> mResolvers;
        private IntegerMapper<string> mResolverIDMapper;

        public void Dispose()
        {
            Binder = default;
            InstanceFactory = default;
            mConstructor = default;
            mDefaultConstructorInfo = default;
            Utils.Reclaim(ref mResolvers);
            Utils.Reclaim(mResolverIDMapper);
        }

        public void InitResolver<InterfaceT>(IServersHolder serverHolder, IPoolBase pool = default)
        {
            InstanceFactory = pool;
            ResolveType = Binder.GetInstanceType(serverHolder);
            mConstructor = ResolveType.GetConstructor;
            mDefaultConstructorInfo = mConstructor(defaultGenericType);

            if(mResolverIDMapper == default)
            {
                mResolverIDMapper = new IntegerMapper<string>();
            }
            SetResolver<InterfaceT>(RESOLVER_DEF, OnResolverDefault, out _);
        }

        private void OnResolverDefault<T>(ref T param)
        {
            if(mDefaultConstructorInfo == default)
            {
                return;
            }
            param = (InstanceFactory == default) ? 
                        (T)mDefaultConstructorInfo.Invoke(defaultGenericParam) : 
                        (T)InstanceFactory.GetInstance();
        }

        public void Binding(ref ResolvableBinder target)
        {
            if (default == Binder)
            {
                Binder = target;
                mResolvers = new List<IResolverHandler>();
            }
        }

        public void SetResolver<InterfaceT>(string resolverName, ResolveDelgate<InterfaceT> resolveDelgate, out int statu)
        {
            statu = 0;
            bool hasRef = mResolverIDMapper.ContainsKey(ref resolverName, out int index);
            if(hasRef)
            {
                statu = 1;
            }
            else
            {
                ResolverHandler<InterfaceT> handler = new ResolverHandler<InterfaceT>();
                handler.SetDelegate(resolveDelgate);
                mResolvers.Add(handler);

                mResolverIDMapper.Add(resolverName, out _);
            }
        }

        public IResolverHandler GetResolver<InterfaceT>(string resolverName, out int statu)
        {
            statu = 0;
            IResolverHandler handler = default;
            bool hasRef = mResolverIDMapper.ContainsKey(ref resolverName, out int index);
            if (hasRef)
            {
                handler = mResolvers[index];
            }
            else
            {
                statu = 1;
            }
            return handler;
        }

        public int ID
        {
            get
            {
                return (default != Binder) ? Binder.BinderID : int.MaxValue;
            }
        }

        public Type ResolveType { get; private set; }
        public ResolvableBinder Binder { get; private set; }
        public IPoolBase InstanceFactory { get; private set; }
    }
}