using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.Windsor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TaskDemo
{
    internal static class CastleWindsor
    {
        public static IWindsorContainer Initialize(ApplicationConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            WindsorContainer container = new WindsorContainer();
            container.Kernel.AddFacility<TypedFactoryFacility>();
            container.Register(new IRegistration[1]
            {
          Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>()
            });
            configuration.Extensibility(extensibility =>
            {
                using (IEnumerator<IInitializable<IWindsorContainer>> enumerator = extensibility.OfType<IInitializable<IWindsorContainer>>().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                        enumerator.Current.Initialize(container);
                }
            });
            container.Install(new IWindsorInstaller[1]
            {
         Install.ByConvention.AddFromAssemblyOfThis<ConventionInstaller>().Ignore<IApplicationContext>().Ignore<IHost>().Ignore<ITask>().Ignore<IStep>()
            });
            return container;
        }
    }
}