﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityCommonLibrary
{
    public class NullLogger : ILogger
    {
        /// <inheritdoc />
        public LogType filterLogType { get; set; }

        /// <inheritdoc />
        public bool logEnabled { get; set; }

        /// <inheritdoc />
        public ILogHandler logHandler { get; set; }

        /// <inheritdoc />
        public bool IsLogTypeAllowed(LogType logType)
        {
            return false;
        }

        /// <inheritdoc />
        public void Log(LogType logType, object message) { }

        /// <inheritdoc />
        public void Log(LogType logType, object message, Object context) { }

        /// <inheritdoc />
        public void Log(LogType logType, string tag, object message) { }

        /// <inheritdoc />
        public void Log(LogType logType, string tag, object message, Object context) { }

        /// <inheritdoc />
        public void Log(object message) { }

        /// <inheritdoc />
        public void Log(string tag, object message) { }

        /// <inheritdoc />
        public void Log(string tag, object message, Object context) { }

        /// <inheritdoc />
        public void LogError(string tag, object message) { }

        /// <inheritdoc />
        public void LogError(string tag, object message, Object context) { }

        /// <inheritdoc />
        public void LogException(Exception exception, Object context) { }

        /// <inheritdoc />
        public void LogException(Exception exception) { }

        /// <inheritdoc />
        public void LogFormat(LogType logType, Object context, string format,
            params object[] args) { }

        /// <inheritdoc />
        public void LogFormat(LogType logType, string format, params object[] args) { }

        /// <inheritdoc />
        public void LogWarning(string tag, object message) { }

        /// <inheritdoc />
        public void LogWarning(string tag, object message, Object context) { }
    }
}