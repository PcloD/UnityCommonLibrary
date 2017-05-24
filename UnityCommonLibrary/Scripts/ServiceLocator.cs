using System;
using System.Collections.Generic;
using UnityCommonLibrary.Attributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityCommonLibrary
{
    public abstract class ServiceLocator : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services =
            new Dictionary<Type, object>();

        public virtual bool RegisterNullServices
        {
            get { return false; }
        }

        protected ServiceLocator()
        {
            RegisterServices();
        }

        /// <summary>
        ///     Retrieves a service by generic StateType.
        /// </summary>
        public TS Get<TS>() where TS : class
        {
            return (TS) GetService(typeof(TS));
        }

        /// <summary>
        ///     Retrieves service by StateType. IServiceProvider implementation.
        /// </summary>
        public object GetService(Type serviceType)
        {
            object service;
            if ((!_services.TryGetValue(serviceType, out service) || service == null) &&
                RegisterNullServices)
            {
                service = RegisterNullService(serviceType);
            }
            return service;
        }

        /// <summary>
        ///     Creates a new provider instance.
        /// </summary>
        /// <typeparam name="S">Service StateType</typeparam>
        /// <typeparam name="P">Provider StateType</typeparam>
        protected TS Register<TS, TP>() where TS : class where TP : TS, new()
        {
            return Register<TS>(new TP());
        }

        /// <summary>
        ///     Binds an existing provider instance to a service.
        /// </summary>
        /// <typeparam name="S">Service StateType</typeparam>
        /// <typeparam name="P">Provider StateType</typeparam>
        protected TS Register<TS>(TS provider) where TS : class
        {
            return (TS) Register(typeof(TS), provider);
        }

        protected object Register(Type type, object provider)
        {
            if (_services.ContainsKey(type))
            {
                _services[type] = provider;
            }
            else
            {
                _services.Add(type, provider);
            }
            return provider;
        }

        /// <summary>
        ///     Binds an existing MonoBehaviour provider instance to a service.
        /// </summary>
        /// <typeparam name="S">Service StateType</typeparam>
        /// <typeparam name="P">Provider StateType</typeparam>
        protected TS RegisterBehaviour<TS, TP>(TP provider, bool dontDestroy = true)
            where TS : class where TP : MonoBehaviour, TS
        {
            if (dontDestroy)
            {
                Object.DontDestroyOnLoad(provider);
            }
            return Register<TS>(provider);
        }

        /// <summary>
        ///     Instantiates a new MonoBehaviour provider instance.
        /// </summary>
        /// <typeparam name="S">Service StateType</typeparam>
        /// <typeparam name="P">Provider StateType</typeparam>
        protected TS RegisterNewBehaviour<TS, TP>(bool dontDestroy = true)
            where TS : class where TP : MonoBehaviour, TS
        {
            return RegisterBehaviour<TS, TP>(
                new GameObject(typeof(TP).Name).AddComponent<TP>(), dontDestroy);
        }

        /// <summary>
        ///     Creates a new ScriptableObject provider instance.
        /// </summary>
        /// <typeparam name="S">Service StateType</typeparam>
        /// <typeparam name="P">Provider StateType</typeparam>
        protected TS RegisterNewScriptable<TS, TP>()
            where TS : class where TP : ScriptableObject, TS
        {
            return Register<TS>(ScriptableObject.CreateInstance<TP>());
        }

        protected abstract void RegisterServices();

        private object RegisterNullService(Type serviceType)
        {
            var allTypes = serviceType.Assembly.GetTypes();
            for (var i = 0; i < allTypes.Length; i++)
            {
                var t = allTypes[i];
                if (serviceType.IsAssignableFrom(t) &&
                    t.GetCustomAttributes(typeof(NullProviderAttribute), false).Length >
                    0)
                {
                    return Register(Activator.CreateInstance(t));
                }
            }
            throw new Exception("No null service for " + serviceType.FullName);
        }
    }
}