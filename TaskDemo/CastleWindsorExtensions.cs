using Castle.MicroKernel.Registration;
using System;

namespace TaskDemo
{
    public static class CastleWindsorExtensions
    {
        public static BasedOnDescriptor Expose(this BasedOnDescriptor descriptor, Action<Type> expose)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));
            if (expose == null)
                throw new ArgumentNullException(nameof(expose));
            return descriptor.If(x =>
            {
                expose(x);
                return true;
            });
        }
    }
}
