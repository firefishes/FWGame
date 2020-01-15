using ShipDock.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShipDock.Applications
{
    public static class AppPaths
    {
        public static string resDataRoot = "ResData/";

        public static string StreamingResDataRoot { get; } = Application.streamingAssetsPath.Append(StringUtils.PATH_SYMBOL, resDataRoot);
        public static string DataPathResDataRoot { get; } = Application.dataPath.Append(StringUtils.PATH_SYMBOL, resDataRoot);
    }

}