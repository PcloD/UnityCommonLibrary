using UnityEngine;

namespace UnityCommonLibrary
{
    /// <summary>
    ///     Centralizes logging for UCL.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class UCLCore
    {
        public static ILogger Logger = Debug.unityLogger;
    }
}