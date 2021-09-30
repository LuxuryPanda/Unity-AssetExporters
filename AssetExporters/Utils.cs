/***
 *
 * @Author: Roman Boryslavskyy
 * @Created on: 30/09/21
 *
 ***/

using UnityEngine;

namespace AssetExporters
{
    internal static class Utils
    {
        #region ## Variables ##

        internal static bool enableLogs = false;

        #endregion

        #region ## Debug Messages ##

        public static void Log(string message)
        {
            if (enableLogs) Debug.Log("<b>[AssetExporters]</b>: " + message);
        }

        #endregion
    }
}