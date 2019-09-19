using System;
using System.Collections;
using System.Collections.Generic;

namespace TaskDemo
{
    public class ExtensibilityConfiguration : IEnumerable<object>, IEnumerable
    {
        private readonly Dictionary<Type, object> _extensions;

        internal ExtensibilityConfiguration()
        {
            this._extensions = new Dictionary<Type, object>();
        }

        public T Register<T>(Func<T> factory) where T : class
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            object obj1;
            T obj2;
            if (this._extensions.TryGetValue(typeof(T), out obj1))
                obj2 = (T)obj1;
            else
                this._extensions[typeof(T)] = (object)(obj2 = factory());
            return obj2;
        }

        internal T Get<T>() where T : class
        {
            return this._extensions[typeof(T)] as T;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return (IEnumerator<object>)this._extensions.Values.GetEnumerator();
        }
    }
}
