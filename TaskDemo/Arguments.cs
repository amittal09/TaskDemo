using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertica.Utilities.Comparisons;

namespace TaskDemo
{
    public class Arguments : IEnumerable<KeyValuePair<string, string>>, IEnumerable
    {
        private readonly string _prefix;
        private readonly KeyValuePair<string, string>[] _pairs;
        private readonly Dictionary<string, string> _dictionary;

        public Arguments()
          : this(new string[0])
        {
        }

        public Arguments(params string[] values)
          : this(((IEnumerable<string>)(values ?? new string[0])).Select<string, KeyValuePair<string, string>>((Func<string, KeyValuePair<string, string>>)(x => new KeyValuePair<string, string>(x, x))).ToArray<KeyValuePair<string, string>>())
        {
        }

        public Arguments(params KeyValuePair<string, string>[] pairs)
          : this((string)null, pairs)
        {
        }

        internal Arguments(string prefix, params KeyValuePair<string, string>[] pairs)
        {
            this._prefix = prefix ?? string.Empty;
            Func<KeyValuePair<string, string>, KeyValuePair<string, string>, bool> equals = (Func<KeyValuePair<string, string>, KeyValuePair<string, string>, bool>)((x, y) => string.Equals(x.Key, y.Key, StringComparison.OrdinalIgnoreCase));
            ChainableEqualizer<KeyValuePair<string, string>> chainableEqualizer = Eq<KeyValuePair<string, string>>.By(equals, (Func<KeyValuePair<string, string>, int>)(x => x.Key.ToLowerInvariant().GetHashCode()));
            this._pairs = ((IEnumerable<KeyValuePair<string, string>>)(pairs ?? new KeyValuePair<string, string>[0])).Distinct<KeyValuePair<string, string>>((IEqualityComparer<KeyValuePair<string, string>>)chainableEqualizer).ToArray<KeyValuePair<string, string>>();
            KeyValuePair<string, string>[] pairs1 = this._pairs;
            Func<KeyValuePair<string, string>, string> keySelector = (Func<KeyValuePair<string, string>, string>)(x => x.Key);
            StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
            this._dictionary = ((IEnumerable<KeyValuePair<string, string>>)pairs1).ToDictionary<KeyValuePair<string, string>, string, string>(keySelector, (Func<KeyValuePair<string, string>, string>)(x => x.Value), (IEqualityComparer<string>)ordinalIgnoreCase);
        }

        public bool Contains(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or empty.", nameof(key));
            return this._dictionary.ContainsKey(key);
        }

        public string this[int index]
        {
            get
            {
                return this._pairs[index].Key;
            }
        }

        public string this[string key]
        {
            get
            {
                string str;
                this.TryGetValue(key, out str);
                return str;
            }
        }

        public int Length
        {
            get
            {
                return this._pairs.Length;
            }
        }

        public bool TryGetValue(string key, out string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or empty.", nameof(key));
            return this._dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this._pairs.OfType<KeyValuePair<string, string>>().GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(" ", ((IEnumerable<KeyValuePair<string, string>>)this._pairs).Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>)(x => string.Format("{0}{1}{2}", (object)this._prefix, (object)x.Key, string.IsNullOrWhiteSpace(x.Value) || string.Equals(x.Key, x.Value) ? (object)string.Empty : (object)(":" + x.Value)))));
        }

        public static Arguments Empty
        {
            get
            {
                return new Arguments(new string[0]);
            }
        }
    }
}
