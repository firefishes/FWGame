using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShipDock.Tools
{
    static public class StringUtils
    {
        public static string GetQualifiedClassName(object target)
        {
            if(target == null)
            {
                return string.Empty;
            }
            Type type = target.GetType();
            return type.FullName;
        }
    }

}
