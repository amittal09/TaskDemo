using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
          : this(((IEnumerable<string>)(values ?? new string[0])).Select(x => new KeyValuePair<string, string>(x, x)).ToArray())
        {
        }

        public Arguments(params KeyValuePair<string, string>[] pairs)
          : this((string)null, pairs)
        {
        }

        internal Arguments(string prefix, params KeyValuePair<string, string>[] pairs)
        {
            this._prefix = prefix ?? string.Empty;
            bool equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y) => string.Equals(x.Key, y.Key, StringComparison.OrdinalIgnoreCase);
            ChainableEqualizer<KeyValuePair<string, string>> chainableEqualizer = Eq<KeyValuePair<string, string>>.By(equals, x => x.Key.ToLowerInvariant().GetHashCode());
            _pairs = ((IEnumerable<KeyValuePair<string, string>>)(pairs ?? new KeyValuePair<string, string>[0])).Distinct(chainableEqualizer).ToArray();
            KeyValuePair<string, string>[] pairs1 = _pairs;
            string keySelector(KeyValuePair<string, string> x) => x.Key;
            StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
            _dictionary = ((IEnumerable<KeyValuePair<string, string>>)pairs1).ToDictionary(keySelector, x => x.Value, ordinalIgnoreCase);
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
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _pairs.OfType<KeyValuePair<string, string>>().GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(" ", ((IEnumerable<KeyValuePair<string, string>>)_pairs).Select(x => string.Format("{0}{1}{2}", _prefix, x.Key, string.IsNullOrWhiteSpace(x.Value) || string.Equals(x.Key, x.Value) ? string.Empty : ":" + x.Value)));
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
