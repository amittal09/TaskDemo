using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using Vertica.Utilities.Extensions.EnumerableExt;

namespace TaskDemo
{
    /// <summary>
    /// All configuration are setup here such as task, all type of dependency are declared here.
    /// </summary>
    public class ApplicationConfiguration: IInitializable<IWindsorContainer>
    {
        private readonly TasksConfiguration _tasks;
        private readonly List<IWindsorInstaller> _customInstallers;
        private readonly HostsConfiguration _hosts;
        private readonly ExtensibilityConfiguration _extensibility;
        private readonly AdvancedConfiguration _advanced;

        public ApplicationConfiguration()
        {
            _extensibility = new ExtensibilityConfiguration();
            _tasks = Register(() => new TasksConfiguration(this));
            _customInstallers = new List<IWindsorInstaller>();
            _hosts = Register(() => new HostsConfiguration(this));
            _advanced = Register(() => new AdvancedConfiguration(this));

        }

        public void Initialize(IWindsorContainer container)
        {
            container.Install(_customInstallers.ToArray());
        }
        public ApplicationConfiguration AddCustomInstaller(IWindsorInstaller installer)
        {
            return AddCustomInstallers(installer);
        }
        public ApplicationConfiguration RegisterDependency<T>(T singletonInstance) where T : class
        {
            if (singletonInstance == null)
                throw new ArgumentNullException(nameof(singletonInstance));
            return AddCustomInstaller(new InstanceInstaller<T>(singletonInstance));
        }
        public ApplicationConfiguration AddCustomInstallers(params IWindsorInstaller[] installers)
        {
            if (installers != null)
                _customInstallers.AddRange(((IEnumerable<IWindsorInstaller>)installers).SkipNulls());
            return this;
        }
        public ApplicationConfiguration Tasks(Action<TasksConfiguration> tasks)
        {
            tasks?.Invoke(this._tasks);
            return this;
        }
        public ApplicationConfiguration Hosts(Action<HostsConfiguration> hosts)
        {
            hosts?.Invoke(this._hosts);
            return this;
        }
        public ApplicationConfiguration Extensibility(Action<ExtensibilityConfiguration> extensibility)
        {
            extensibility?.Invoke(this._extensibility);
            return this;
        }
        private T Register<T>(Func<T> factory) where T : class
        {
            T result = default(T);
            this.Extensibility(extensibility => result = extensibility.Register<T>(factory));
            return result;
        }
    }
}
