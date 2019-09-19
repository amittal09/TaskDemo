using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    /// <summary>
    /// Startup class to initialize the framework, Castle is initailzed here
    /// </summary>
    public sealed class ApplicationContext : IApplicationContext, IDisposable
    {
        private static readonly Lazy<Action> EnsureSingleton = new Lazy<Action>(() => () => { });
        private readonly Lazy<Action> _disposed = new Lazy<Action>(() => () => { });
        private readonly ApplicationConfiguration _configuration;
        private readonly IWindsorContainer _container;
        private readonly IArgumentsParser _parser;
        private readonly IHost[] _hosts;

        

        internal ApplicationContext(Action<ApplicationConfiguration> application)
        {
            this._configuration = new ApplicationConfiguration();
            application?.Invoke(this._configuration);
            this._configuration.RegisterDependency((IApplicationContext)this);
            this._container = CastleWindsor.Initialize(this._configuration);
            this._parser = this.Resolve<IArgumentsParser>();
            this._hosts = this.Resolve<IHostFactory>().GetAll();
        }

        public static IApplicationContext Create(Action<ApplicationConfiguration> application = null)
        {
            if (EnsureSingleton.IsValueCreated)
                throw new InvalidOperationException("An instance of ApplicationContext has already been created. It might have been disposed, but you should make sure to reuse the same instance for the entire lifecycle of this application.");
            EnsureSingleton.Value();
            return new ApplicationContext(application);
        }

        public T Resolve<T>()
        {
            return this._container.Resolve<T>();
        }

        public T[] ResolveAll<T>()
        {
            return this._container.ResolveAll<T>();
        }

        public void Execute(params string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            this.Execute(this._parser.Parse(args));
        }

        public void Execute(HostArguments args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            IHost[] array = ((IEnumerable<IHost>)_hosts).Where(x => x.CanHandle(args)).ToArray();
            if (array.Length == 0)
                throw new Exception("NoHostFoundException");
            if (array.Length > 1)
                throw new Exception("MultipleHostsFoundException");
            array[0].Handle(args);
        }

        public void Dispose()
        {
            if (this._disposed.IsValueCreated)
                throw new InvalidOperationException("ApplicationContext has already been disposed.");
            this._disposed.Value();
            ((IDisposable)this._container).Dispose();
        }
    }
}
