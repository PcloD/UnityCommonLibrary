using System;
using System.Collections.Generic;
using UnityCommonLibrary.Attributes;
using UnityEngine;

namespace UnityCommonLibrary
{
    public abstract class ServiceLocator : IServiceProvider
    {
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public virtual bool registerNullServices
        {
            get
            {
                return false;
            }
        }

        public ServiceLocator()
        {
            RegisterServices();
        }

        /// <summary>
        /// Retrieves a service by generic type.
        /// </summary>
        public S Get<S>() where S : class
        {
            return (S)GetService(typeof(S));
        }
        /// <summary>
        /// Retrieves service by type. IServiceProvider implementation.
        /// </summary>
        public object GetService(Type serviceType)
        {
            object service = null;
            if ((!services.TryGetValue(serviceType, out service) || service == null) && registerNullServices)
            {
                service = RegisterNullService(serviceType);
            }
            return service;
        }
        /// <summary>
        /// Instantiates a new MonoBehaviour provider instance.
        /// </summary>
        /// <typeparam name="S">Service type</typeparam>
        /// <typeparam name="P">Provider type</typeparam>
        protected S RegisterNewBehaviour<S, P>(bool dontDestroy = true) where S : class where P : MonoBehaviour, S
        {
            return RegisterBehaviour<S, P>(new GameObject(typeof(P).Name).AddComponent<P>(), dontDestroy);
        }
        /// <summary>
        /// Binds an existing MonoBehaviour provider instance to a service.
        /// </summary>
        /// <typeparam name="S">Service type</typeparam>
        /// <typeparam name="P">Provider type</typeparam>
        protected S RegisterBehaviour<S, P>(P provider, bool dontDestroy = true) where S : class where P : MonoBehaviour, S
        {
            if (dontDestroy)
            {
                UnityEngine.Object.DontDestroyOnLoad(provider);
            }
            return Register<S>(provider);
        }
        /// <summary>
        /// Creates a new ScriptableObject provider instance.
        /// </summary>
        /// <typeparam name="S">Service type</typeparam>
        /// <typeparam name="P">Provider type</typeparam>
        protected S RegisterNewScriptable<S, P>() where S : class where P : ScriptableObject, S
        {
            return Register<S>(ScriptableObject.CreateInstance<P>());
        }
        /// <summary>
        /// Creates a new provider instance.
        /// </summary>
        /// <typeparam name="S">Service type</typeparam>
        /// <typeparam name="P">Provider type</typeparam>
        protected S Register<S, P>() where S : class where P : S, new()
        {
            return Register<S>(new P());
        }
        /// <summary>
        /// Binds an existing provider instance to a service.
        /// </summary>
        /// <typeparam name="S">Service type</typeparam>
        /// <typeparam name="P">Provider type</typeparam>
        protected S Register<S>(S provider) where S : class
        {
            return (S)Register(typeof(S), provider);
        }
        protected object Register(Type type, object provider)
        {
            if (services.ContainsKey(type))
            {
                services[type] = provider;
            }
            else
            {
                services.Add(type, provider);
            }
            return provider;
        }
        protected abstract void RegisterServices();
        private object RegisterNullService(Type serviceType)
        {
            var allTypes = serviceType.Assembly.GetTypes();
            for (int i = 0; i < allTypes.Length; i++)
            {
                var t = allTypes[i];
                if (serviceType.IsAssignableFrom(t) && t.GetCustomAttributes(typeof(NullProviderAttribute), false).Length > 0)
                {
                    return Register(Activator.CreateInstance(t));
                }
            }
            throw new Exception("No null service for " + serviceType.FullName);
        }
    }
}