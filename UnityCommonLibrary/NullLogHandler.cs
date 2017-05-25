using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityCommonLibrary
{
    public class NullLogHandler : ILogHandler
    {
        /// <inheritdoc />
        public void LogException(Exception exception, Object context) { }

        /// <inheritdoc />
        public void LogFormat(LogType logType, Object context, string format,
            params object[] args) { }
    }
}