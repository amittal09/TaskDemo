using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskDemo
{
    public class AdvancedConfiguration : IInitializable<IWindsorContainer>
    {
        private readonly IDictionary<Type, Tuple<Type, Type>> _types;
        private readonly IDictionary<Type, Tuple<Func<object>, Func<object>>> _instances;
        internal AdvancedConfiguration(ApplicationConfiguration application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));
            Application = application;
            _types = new Dictionary<Type, Tuple<Type, Type>>();
            _instances = new Dictionary<Type, Tuple<Func<object>, Func<object>>>();
            Register(() =>
            {
                if (!Environment.UserInteractive)
                    return TextWriter.Null;
                return Console.Out;
            });
        }

        public ApplicationConfiguration Application { get; }

        public AdvancedConfiguration Register<TService, TImpl>() where TService : class where TImpl : TService
        {
            return this.Register<TService, TImpl, TImpl>();
        }

        public AdvancedConfiguration Register<TService, TIntegrationDbEnabledImpl, TIntegrationDbDisabledImpl>() where TService : class where TIntegrationDbEnabledImpl : TService where TIntegrationDbDisabledImpl : TService
        {
            _types[typeof(TService)] = Tuple.Create(typeof(TIntegrationDbEnabledImpl), typeof(TIntegrationDbDisabledImpl));
            _instances[typeof(TService)] = null;
            return this;
        }

        public AdvancedConfiguration Register<TService>(Func<TService> instance) where TService : class
        {
            return this.Register(instance, instance);
        }

        public AdvancedConfiguration Register<TService>(Func<TService> integrationDbEnabledInstance, Func<TService> integrationDbDisabledInstance) where TService : class
        {
            if (integrationDbEnabledInstance == null)
                throw new ArgumentNullException(nameof(integrationDbEnabledInstance));
            if (integrationDbDisabledInstance == null)
                throw new ArgumentNullException(nameof(integrationDbDisabledInstance));
            _instances[typeof(TService)] = Tuple.Create((Func<object>)integrationDbEnabledInstance, (Func<object>)integrationDbDisabledInstance);
            _types[typeof(TService)] = null;
            return this;
        }

       

        void IInitializable<IWindsorContainer>.Initialize(IWindsorContainer container)
        {
            bool disabled = false;
            foreach (KeyValuePair<Type, Tuple<Type, Type>> keyValuePair in _types.Where(x => x.Value != null))
                container.Register(new IRegistration[1]
                {
            Component.For(keyValuePair.Key).ImplementedBy(!disabled ? keyValuePair.Value.Item1 : keyValuePair.Value.Item2)
                });
            foreach (KeyValuePair<Type, Tuple<Func<object>, Func<object>>> keyValuePair in _instances.Where(x => x.Value != null))
                container.Register(new IRegistration[1]
                {
            Component.For(keyValuePair.Key).Instance(!disabled ? keyValuePair.Value.Item1() : keyValuePair.Value.Item2())
                });
        }
    }
}