
using System;

namespace ShipDock.Server
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ResolvableAttribute : Attribute
    {

        public ResolvableAttribute(string name)
        {
            Alias = name;
        }
        
        public string Alias { get; private set; }
    }
}